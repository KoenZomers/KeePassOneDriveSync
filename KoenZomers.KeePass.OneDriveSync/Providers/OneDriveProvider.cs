using KoenZomers.KeePass.OneDriveSync;
using KoenZomers.KeePass.OneDriveSync.Enums;
using KoenZomers.OneDrive.Api;
using KoenZomers.OneDrive.Api.Entities;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KoenZomersKeePassOneDriveSync.Providers
{
    internal static class OneDriveProvider
    {
        /// <summary>
        /// Uses a Microsoft OneDrive Cloud Storage Provider (OneDrive Consumer, OneDrive for Business, Microsoft Graph) to sync the KeePass database
        /// </summary>
        /// <param name="databaseConfig">Configuration of the database to sync</param>
        /// <param name="localKeePassDatabasePath">Path to where the KeePass database to sync resides</param>
        /// <param name="forceSync">Flag to indicate if the sync should always take place</param>
        /// <param name="updateStatus">Action to write status messages to to display in the UI</param>
        /// <returns>True if successful, false if failed</returns>
        public static async Task<bool> SyncUsingOneDriveCloudProvider(Configuration databaseConfig, string localKeePassDatabasePath, bool forceSync, Action<string> updateStatus)
        {
            // Connect to OneDrive
            OneDriveApi oneDriveApi = null;
            bool retryGettingApiInstance;
            do
            {
                retryGettingApiInstance = false;
                try
                {
                    oneDriveApi = await Utilities.GetOneDriveApi(databaseConfig);
                }
                catch (KoenZomers.OneDrive.Api.Exceptions.TokenRetrievalFailedException e)
                {
                    // Failed to get a OneDrive API instance because retrieving an oAuth token failed. This could be because the oAuth token expired or has been removed. Show the login dialog again so we can get a new token.
                    databaseConfig.RefreshToken = null;
                    retryGettingApiInstance = true;
                }
                catch (Exception e)
                {
                    // Build the error text to show to the end user
                    var errorMessage = new System.Text.StringBuilder();
                    errorMessage.Append("Failed to connect to ");
                    switch (databaseConfig.CloudStorageType.GetValueOrDefault(CloudStorageType.OneDriveConsumer))
                    {
                        case CloudStorageType.OneDriveConsumer:
                            errorMessage.Append("OneDrive");
                            break;
                        case CloudStorageType.OneDriveForBusiness:
                            errorMessage.Append("OneDrive for Business");
                            break;
                        case CloudStorageType.MicrosoftGraph:
                            errorMessage.Append("Microsoft Graph");
                            break;
                        default:
                            errorMessage.Append("cloud storage provider");
                            break;
                    }
                    errorMessage.AppendLine(":");
                    errorMessage.AppendLine();
                    errorMessage.AppendLine(e.Message);

                    // If there's an inner exception, show its message as well as it typically gives more detail why it went wrong
                    if (e.InnerException != null)
                    {
                        errorMessage.AppendLine(e.InnerException.Message);

                        // Verify if we're offline
                        if (e.InnerException.Message.Contains("remote name could not be resolved"))
                        {
                            // Offline, don't display a modal dialog but use the status bar instead
                            KeePassDatabase.UpdateStatus("Can't connect. Working offline.");
                            return false;
                        }
                    }

                    MessageBox.Show(errorMessage.ToString(), "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
            } while (retryGettingApiInstance);

            if (oneDriveApi == null)
            {
                switch (databaseConfig.CloudStorageType.GetValueOrDefault(CloudStorageType.OneDriveConsumer))
                {
                    case CloudStorageType.OneDriveConsumer: updateStatus("Failed to connect to OneDrive"); break;
                    case CloudStorageType.OneDriveForBusiness: updateStatus("Failed to connect to OneDrive for Business"); break;
                    case CloudStorageType.MicrosoftGraph: updateStatus("Failed to connect to Microsoft Graph"); break;
                    default: updateStatus("Failed to connect to cloud service"); break;
                }

                return false;
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
                // Ask the user where to store the database on OneDrive
                var oneDriveFilePickerDialog = new Forms.OneDriveFilePickerDialog(oneDriveApi)
                {
                    ExplanationText = "Select where you want to store the KeePass database. Right click for additional options.",
                    AllowEnteringNewFileName = true
                };
                await oneDriveFilePickerDialog.LoadFolderItems();
                var result = oneDriveFilePickerDialog.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrEmpty(oneDriveFilePickerDialog.FileName))
                {
                    return false;
                }
                databaseConfig.RemoteDatabasePath = (oneDriveFilePickerDialog.CurrentOneDriveItem.ParentReference != null ? oneDriveFilePickerDialog.CurrentOneDriveItem.ParentReference.Path : "") + "/" + oneDriveFilePickerDialog.CurrentOneDriveItem.Name + "/" + oneDriveFilePickerDialog.FileName;
                databaseConfig.RemoteFolderId = oneDriveFilePickerDialog.CurrentOneDriveItem.Id;
                databaseConfig.RemoteFileName = oneDriveFilePickerDialog.FileName;
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
                    return false;
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
                return false;
            }

            // Use the ETag from the OneDrive item to compare it against the local database config etag to see if the content has changed
            if (!forceSync && oneDriveItem.ETag == databaseConfig.ETag)
            {
                updateStatus("Databases are in sync");

                databaseConfig.LastCheckedAt = DateTime.Now;
                Configuration.Save();

                return false;
            }

            // Download the database from OneDrive
            updateStatus("Downloading KeePass database from OneDrive");

            var temporaryKeePassDatabasePath = System.IO.Path.GetTempFileName();
            var downloadSuccessful = await oneDriveApi.DownloadItemAndSaveAs(oneDriveItem, temporaryKeePassDatabasePath);

            if (!downloadSuccessful)
            {
                updateStatus("Failed to download the KeePass database from OneDrive");

                return false;
            }

            // Sync database
            updateStatus("KeePass database downloaded, going to sync");

            // Ensure the database that needs to be synced is still the database currently selected in KeePass to avoid merging the downloaded database with the wrong database in KeePass
            if ((!KoenZomersKeePassOneDriveSyncExt.Host.Database.IOConnectionInfo.Path.StartsWith(Environment.CurrentDirectory) && KoenZomersKeePassOneDriveSyncExt.Host.Database.IOConnectionInfo.Path != localKeePassDatabasePath) ||
                (KoenZomersKeePassOneDriveSyncExt.Host.Database.IOConnectionInfo.Path.StartsWith(Environment.CurrentDirectory) && KoenZomersKeePassOneDriveSyncExt.Host.Database.IOConnectionInfo.Path.Remove(0, Environment.CurrentDirectory.Length + 1) != localKeePassDatabasePath))
            {
                updateStatus("Failed to sync. Please don't switch to another database before done.");

                return false;
            }

            // Merge the downloaded database with the currently open KeePass database
            var syncSuccessful = KeePassDatabase.MergeDatabases(databaseConfig, temporaryKeePassDatabasePath);

            if (!syncSuccessful)
            {
                updateStatus("Failed to synchronize the KeePass databases");
                return false;
            }

            // Upload the synced database
            updateStatus("Uploading the new KeePass database to OneDrive");

            var uploadResult = await oneDriveApi.UploadFileAs(temporaryKeePassDatabasePath, oneDriveItem.Name, oneDriveItem.ParentReference.Path.Equals("/drive/root:", StringComparison.CurrentCultureIgnoreCase) ? await oneDriveApi.GetDriveRoot() : await oneDriveApi.GetItemById(oneDriveItem.ParentReference.Id));
            if (uploadResult == null)
            {
                updateStatus("Failed to upload the KeePass database");
                return false;
            }

            // Delete the temporary database used for merging
            System.IO.File.Delete(temporaryKeePassDatabasePath);

            databaseConfig.ETag = uploadResult.ETag;
            return true;
        }

        /// <summary>
        /// Uses a Microsoft OneDrive Cloud Storage Provider (OneDrive Consumer, OneDrive for Business, Microsoft Graph) to download a KeePass database
        /// </summary>
        /// <param name="databaseConfig">Configuration of the database to sync</param>
        /// <param name="updateStatus">Action to write status messages to to display in the UI</param>
        /// <returns>Path to the local KeePass database or NULL if the process has been aborted</returns>
        public static async Task<string> OpenFromOneDriveCloudProvider(Configuration databaseConfig, Action<string> updateStatus)
        {
            // Connect to OneDrive
            var oneDriveApi = await Utilities.GetOneDriveApi(databaseConfig);

            if (oneDriveApi == null)
            {
                switch (databaseConfig.CloudStorageType.Value)
                {
                    case CloudStorageType.OneDriveConsumer: updateStatus("Failed to connect to OneDrive"); break;
                    case CloudStorageType.OneDriveForBusiness: updateStatus("Failed to connect to OneDrive for Business"); break;
                    case CloudStorageType.MicrosoftGraph: updateStatus("Failed to connect to Microsoft Graph"); break;
                    default: updateStatus("Failed to connect to cloud service"); break;
                }
                return null;
            }

            // Save the RefreshToken in the configuration so we can use it again next time
            databaseConfig.RefreshToken = oneDriveApi.AccessToken.RefreshToken;

            // Fetch details about this OneDrive account
            var oneDriveAccount = await oneDriveApi.GetDrive();
            databaseConfig.OneDriveName = oneDriveAccount.Owner.User.DisplayName;

            // Ask the user to select the database to open on OneDrive
            var oneDriveFilePickerDialog = new Forms.OneDriveFilePickerDialog(oneDriveApi)
            {
                ExplanationText = "Select the KeePass database to open. Right click for additional options.",
                AllowEnteringNewFileName = false
            };
            await oneDriveFilePickerDialog.LoadFolderItems();
            var result = oneDriveFilePickerDialog.ShowDialog();
            if (result != DialogResult.OK || string.IsNullOrEmpty(oneDriveFilePickerDialog.FileName))
            {
                KeePassDatabase.UpdateStatus("Open KeePass database from OneDrive aborted");
                return null;
            }

            databaseConfig.RemoteDatabasePath = (oneDriveFilePickerDialog.CurrentOneDriveItem.ParentReference != null ? oneDriveFilePickerDialog.CurrentOneDriveItem.ParentReference.Path : "") + "/" + oneDriveFilePickerDialog.CurrentOneDriveItem.Name + "/" + oneDriveFilePickerDialog.FileName;
            databaseConfig.RemoteFolderId = oneDriveFilePickerDialog.CurrentOneDriveItem.Id;
            databaseConfig.RemoteFileName = oneDriveFilePickerDialog.FileName;

            // Retrieve the KeePass database from OneDrive
            var oneDriveItem = await oneDriveApi.GetItemInFolder(databaseConfig.RemoteFolderId, databaseConfig.RemoteFileName);

            if (oneDriveItem == null)
            {
                // KeePass database not found on OneDrive
                switch (databaseConfig.CloudStorageType.Value)
                {
                    case CloudStorageType.OneDriveConsumer: updateStatus("Unable to find the database on your OneDrive"); break;
                    case CloudStorageType.OneDriveForBusiness: updateStatus("Unable to find the database on your OneDrive for Business"); break;
                    case CloudStorageType.MicrosoftGraph: updateStatus("Unable to find the database on through Microsoft Graph"); break;
                    default: updateStatus("Failed to connect to cloud service"); break;
                }
                return null;
            }

            // Show the save as dialog to select a location locally where to store the KeePass database
            var saveFiledialog = new SaveFileDialog
            {
                Filter = "KeePass databases (*.kdbx)|*.kdbx|All Files (*.*)|*.*",
                Title = "Select where to store the KeePass database locally",
                CheckFileExists = false,
                FileName = oneDriveItem.Name
            };

            var saveFileDialogResult = saveFiledialog.ShowDialog();
            if (saveFileDialogResult != DialogResult.OK || string.IsNullOrEmpty(saveFiledialog.FileName))
            {
                updateStatus("Open KeePass database from OneDrive aborted");
                return null;
            }

            // Download the KeePass database to the selected location
            updateStatus("Downloading KeePass database");
            await oneDriveApi.DownloadItemAndSaveAs(oneDriveItem, saveFiledialog.FileName);

            return saveFiledialog.FileName;
        }
    }
}
