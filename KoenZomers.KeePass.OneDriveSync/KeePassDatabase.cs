using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeePass.DataExchange;
using KeePassLib.Serialization;
using KoenZomers.KeePass.OneDriveSync;
using KoenZomers.KeePass.OneDriveSync.Enums;
using KoenZomers.OneDrive.Api.Entities;
using KoenZomersKeePassOneDriveSync.Forms;

namespace KoenZomersKeePassOneDriveSync
{
    /// <summary>
    /// Methods that regard the KeePass database
    /// </summary>
    public static class KeePassDatabase
    {
        /// <summary>
        /// Checks if a newer database exists on OneDrive compared to the locally opened version and syncs them if so
        /// </summary>
        /// <param name="localKeePassDatabasePath">Full path to where the KeePass database resides locally</param>
        /// <param name="updateStatus">Method to call to update the status</param>
        /// <param name="forceSync">If True, no attempts will be made to validate if the local copy differs from the remote copy and it will always sync. If False, it will try to validate if there are any changes and not sync if there are no changes.</param>
        public static async Task SyncDatabase(string localKeePassDatabasePath, Action<string> updateStatus, bool forceSync)
        {
            // Retrieve the KeePassOneDriveSync settings
            var databaseConfig = Configuration.GetPasswordDatabaseConfiguration(localKeePassDatabasePath);

            // Check if this database explicitly does not allow to be synced with OneDrive and if syncing is enabled on this database
            if (databaseConfig.DoNotSync || !databaseConfig.SyncingEnabled)
            {
                return;
            }

            // Check if the current database is synced with OneDrive
            if (string.IsNullOrEmpty(databaseConfig.RemoteDatabasePath) && string.IsNullOrEmpty(databaseConfig.RemoteFolderId) && string.IsNullOrEmpty(databaseConfig.RemoteFileName))
            {
                // Current database is not being syned with OneDrive, ask if it should be synced
                var oneDriveAskToStartSyncingForm = new OneDriveAskToStartSyncingDialog(databaseConfig);
                if (oneDriveAskToStartSyncingForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                // Ask which cloud service to connect to
                var oneDriveCloudTypeForm = new OneDriveCloudTypeForm(databaseConfig);
                if (oneDriveCloudTypeForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            // Connect to OneDrive
            var oneDriveApi = await Utilities.GetOneDriveApi(databaseConfig);

            if (oneDriveApi == null)
            {
                switch (databaseConfig.CloudStorageType.GetValueOrDefault(CloudStorageType.OneDriveConsumer))
                {
                    case CloudStorageType.OneDriveConsumer: updateStatus("Failed to connect to OneDrive"); break;
                    case CloudStorageType.OneDriveForBusiness: updateStatus("Failed to connect to OneDrive for Business"); break;
                    default: updateStatus("Failed to connect to cloud service");break;
                }                
                return;
            }

            // Save the RefreshToken in the configuration so we can use it again next time
            databaseConfig.RefreshToken = oneDriveApi.AccessToken.RefreshToken;

            // Verify if we already have retrieved the name of this OneDrive
            if (string.IsNullOrEmpty(databaseConfig.OneDriveName))
            {
                // Fetch details about this OneDrive account
                var oneDriveAccount = await oneDriveApi.GetDrive();
                databaseConfig.OneDriveName = oneDriveAccount.Owner.User.DisplayName;
            }
            Configuration.Save();

            // Check if we have a location on OneDrive to sync with
            if (string.IsNullOrEmpty(databaseConfig.RemoteDatabasePath) && string.IsNullOrEmpty(databaseConfig.RemoteFolderId) && string.IsNullOrEmpty(databaseConfig.RemoteFileName))
            {
                // Have the user enter a location for the database on OneDrive 
                var oneDriveFileSaveAsDialog = new OneDriveFileSaveAsDialog(oneDriveApi);                
                await oneDriveFileSaveAsDialog.LoadFolderItems();
                var result1 = oneDriveFileSaveAsDialog.ShowDialog();
                if (result1 != DialogResult.OK || string.IsNullOrEmpty(oneDriveFileSaveAsDialog.FileName))
                {
                    return;
                }                
                databaseConfig.RemoteDatabasePath = oneDriveFileSaveAsDialog.CurrentOneDriveItem.ParentReference?.Path + "/" + oneDriveFileSaveAsDialog.CurrentOneDriveItem.Name + "/" + oneDriveFileSaveAsDialog.FileName;
                databaseConfig.RemoteFolderId = oneDriveFileSaveAsDialog.CurrentOneDriveItem.Id;
                databaseConfig.RemoteFileName = oneDriveFileSaveAsDialog.FileName;
                Configuration.Save();
            }

            // Retrieve the KeePass database from OneDrive
            var oneDriveItem = string.IsNullOrEmpty(databaseConfig.RemoteFolderId) ? await oneDriveApi.GetItem(databaseConfig.RemoteDatabasePath) : await oneDriveApi.GetItemInFolder(databaseConfig.RemoteFolderId, databaseConfig.RemoteFileName);

            if (oneDriveItem == null)
            {
                // KeePass database not found on OneDrive
                updateStatus("Database does not exist yet on OneDrive, uploading it now");

                OneDriveItem oneDriveFolder;
                string fileName;
                if (string.IsNullOrEmpty(databaseConfig.RemoteFolderId))
                {
                    oneDriveFolder = databaseConfig.RemoteDatabasePath.Contains("/") ? await oneDriveApi.GetFolderOrCreate(databaseConfig.RemoteDatabasePath.Remove(databaseConfig.RemoteDatabasePath.LastIndexOf("/", StringComparison.Ordinal))) : await oneDriveApi.GetDriveRoot();
                    fileName = databaseConfig.RemoteDatabasePath.Contains("/") ? databaseConfig.RemoteDatabasePath.Remove(0, databaseConfig.RemoteDatabasePath.LastIndexOf("/", StringComparison.Ordinal) + 1) : databaseConfig.RemoteDatabasePath;
                }
                else
                {
                    oneDriveFolder = await oneDriveApi.GetItemById(databaseConfig.RemoteFolderId);
                    fileName = databaseConfig.RemoteFileName;
                }

                if (oneDriveFolder == null)
                {
                    updateStatus("Unable to upload database to OneDrive. Remote path is invalid.");
                    return;
                }

                // Upload the database to OneDrive                
                var newUploadResult = await oneDriveApi.UploadFileAs(localKeePassDatabasePath, fileName, oneDriveFolder);

                updateStatus(newUploadResult == null ? "Failed to upload the KeePass database" : "Successfully uploaded the new KeePass database to OneDrive");

                databaseConfig.LocalFileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);
                if (newUploadResult != null)
                {
                    databaseConfig.LastCheckedAt = DateTime.Now;
                    databaseConfig.LastSyncedAt = DateTime.Now;
                    databaseConfig.ETag = newUploadResult.ETag;
                }
                Configuration.Save();
                return;
            }

            // Use the ETag from the OneDrive item to compare it against the local database config etag to see if the content has changed
            if (!forceSync && oneDriveItem.ETag == databaseConfig.ETag)
            {
                updateStatus("Databases are in sync");

                databaseConfig.LastCheckedAt = DateTime.Now;
                Configuration.Save();

                return;
            }

            // Download the database from OneDrive
            updateStatus("Downloading KeePass database from OneDrive");

            var temporaryKeePassDatabasePath = Path.GetTempFileName();
            var downloadSuccessful = await oneDriveApi.DownloadItemAndSaveAs(oneDriveItem, temporaryKeePassDatabasePath);

            if (!downloadSuccessful)
            {
                updateStatus("Failed to download the KeePass database from OneDrive");
                return;
            }

            // Sync database
            updateStatus("KeePass database downloaded, going to sync");

            var connectionInfo = IOConnectionInfo.FromPath(temporaryKeePassDatabasePath);
            var formatter = KoenZomersKeePassOneDriveSyncExt.Host.FileFormatPool.Find("KeePass KDBX (2.x)");

            // Temporarily switch off syncing for this database to avoid the import operation, which causes a save, to create and endless loop
            databaseConfig.SyncingEnabled = false;

            // Import the current database with the downloaded database. Import causes a one way sync, syncing would try to update both ends which won't work for OneDrive.
            var importSuccessful = ImportUtil.Import(KoenZomersKeePassOneDriveSyncExt.Host.Database, formatter, new[] { connectionInfo }, true, KoenZomersKeePassOneDriveSyncExt.Host.MainWindow, false, KoenZomersKeePassOneDriveSyncExt.Host.MainWindow);            

            // Enable syncing of this database again
            databaseConfig.SyncingEnabled = true;

            // Remove the temporary database from the Most Recently Used (MRU) list in KeePass as its added automatically on the import action
            KoenZomersKeePassOneDriveSyncExt.Host.MainWindow.FileMruList.RemoveItem(temporaryKeePassDatabasePath);

            if (!importSuccessful.GetValueOrDefault(false))
            {
                updateStatus("Failed to synchronize the KeePass databases");
                return;
            }

            // Upload the synced database
            updateStatus("Uploading the new KeePass database to OneDrive");

            var uploadResult = await oneDriveApi.UploadFileAs(temporaryKeePassDatabasePath, oneDriveItem.Name, oneDriveItem.ParentReference.Path.Equals("/drive/root:", StringComparison.CurrentCultureIgnoreCase) ? await oneDriveApi.GetDriveRoot() : await oneDriveApi.GetItemById(oneDriveItem.ParentReference.Id));

            if (uploadResult == null)
            {
                updateStatus("Failed to upload the KeePass database");
                return;
            }            

            // Delete the temporary database used for merging
            File.Delete(temporaryKeePassDatabasePath);
            
            // Update the KeePass UI so it shows the new entries
            KoenZomersKeePassOneDriveSyncExt.Host.MainWindow.UpdateUI(false, null, true, null, true, null, false);

            updateStatus("KeePass database has successfully been synchronized and uploaded");

            databaseConfig.LocalFileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);
            databaseConfig.ETag = uploadResult.ETag;
            databaseConfig.LastSyncedAt = DateTime.Now;
            databaseConfig.LastCheckedAt = DateTime.Now;
            Configuration.Save();
        }

        /// <summary>
        /// Displays a status message on the bottom bar
        /// </summary>
        /// <param name="message">Message to show</param>
        public static void UpdateStatus(string message)
        {
            KoenZomersKeePassOneDriveSyncExt.Host.MainWindow.SetStatusEx(message);
        }
    }
}
