using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeePass.DataExchange;
using KeePassLib.Serialization;
using KoenZomers.KeePass.OneDriveSync;

namespace KoenZomersKeePassOneDriveSync
{
    /// <summary>
    /// Methods that regard the KeePass database
    /// </summary>
    public static class KeePass
    {
        /// <summary>
        /// Checks if a newer database exists on OneDrive compared to the locally opened version and syncs them if so
        /// </summary>
        /// <param name="localKeePassDatabasePath">Full path to where the KeePass database resides locally</param>
        /// <param name="updateStatus">Method to call to update the status</param>
        public static async Task SyncDatabase(string localKeePassDatabasePath, Action<string> updateStatus)
        {
            // Retrieve the KeePassOneDriveSync settings
            var databaseConfig = Configuration.GetPasswordDatabaseConfiguration(localKeePassDatabasePath);

            // Check if this database explicitly does not allow to be synced with OneDrive and if syncing is enabled on this database
            if (databaseConfig.DoNotSync || !databaseConfig.SyncingEnabled)
            {
                return;
            }

            // Check if the current database is syned with OneDrive
            if (string.IsNullOrEmpty(databaseConfig.RemoteDatabasePath))
            {
                // Current database is not being syned with OneDrive, ask if it should be synced
                var oneDriveAskToStartSyncingForm = new OneDriveAskToStartSyncingDialog(databaseConfig);
                var result = oneDriveAskToStartSyncingForm.ShowDialog();

                if (result != DialogResult.OK)
                {
                    return;
                }
            }

            // Connect to OneDrive
            var oneDriveApi = await Utilities.GetOneDriveApi(databaseConfig, KoenZomersKeePassOneDriveSyncExt.OneDriveClientId, KoenZomersKeePassOneDriveSyncExt.OneDriveClientSecret);

            if (oneDriveApi == null)
            {
                updateStatus("Failed to connect to OneDrive");
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
            if (string.IsNullOrEmpty(databaseConfig.RemoteDatabasePath))
            {
                // Have the user enter a location for the database on OneDrive 
                var oneDriveRemoteLocationForm = new OneDriveRemoteLocationDialog(databaseConfig);
                var result = oneDriveRemoteLocationForm.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrWhiteSpace(oneDriveRemoteLocationForm.OneDriveRemotePathTextBox.Text))
                {
                    return;
                }
                databaseConfig.RemoteDatabasePath = oneDriveRemoteLocationForm.OneDriveRemotePathTextBox.Text.Replace('\\', '/').TrimStart('/');
                Configuration.Save();
            }

            // Retrieve the KeePass database from OneDrive
            var oneDriveItem = await oneDriveApi.GetItem(databaseConfig.RemoteDatabasePath);

            if (oneDriveItem == null)
            {
                // KeePass database not found on OneDrive
                updateStatus("Database does not exist yet on OneDrive, uploading it now");

                // Create or retrieve the folder in which to store the database on OneDrive
                var oneDriveFolder = databaseConfig.RemoteDatabasePath.Contains("/") ? await oneDriveApi.GetFolderOrCreate(databaseConfig.RemoteDatabasePath.Remove(databaseConfig.RemoteDatabasePath.LastIndexOf("/", StringComparison.Ordinal))) : await oneDriveApi.GetDriveRoot();
                if (oneDriveFolder == null)
                {
                    updateStatus("Unable to upload database to OneDrive. Remote path is invalid.");
                    return;
                }
                
                // Upload the database to OneDrive
                var fileName = databaseConfig.RemoteDatabasePath.Contains("/") ? databaseConfig.RemoteDatabasePath.Remove(0, databaseConfig.RemoteDatabasePath.LastIndexOf("/", StringComparison.Ordinal) + 1) : databaseConfig.RemoteDatabasePath;                
                var newUploadResult = await oneDriveApi.UploadFileAs(localKeePassDatabasePath, fileName, oneDriveFolder);

                updateStatus(newUploadResult == null ? "Failed to upload the KeePass database" : "Successfully uploaded the new KeePass database to OneDrive");

                databaseConfig.LocalFileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);
                if (newUploadResult != null)
                {
                    databaseConfig.LastCheckedAt = DateTime.Now;
                    databaseConfig.LastSyncedAt = DateTime.Now;                                       
                }
                Configuration.Save();
                return;
            }

            // Calculate the SHA1 hash of the local KeePass database
            var fileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);

            databaseConfig.LocalFileHash = fileHash;
            databaseConfig.LastCheckedAt = DateTime.Now;
            Configuration.Save();

            if (oneDriveItem.File.Hashes.Sha1.Equals(fileHash))
            {
                updateStatus("Databases are in sync");
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

            databaseConfig.LastSyncedAt = DateTime.Now;
            Configuration.Save();

            // Upload the synced database
            updateStatus("Uploading the new KeePass database to OneDrive");

            var uploadResult = await oneDriveApi.UploadFileAs(temporaryKeePassDatabasePath, new FileInfo(localKeePassDatabasePath).Name, oneDriveItem.ParentReference.Path.Equals("/drive/root:", StringComparison.CurrentCultureIgnoreCase) ? await oneDriveApi.GetDriveRoot() : await oneDriveApi.GetItem(oneDriveItem.ParentReference.Path.Remove(0, 13)));

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
