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
                    errorMessage.AppendLine(string.Format(" for database {0}", databaseConfig.KeePassDatabase.Name));
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
                    case CloudStorageType.OneDriveConsumer: updateStatus(string.Format("Failed to connect to OneDrive for database {0}", databaseConfig.KeePassDatabase.Name)); break;
                    case CloudStorageType.OneDriveForBusiness: updateStatus(string.Format("Failed to connect to OneDrive for Business for database {0}", databaseConfig.KeePassDatabase.Name)); break;
                    case CloudStorageType.MicrosoftGraph: updateStatus(string.Format("Failed to connect to Microsoft Graph for database {0}", databaseConfig.KeePassDatabase.Name)); break;
                    default: updateStatus(string.Format("Failed to connect to cloud service for database {0}", databaseConfig.KeePassDatabase.Name)); break;
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
                    AllowEnteringNewFileName = true,
                    FileName = new System.IO.FileInfo(localKeePassDatabasePath).Name
                };
                await oneDriveFilePickerDialog.LoadFolderItems();
                var result = oneDriveFilePickerDialog.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrEmpty(oneDriveFilePickerDialog.FileName))
                {
                    return false;
                }

                databaseConfig.RemoteDatabasePath = (oneDriveFilePickerDialog.CurrentOneDriveItem.ParentReference != null ? oneDriveFilePickerDialog.CurrentOneDriveItem.ParentReference.Path : "") + "/" + oneDriveFilePickerDialog.CurrentOneDriveItem.Name + "/" + oneDriveFilePickerDialog.FileName;
                databaseConfig.RemoteDriveId = oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem != null ? oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem.ParentReference.DriveId : oneDriveFilePickerDialog.CurrentOneDriveItem.ParentReference.DriveId != null ? oneDriveFilePickerDialog.CurrentOneDriveItem.ParentReference.DriveId : null;
                if (oneDriveFilePickerDialog.CurrentOneDriveItem.File != null || (oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem != null && oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem.File != null))
                {
                    databaseConfig.RemoteItemId = oneDriveFilePickerDialog.CurrentOneDriveItem.File != null ? oneDriveFilePickerDialog.CurrentOneDriveItem.Id : oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem.Id;
                }
                else
                {
                    databaseConfig.RemoteFolderId = oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem != null ? oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem.ParentReference != null ? string.IsNullOrEmpty(oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem.ParentReference.Id) ? oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem.Id : oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem.ParentReference.Id : oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem.Id : oneDriveFilePickerDialog.CurrentOneDriveItem.Id;
                }
                databaseConfig.RemoteFileName = oneDriveFilePickerDialog.FileName;
                Configuration.Save();
            }

            // Retrieve the metadata of the KeePass database on OneDrive
            OneDriveItem oneDriveItem;
            if (string.IsNullOrEmpty(databaseConfig.RemoteItemId))
            {
                // We don't have the ID of the KeePass file, check if the database is stored on the current user its drive or on a shared drive
                OneDriveItem folder;
                if (string.IsNullOrEmpty(databaseConfig.RemoteDriveId))
                {
                    // KeePass database is on the current user its drive
                    if (string.IsNullOrEmpty(databaseConfig.RemoteFolderId))
                    {
                        // No direct reference to the folder on the drive available, try locating it by its remote path
                        oneDriveItem = await oneDriveApi.GetItem(databaseConfig.RemoteDatabasePath);
                    }
                    else
                    {
                        // Get the folder in which the KeePass file resides
                        folder = await oneDriveApi.GetItemById(databaseConfig.RemoteFolderId);

                        if (folder == null)
                        {
                            updateStatus(string.Format("Unable to download database {0} from OneDrive. Remote path cannot be found.", databaseConfig.KeePassDatabase.Name));
                            return false;
                        }

                        // Locate the KeePass file in the folder
                        oneDriveItem = await oneDriveApi.GetItemInFolder(folder, databaseConfig.RemoteFileName);
                    }
                }
                else
                {
                    // KeePass database is on a shared drive or has not been uploaded yet
                    folder = await oneDriveApi.GetItemFromDriveById(databaseConfig.RemoteFolderId, databaseConfig.RemoteDriveId);

                    // Locate the KeePass file in the folder. Will return NULL if the file has not been uploaded yet.
                    oneDriveItem = await oneDriveApi.GetItemInFolder(folder, databaseConfig.RemoteFileName);
                }

                // Check if the KeePass file has been found
                if (oneDriveItem != null)
                {
                    // Store the direct Id to the KeePass file so we can save several API calls on future syncs
                    databaseConfig.RemoteItemId = oneDriveItem.Id;
                    Configuration.Save();
                }
            }
            else
            {
                // We have the ID of the KeePass file, check if it's on the current user its drive or on a shared drive
                if (string.IsNullOrEmpty(databaseConfig.RemoteDriveId))
                {
                    // KeePass database is on the current user its drive
                    oneDriveItem = await oneDriveApi.GetItemById(databaseConfig.RemoteItemId);
                }
                else
                {
                    // KeePass database is on a shared drive
                    oneDriveItem = await oneDriveApi.GetItemFromDriveById(databaseConfig.RemoteItemId, databaseConfig.RemoteDriveId);
                }
            }
            
            if (oneDriveItem == null)
            {
                // KeePass database not found on OneDrive
                updateStatus(string.Format("Database {0} does not exist yet on OneDrive, uploading it now", databaseConfig.KeePassDatabase.Name));

                OneDriveItem oneDriveFolder;
                string fileName;
                if (string.IsNullOrEmpty(databaseConfig.RemoteFolderId))
                {
                    oneDriveFolder = databaseConfig.RemoteDatabasePath.Contains("/") ? await oneDriveApi.GetFolderOrCreate(databaseConfig.RemoteDatabasePath.Remove(databaseConfig.RemoteDatabasePath.LastIndexOf("/", StringComparison.Ordinal))) : await oneDriveApi.GetDriveRoot();
                    fileName = databaseConfig.RemoteDatabasePath.Contains("/") ? databaseConfig.RemoteDatabasePath.Remove(0, databaseConfig.RemoteDatabasePath.LastIndexOf("/", StringComparison.Ordinal) + 1) : databaseConfig.RemoteDatabasePath;
                }
                else
                {
                    oneDriveFolder = databaseConfig.RemoteDriveId == null ? await oneDriveApi.GetItemById(databaseConfig.RemoteFolderId) : await oneDriveApi.GetItemFromDriveById(databaseConfig.RemoteFolderId, databaseConfig.RemoteDriveId);
                    fileName = databaseConfig.RemoteFileName;
                }

                if (oneDriveFolder == null)
                {
                    updateStatus(string.Format("Unable to upload database {0} to OneDrive. Remote path is invalid.", databaseConfig.KeePassDatabase.Name));
                    return false;
                }

                // Upload the database to OneDrive                
                var newUploadResult = await oneDriveApi.UploadFileAs(localKeePassDatabasePath, fileName, oneDriveFolder);

                updateStatus(string.Format(newUploadResult == null ? "Failed to upload the KeePass database {0}" : "Successfully uploaded the new KeePass database {0} to OneDrive", databaseConfig.KeePassDatabase.Name));

                databaseConfig.LocalFileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);
                if (newUploadResult != null)
                {
                    databaseConfig.RemoteItemId = newUploadResult.Id;
                    databaseConfig.LastCheckedAt = DateTime.Now;
                    databaseConfig.LastSyncedAt = DateTime.Now;
                    databaseConfig.ETag = newUploadResult.ETag;
                }
                Configuration.Save();
                return false;
            }

            // Use the ETag from the OneDrive item to compare it against the local database config etag to see if the content has changed
            // Microsoft Graph API reports back a different ETag when uploading than the file actually gets assigned for some unknown reason. This would cause each sync attempt to sync again as the ETags differ. As a workaround we'll use the CTag which does seem reliable to detect a change to the file.
            if (!forceSync && 
                oneDriveItem.CTag == databaseConfig.ETag)
            {
                updateStatus(string.Format("Database {0} is in sync", databaseConfig.KeePassDatabase.Name));

                databaseConfig.LastCheckedAt = DateTime.Now;
                Configuration.Save();

                return false;
            }

            // Download the database from OneDrive
            updateStatus(string.Format("Downloading KeePass database {0} from OneDrive", databaseConfig.KeePassDatabase.Name));

            var temporaryKeePassDatabasePath = System.IO.Path.GetTempFileName();
            var downloadSuccessful = await oneDriveApi.DownloadItemAndSaveAs(oneDriveItem, temporaryKeePassDatabasePath);

            if (!downloadSuccessful)
            {
                updateStatus(string.Format("Failed to download the KeePass database {0} from OneDrive", databaseConfig.KeePassDatabase.Name));

                return false;
            }

            // Sync database
            updateStatus(string.Format("KeePass database {0} downloaded, going to sync", databaseConfig.KeePassDatabase.Name));

            // Merge the downloaded database with the currently open KeePass database
            var syncSuccessful = KeePassDatabase.MergeDatabases(databaseConfig, temporaryKeePassDatabasePath);

            string localDatabaseToUpload;
            if (!syncSuccessful)
            {
                updateStatus(string.Format("Failed to synchronize the KeePass database {0}", databaseConfig.KeePassDatabase.Name));

                var confirm = MessageBox.Show("Unable to merge the databases. Did you just change the master password for this KeePass database? If so and you would like to OVERWRITE the KeePass database stored on your OneDrive with your local database, select Yes, otherwise select No.", "Confirm overwriting your KeePass database", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (confirm != DialogResult.Yes) return false;

                // Upload the local database
                localDatabaseToUpload = databaseConfig.KeePassDatabase.IOConnectionInfo.Path;

                updateStatus(string.Format("Uploading the local KeePass database {0} to OneDrive", databaseConfig.KeePassDatabase.Name));
            }
            else
            {
                // Upload the synced database
                localDatabaseToUpload = temporaryKeePassDatabasePath;

                updateStatus(string.Format("Uploading the merged KeePass database {0} to OneDrive", databaseConfig.KeePassDatabase.Name));
            }

            OneDriveItem uploadResult = null;

            // Due to some issues with the OneDrive API throwing random errors for no reason, retry a few times to upload the database before it will be considered failed
            for (var uploadAttempts = 1; uploadAttempts <= 5; uploadAttempts++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(databaseConfig.RemoteItemId))
                    {
                        // Database is already present on OneDrive, update it
                        uploadResult = await oneDriveApi.UpdateFile(localDatabaseToUpload, oneDriveItem);
                    }
                    else
                    {
                        // Database resides on the user its own OneDrive in the root folder
                        if (oneDriveItem.ParentReference.Path.Equals("/drive/root:", StringComparison.CurrentCultureIgnoreCase))
                        {
                            uploadResult = await oneDriveApi.UploadFileAs(localDatabaseToUpload, oneDriveItem.Name, await oneDriveApi.GetDriveRoot());
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(databaseConfig.RemoteDriveId))
                            {
                                // Database resides on the user its own OneDrive in a folder
                                uploadResult = await oneDriveApi.UploadFileAs(localDatabaseToUpload, oneDriveItem.Name, await oneDriveApi.GetItemById(oneDriveItem.ParentReference.Id));
                            }
                            else
                            {
                                // Database resides on another OneDrive
                                uploadResult = await oneDriveApi.UploadFileAs(localDatabaseToUpload, oneDriveItem.Name, await oneDriveApi.GetItemFromDriveById(oneDriveItem.ParentReference.Id, oneDriveItem.ParentReference.DriveId));
                            }
                        }
                    }

                    // Uploading succeeded
                    break;
                }
                catch (ArgumentNullException e)
                {
                    // If any other exception than the one we will get if the OneDrive API throws the random error, then ensure we pass on the exception instead of swallowing it
                    if (e.ParamName != "oneDriveUploadSession")
                    {
                        throw;
                    }

                    updateStatus(string.Format("Uploading the new KeePass database {1} to OneDrive (attempt {0})", uploadAttempts + 1, databaseConfig.KeePassDatabase.Name));
                }
            }

            if (uploadResult == null)
            {
                updateStatus(string.Format("Failed to upload the KeePass database {0}", databaseConfig.KeePassDatabase.Name));
                return false;
            }

            // Delete the temporary database used for merging
            System.IO.File.Delete(temporaryKeePassDatabasePath);

           // The ETag changes with every request of the item so we use the CTag instead which only changes when the file changes
            databaseConfig.ETag = uploadResult.CTag;

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
            databaseConfig.RemoteDriveId = oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem != null ? oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem.ParentReference.DriveId : oneDriveFilePickerDialog.CurrentOneDriveItem.ParentReference.DriveId != null ? oneDriveFilePickerDialog.CurrentOneDriveItem.ParentReference.DriveId : null;
            if (oneDriveFilePickerDialog.CurrentOneDriveItem.File != null || (oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem != null && oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem.File != null))
            {
                databaseConfig.RemoteItemId = oneDriveFilePickerDialog.CurrentOneDriveItem.File != null ? oneDriveFilePickerDialog.CurrentOneDriveItem.Id : oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem.Id;
            }
            else
            {
                databaseConfig.RemoteFolderId = oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem != null ? oneDriveFilePickerDialog.CurrentOneDriveItem.RemoteItem.Id : oneDriveFilePickerDialog.CurrentOneDriveItem.Id;
            }
            databaseConfig.RemoteFileName = oneDriveFilePickerDialog.FileName;

            // Retrieve the metadata of the KeePass database on OneDrive
            OneDriveItem oneDriveItem;
            if (string.IsNullOrEmpty(databaseConfig.RemoteItemId))
            {
                // We don't have the ID of the KeePass file, check if the database is stored on the current user its drive or on a shared drive
                OneDriveItem folder;
                if (string.IsNullOrEmpty(databaseConfig.RemoteDriveId))
                {
                    // KeePass database is on the current user its drive
                    folder = await oneDriveApi.GetItemById(databaseConfig.RemoteFolderId);
                }
                else
                {
                    // KeePass database is on a shared drive
                    folder = await oneDriveApi.GetItemFromDriveById(databaseConfig.RemoteFolderId, databaseConfig.RemoteDriveId);
                }

                // Locate the KeePass file in the folder
                oneDriveItem = await oneDriveApi.GetItemInFolder(folder, databaseConfig.RemoteFileName);

                // Check if the KeePass file has been found
                if (oneDriveItem != null)
                {
                    // Store the direct Id to the KeePass file so we can save several API calls on future syncs
                    databaseConfig.RemoteItemId = oneDriveItem.Id;
                    Configuration.Save();
                }
            }
            else
            {
                // We have the ID of the KeePass file, check if it's on the current user its drive or on a shared drive
                if (string.IsNullOrEmpty(databaseConfig.RemoteDriveId))
                {
                    // KeePass database is on the current user its drive
                    oneDriveItem = await oneDriveApi.GetItemById(databaseConfig.RemoteItemId);
                }
                else
                {
                    // KeePass database is on a shared drive
                    oneDriveItem = await oneDriveApi.GetItemFromDriveById(databaseConfig.RemoteItemId, databaseConfig.RemoteDriveId);
                }
            }

            if (oneDriveItem == null)
            {
                // KeePass database not found on OneDrive
                switch (databaseConfig.CloudStorageType.Value)
                {
                    case CloudStorageType.OneDriveConsumer: updateStatus(string.Format("Unable to find the database {0} on your OneDrive", databaseConfig.KeePassDatabase.Name)); break;
                    case CloudStorageType.OneDriveForBusiness: updateStatus(string.Format("Unable to find the database {0} on your OneDrive for Business", databaseConfig.KeePassDatabase.Name)); break;
                    case CloudStorageType.MicrosoftGraph: updateStatus(string.Format("Unable to find the database {0} through Microsoft Graph", databaseConfig.KeePassDatabase.Name)); break;
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
                updateStatus(string.Format("Open KeePass database {0} from OneDrive aborted", databaseConfig.KeePassDatabase.Name));
                return null;
            }

            // Download the KeePass database to the selected location
            updateStatus(string.Format("Downloading KeePass database {0}", databaseConfig.KeePassDatabase.Name));
            await oneDriveApi.DownloadItemAndSaveAs(oneDriveItem, saveFiledialog.FileName);

            // The ETag changes with every request of the item so we use the CTag instead which only changes when the file changes
            databaseConfig.ETag = oneDriveItem.CTag;

            return saveFiledialog.FileName;
        }
    }
}
