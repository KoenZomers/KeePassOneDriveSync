using KoenZomers.KeePass.OneDriveSync;
using Microsoft.SharePoint.Client;
using System;
using System.IO;
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
                    sharePointDocumentLibraryPickerDialog.FileName = !string.IsNullOrEmpty(databaseConfig.RemoteFileName) ? databaseConfig.RemoteFileName : new FileInfo(localKeePassDatabasePath).Name;
                    var result = sharePointDocumentLibraryPickerDialog.ShowDialog();
                    if (result != DialogResult.OK || string.IsNullOrEmpty(sharePointDocumentLibraryPickerDialog.SelectedDocumentLibraryServerRelativeUrl))
                    {
                        return false;
                    }
                    databaseConfig.RemoteFolderId = sharePointDocumentLibraryPickerDialog.SelectedDocumentLibraryServerRelativeUrl;
                    databaseConfig.RemoteFileName = sharePointDocumentLibraryPickerDialog.FileName;
                    Configuration.Save();
                }

                // Retrieve the KeePass database from SharePoint
                var serverRelativeSharePointUrl = string.Concat(databaseConfig.RemoteFolderId, "/", databaseConfig.RemoteFileName);
                var sharePointItem = TryGetFileByServerRelativeUrl(clientContext.Web, serverRelativeSharePointUrl);

                if(sharePointItem == null)
                {
                    // KeePass database not found on OneDrive
                    updateStatus("Database does not exist yet on SharePoint, uploading it now");

                    // Upload the database to SharePoint
                    sharePointItem = UploadFile(databaseConfig.KeePassDatabase.IOConnectionInfo.Path, databaseConfig.RemoteFolderId, databaseConfig.RemoteFileName, clientContext);

                    updateStatus(sharePointItem == null ? "Failed to upload the KeePass database" : "Successfully uploaded the new KeePass database to SharePoint");

                    databaseConfig.LocalFileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);
                    if (sharePointItem != null)
                    {
                        databaseConfig.LastCheckedAt = DateTime.Now;
                        databaseConfig.LastSyncedAt = DateTime.Now;
                        databaseConfig.ETag = sharePointItem.ETag;
                    }
                    Configuration.Save();
                    return false;
                }

                // Use the ETag from the SharePoint item to compare it against the local database config etag to see if the content has changed
                if (!forceSync && sharePointItem.ETag == databaseConfig.ETag)
                {
                    updateStatus("Databases are in sync");

                    databaseConfig.LastCheckedAt = DateTime.Now;
                    Configuration.Save();

                    return false;
                }

                // Download the database from SharePoint
                updateStatus("Downloading KeePass database from SharePoint");

                var temporaryKeePassDatabasePath = Path.GetTempFileName();
                var downloadSuccessful = DownloadFile(temporaryKeePassDatabasePath, serverRelativeSharePointUrl, clientContext);

                if (!downloadSuccessful)
                {
                    updateStatus("Failed to download the KeePass database from SharePoint");

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
                updateStatus("Uploading the new KeePass database to SharePoint");

                var uploadResult = UploadFile(temporaryKeePassDatabasePath, databaseConfig.RemoteFolderId, databaseConfig.RemoteFileName, clientContext);
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
        }

        /// <summary>
        /// Tries to retrieve the file at the provided server relative URL from SharePoint
        /// </summary>
        /// <param name="web">Web in which the file resides</param>
        /// <param name="serverRelativeUrl">Server relative URL of the file to try to retrieve</param>
        /// <returns>File instance of the file or NULL if unable to find the file</returns>
        public static Microsoft.SharePoint.Client.File TryGetFileByServerRelativeUrl(Web web, string serverRelativeUrl)
        {
            var ctx = web.Context;
            try
            {
                var file = web.GetFileByServerRelativeUrl(serverRelativeUrl);
                ctx.Load(file, f => f.ETag);
                ctx.ExecuteQuery();
                return file;
            }
            catch (ServerException ex)
            {
                if (ex.ServerErrorTypeName == "System.IO.FileNotFoundException")
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Uploads a file to SharePoint
        /// </summary>
        /// <param name="localDatabasePath">Full path to where the file to upload resides locally</param>
        /// <param name="serverRelativeUrl">Server relative URL where the file should be uploaded. Should not include the filename.</param>
        /// <param name="fileName">Filename under which to store the file in SharePoint</param>
        /// <param name="context">SharePoint Context to use for the SharePoint communication</param>
        /// <returns>File instance representing the uploaded file if successful, NULL if it failed</returns>
        public static Microsoft.SharePoint.Client.File UploadFile(string localDatabasePath, string serverRelativeUrl, string fileName, ClientContext context)
        {
            try
            {
                var list = context.Web.GetList(serverRelativeUrl);
                var fileCreationInformation = new FileCreationInformation
                {
                    Content = System.IO.File.ReadAllBytes(localDatabasePath),
                    Url = fileName,
                    Overwrite = true
                };
                var file = list.RootFolder.Files.Add(fileCreationInformation);
                context.Load(file, f => f.ETag);
                context.ExecuteQuery();

                return file;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Downloads a file from SharePoint
        /// </summary>
        /// <param name="localDatabasePath">Full path to where to download the file to</param>
        /// <param name="serverRelativeUrl">Server relative URL where the file should be downloaded from. Should include the filename.</param>
        /// <param name="context">SharePoint Context to use for the SharePoint communication</param>
        /// <returns>File instance representing the uploaded file if successful, NULL if it failed</returns>
        public static bool DownloadFile(string localDatabasePath, string serverRelativeUrl, ClientContext context)
        {
            try
            {
                var file = context.Web.GetFileByServerRelativeUrl(serverRelativeUrl);
                var streamResult = file.OpenBinaryStream();
                context.ExecuteQuery();
                
                using (var fileStream = System.IO.File.Create(localDatabasePath))
                {
                    streamResult.Value.CopyTo(fileStream);
                }
                
                return true;
            }
            catch(Exception)
            {
                return false;
            }
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
