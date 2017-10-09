using KoenZomers.KeePass.OneDriveSync;
using Microsoft.SharePoint.Client;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KoenZomersKeePassOneDriveSync.Providers
{
    internal static class SharePointProvider
    {
        /// <summary>
        /// Uses the Microsoft SharePoint platform (SharePoint 2013, 2016 or Online) to sync the KeePass database
        /// </summary>
        /// <param name="databaseConfig">Configuration of the database to sync</param>
        /// <param name="localKeePassDatabasePath">Path to where the KeePass database to sync resides</param>
        /// <param name="forceSync">Flag to indicate if the sync should always take place</param>
        /// <param name="updateStatus">Action to write status messages to to display in the UI</param>
        /// <returns>True if successful, false if failed</returns>
        public static bool SyncUsingSharePointPlatform(Configuration databaseConfig, string localKeePassDatabasePath, bool forceSync, Action<string> updateStatus)
        {
            if (string.IsNullOrEmpty(databaseConfig.RefreshToken) || databaseConfig.RefreshToken.IndexOf(';') == -1 || string.IsNullOrEmpty(databaseConfig.RemoteDatabasePath))
            {
                // Configuration does not have a SharePoint config in it yet, ask for connection details
                bool retryGettingApiInstance;
                do
                {
                    retryGettingApiInstance = false;
                    try
                    {
                        var requestSharePointDetailsSuccessful = RequestSharePointDetails(databaseConfig);

                        if (!requestSharePointDetailsSuccessful)
                        {
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        // Build the error text to show to the end user
                        var errorMessage = new System.Text.StringBuilder();
                        errorMessage.Append("Failed to connect to SharePoint:");
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
                        retryGettingApiInstance = true;
                    }
                } while (retryGettingApiInstance);
            }

            using (var clientContext = CreateSharePointClientContext(databaseConfig))
            {
                if (clientContext == null)
                {
                    updateStatus("Failed to connect to SharePoint");
                    return false;
                }

                // Verify if we already have retrieved the name of this OneDrive
                if (string.IsNullOrEmpty(databaseConfig.OneDriveName))
                {
                    clientContext.Load(clientContext.Web, w => w.Title);
                    clientContext.ExecuteQuery();

                    databaseConfig.OneDriveName = clientContext.Web.Title;
                    Configuration.Save();
                }

                // Check if we have a Document Library on SharePoint to sync with
                if (string.IsNullOrEmpty(databaseConfig.RemoteFolderId) && string.IsNullOrEmpty(databaseConfig.RemoteFileName))
                {
                    // Ask the user where to store the database on SharePoint
                    var sharePointDocumentLibraryPickerDialog = new Forms.SharePointDocumentLibraryPickerDialog(clientContext);
                    sharePointDocumentLibraryPickerDialog.LoadDocumentLibraryItems();
                    var result = sharePointDocumentLibraryPickerDialog.ShowDialog();
                    if (result != DialogResult.OK || string.IsNullOrEmpty(sharePointDocumentLibraryPickerDialog.SelectedDocumentLibraryId))
                    {
                        return false;
                    }
                    databaseConfig.RemoteFolderId = sharePointDocumentLibraryPickerDialog.SelectedDocumentLibraryId;
                    Configuration.Save();
                }
            }

            // Retrieve the KeePass database from SharePoint
            //var oneDriveItem = string.IsNullOrEmpty(databaseConfig.RemoteFolderId) ? await oneDriveApi.GetItem(databaseConfig.RemoteDatabasePath) : await oneDriveApi.GetItemInFolder(databaseConfig.RemoteFolderId, databaseConfig.RemoteFileName);

            //if (oneDriveItem == null)
            //{
            //    // KeePass database not found on OneDrive
            //    updateStatus("Database does not exist yet on OneDrive, uploading it now");

            //    OneDriveItem oneDriveFolder;
            //    string fileName;
            //    if (string.IsNullOrEmpty(databaseConfig.RemoteFolderId))
            //    {
            //        oneDriveFolder = databaseConfig.RemoteDatabasePath.Contains("/") ? await oneDriveApi.GetFolderOrCreate(databaseConfig.RemoteDatabasePath.Remove(databaseConfig.RemoteDatabasePath.LastIndexOf("/", StringComparison.Ordinal))) : await oneDriveApi.GetDriveRoot();
            //        fileName = databaseConfig.RemoteDatabasePath.Contains("/") ? databaseConfig.RemoteDatabasePath.Remove(0, databaseConfig.RemoteDatabasePath.LastIndexOf("/", StringComparison.Ordinal) + 1) : databaseConfig.RemoteDatabasePath;
            //    }
            //    else
            //    {
            //        oneDriveFolder = await oneDriveApi.GetItemById(databaseConfig.RemoteFolderId);
            //        fileName = databaseConfig.RemoteFileName;
            //    }

            //    if (oneDriveFolder == null)
            //    {
            //        updateStatus("Unable to upload database to OneDrive. Remote path is invalid.");
            //        return false;
            //    }

            //    // Upload the database to OneDrive                
            //    var newUploadResult = await oneDriveApi.UploadFileAs(localKeePassDatabasePath, fileName, oneDriveFolder);

            //    updateStatus(newUploadResult == null ? "Failed to upload the KeePass database" : "Successfully uploaded the new KeePass database to OneDrive");

            //    databaseConfig.LocalFileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);
            //    if (newUploadResult != null)
            //    {
            //        databaseConfig.LastCheckedAt = DateTime.Now;
            //        databaseConfig.LastSyncedAt = DateTime.Now;
            //        databaseConfig.ETag = newUploadResult.ETag;
            //    }
            //    Configuration.Save();
            //    return false;
            //}

            //// Use the ETag from the OneDrive item to compare it against the local database config etag to see if the content has changed
            //if (!forceSync && oneDriveItem.ETag == databaseConfig.ETag)
            //{
            //    updateStatus("Databases are in sync");

            //    databaseConfig.LastCheckedAt = DateTime.Now;
            //    Configuration.Save();

            //    return false;
            //}

            //// Download the database from OneDrive
            //updateStatus("Downloading KeePass database from OneDrive");

            //var temporaryKeePassDatabasePath = Path.GetTempFileName();
            //var downloadSuccessful = await oneDriveApi.DownloadItemAndSaveAs(oneDriveItem, temporaryKeePassDatabasePath);

            //if (!downloadSuccessful)
            //{
            //    updateStatus("Failed to download the KeePass database from OneDrive");

            //    return false;
            //}

            //// Sync database
            //updateStatus("KeePass database downloaded, going to sync");

            //// Ensure the database that needs to be synced is still the database currently selected in KeePass to avoid merging the downloaded database with the wrong database in KeePass
            //if ((!KoenZomersKeePassOneDriveSyncExt.Host.Database.IOConnectionInfo.Path.StartsWith(Environment.CurrentDirectory) && KoenZomersKeePassOneDriveSyncExt.Host.Database.IOConnectionInfo.Path != localKeePassDatabasePath) ||
            //    (KoenZomersKeePassOneDriveSyncExt.Host.Database.IOConnectionInfo.Path.StartsWith(Environment.CurrentDirectory) && KoenZomersKeePassOneDriveSyncExt.Host.Database.IOConnectionInfo.Path.Remove(0, Environment.CurrentDirectory.Length + 1) != localKeePassDatabasePath))
            //{
            //    updateStatus("Failed to sync. Please don't switch to another database before done.");

            //    return false;
            //}

            //// Merge the downloaded database with the currently open KeePass database
            //var syncSuccessful = MergeDatabases(databaseConfig, temporaryKeePassDatabasePath);

            //if (!syncSuccessful)
            //{
            //    updateStatus("Failed to synchronize the KeePass databases");
            //    return false;
            //}

            //// Upload the synced database
            //updateStatus("Uploading the new KeePass database to OneDrive");

            //var uploadResult = await oneDriveApi.UploadFileAs(temporaryKeePassDatabasePath, oneDriveItem.Name, oneDriveItem.ParentReference.Path.Equals("/drive/root:", StringComparison.CurrentCultureIgnoreCase) ? await oneDriveApi.GetDriveRoot() : await oneDriveApi.GetItemById(oneDriveItem.ParentReference.Id));
            //if (uploadResult == null)
            //{
            //    updateStatus("Failed to upload the KeePass database");
            //    return false;
            //}

            //// Delete the temporary database used for merging
            //File.Delete(temporaryKeePassDatabasePath);

            //databaseConfig.ETag = uploadResult.ETag;
            return true;
        }

        /// <summary>
        /// Creates a SharePoint ClientContext based on a Configuration file specific for a SharePoint synchronization
        /// </summary>
        /// <param name="databaseConfig">Configuration set to be specific for a SharePoint synchronization</param>
        /// <returns>SharePoint ClientContext or NULL if unable to establish one based on the provided configuration</returns>
        public static ClientContext CreateSharePointClientContext(Configuration databaseConfig)
        {
            // Collect the SharePoint variables required to connect
            var sharePointUri = new Uri(databaseConfig.RemoteDatabasePath);
            var sharePointClientId = databaseConfig.RefreshToken.Remove(databaseConfig.RefreshToken.IndexOf(';'));
            var sharePointClientSecret = databaseConfig.RefreshToken.Remove(0, databaseConfig.RefreshToken.IndexOf(';') + 1);

            return CreateSharePointClientContext(sharePointUri, sharePointClientId, sharePointClientSecret);
        }

        /// <summary>
        /// Creates a SharePoint ClientContext based on the provided SharePoint Uri, ClientId and ClientSecret
        /// </summary>
        /// <param name="sharePointUri">Uri of the SharePoint site to connect to</param>
        /// <param name="sharePointClientId">ClientId to use for the Low Trust to connect to SharePoint</param>
        /// <param name="sharePointClientSecret">ClientSecret to use for the Low Trust to connect to SharePoint</param>
        /// <returns>SharePoint ClientContext or NULL if unable to establish one based on the provided configuration</returns>
        public static ClientContext CreateSharePointClientContext(Uri sharePointUri, string sharePointClientId, string sharePointClientSecret)
        {
            // Get the realm for the SharePoint site
            var realm = TokenHelper.GetRealmFromTargetUrl(sharePointUri);

            // Get the access token for the URL
            var accessToken = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, sharePointUri.Authority, realm, sharePointClientId, sharePointClientSecret).AccessToken;

            // Connect to SharePoint
            var clientContext = TokenHelper.GetClientContextWithAccessToken(sharePointUri.ToString(), accessToken);
            return clientContext;
        }

        /// <summary>
        /// Form to request the SharePoint details to connect to
        /// </summary>
        /// <param name="databaseConfig">Configuration which needs to map to a SharePoint environment</param>
        /// <returns>True if successful, false if failed to receive a SharePoint configuration</returns>
        private static bool RequestSharePointDetails(Configuration databaseConfig)
        {
            var sharePointCredentialsDialog = new Forms.SharePointCredentialsForm();
            var result = sharePointCredentialsDialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return false;
            }

            // Copy the entered data into the database configuration
            databaseConfig.RefreshToken = string.Format("{0};{1}", sharePointCredentialsDialog.SharePointClientId, sharePointCredentialsDialog.SharePointClientSecret);
            databaseConfig.RemoteDatabasePath = sharePointCredentialsDialog.SharePointUrl;
            Configuration.Save();

            return true;
        }

        /// <summary>
        /// Test the connection with the provided ClientContext
        /// </summary>
        /// <param name="clientContext">The ClientContext to use to test the connection</param>
        /// <returns>True if connection successful, False if the test failed</returns>
        public static bool TestConnection(ClientContext clientContext)
        {
            try
            {
                clientContext.Load(clientContext.Web, w => w.Title);
                clientContext.ExecuteQuery();

                return true;
            }
            catch (Exception)
            {
                return false;                
            }
        }
    }
}
