using KoenZomers.KeePass.OneDriveSync;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
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
        public static async Task<bool> SyncUsingSharePointPlatform(Configuration databaseConfig, string localKeePassDatabasePath, bool forceSync, Action<string> updateStatus)
        {
            if(! await EnsureSharePointCredentials(databaseConfig))
            {
                return false;
            }

            using (var httpClient = CreateSharePointHttpClient(databaseConfig))
            {
                // Check if we have a Document Library on SharePoint to sync with
                if (string.IsNullOrEmpty(databaseConfig.RemoteFolderId) && string.IsNullOrEmpty(databaseConfig.RemoteFileName))
                {
                    // Ask the user where to store the database on SharePoint
                    var sharePointDocumentLibraryPickerDialog = new Forms.SharePointDocumentLibraryPickerDialog(httpClient);
                    await sharePointDocumentLibraryPickerDialog.LoadDocumentLibraryItems();
                    sharePointDocumentLibraryPickerDialog.FileName = !string.IsNullOrEmpty(databaseConfig.RemoteFileName) ? databaseConfig.RemoteFileName : new System.IO.FileInfo(localKeePassDatabasePath).Name;
                    var result = sharePointDocumentLibraryPickerDialog.ShowDialog();
                    if (result != DialogResult.OK || string.IsNullOrEmpty(sharePointDocumentLibraryPickerDialog.SelectedDocumentLibraryServerRelativeUrl))
                    {
                        return false;
                    }
                    databaseConfig.RemoteFolderId = sharePointDocumentLibraryPickerDialog.SelectedDocumentLibraryServerRelativeUrl;
                    databaseConfig.RemoteFileName = sharePointDocumentLibraryPickerDialog.FileName;
                    Configuration.Save();
                }

                // Ensure we have the SharePoint site name
                if(string.IsNullOrEmpty(databaseConfig.OneDriveName))
                {
                    // We don't have the SharePoint site name yet, retrieve it now by triggering TestConnection
                    await TestConnection(httpClient, databaseConfig);
                    Configuration.Save();
                }

                // Retrieve the KeePass database from SharePoint
                var serverRelativeSharePointUrl = string.Concat(databaseConfig.RemoteFolderId, "/", databaseConfig.RemoteFileName);
                var eTag = await GetEtagOfFile(httpClient, serverRelativeSharePointUrl);

                if (eTag == null)
                {
                    // KeePass database not found on OneDrive
                    updateStatus("Database does not exist yet on SharePoint, uploading it now");

                    // Upload the database to SharePoint
                    eTag = await UploadFile(databaseConfig.KeePassDatabase.IOConnectionInfo.Path, databaseConfig.RemoteFolderId, databaseConfig.RemoteFileName, httpClient);

                    updateStatus(eTag == null ? "Failed to upload the KeePass database" : "Successfully uploaded the new KeePass database to SharePoint");

                    databaseConfig.LocalFileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);
                    if (eTag != null)
                    {
                        databaseConfig.LastCheckedAt = DateTime.Now;
                        databaseConfig.LastSyncedAt = DateTime.Now;
                        databaseConfig.ETag = eTag;
                    }
                    Configuration.Save();
                    return false;
                }

                // Use the ETag from the SharePoint item to compare it against the local database config etag to see if the content has changed
                if (!forceSync && eTag == databaseConfig.ETag)
                {
                    updateStatus("Databases are in sync");

                    databaseConfig.LastCheckedAt = DateTime.Now;
                    Configuration.Save();

                    return false;
                }

                // Download the database from SharePoint
                updateStatus("Downloading KeePass database from SharePoint");

                var temporaryKeePassDatabasePath = System.IO.Path.GetTempFileName();
                var downloadSuccessful = DownloadFile(temporaryKeePassDatabasePath, serverRelativeSharePointUrl, httpClient);

                if (! await downloadSuccessful)
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

                var uploadResult = await UploadFile(temporaryKeePassDatabasePath, databaseConfig.RemoteFolderId, databaseConfig.RemoteFileName, httpClient);
                if (uploadResult == null)
                {
                    updateStatus("Failed to upload the KeePass database");
                    return false;
                }

                // Delete the temporary database used for merging
                System.IO.File.Delete(temporaryKeePassDatabasePath);

                databaseConfig.ETag = uploadResult;
                return true;
            }
        }

        /// <summary>
        /// Tries to retrieve the ETag of the file at the provided server relative URL from SharePoint
        /// </summary>
        /// <param name="httpClient">HttpClientt to use for the SharePoint communication</param>
        /// <param name="serverRelativeUrl">Server relative URL of the file to query for</param>
        /// <returns>ETag of the file or NULL if unable to find the file</returns>
        public static async Task<string> GetEtagOfFile(HttpClient httpClient, string serverRelativeUrl)
        {
            // Retrieve the ETag of the file
            using (var response = await httpClient.GetAsync("web/GetFileByServerRelativeUrl('" + serverRelativeUrl + "')?$select=ETag"))
            {
                // Check if the attempt was successful
                if(!response.IsSuccessStatusCode)
                {
                    // Attempt failed
                    return null;
                }

                // Attempt was successful, parse the JSON response
                var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());

                // Validate if ETag node exists in the result
                if (responseJson.TryGetValue("ETag", out JToken value))
                {
                    // ETag node exists, return it
                    return value.Value<string>();
                }
            }

            return null;
        }

        /// <summary>
        /// Uploads a file to SharePoint
        /// </summary>
        /// <param name="localDatabasePath">Full path to where the file to upload resides locally</param>
        /// <param name="serverRelativeUrl">Server relative URL where the file should be uploaded. Should not include the filename.</param>
        /// <param name="fileName">Filename under which to store the file in SharePoint</param>
        /// <param name="httpClient">HttpClientt to use for the SharePoint communication</param>
        /// <returns>ETag of the uploaded file if successful, NULL if it failed</returns>
        public static async Task<string> UploadFile(string localDatabasePath, string serverRelativeUrl, string fileName, HttpClient httpClient)
        {
            try
            {
                // Get a FormDigest to send to SharePoint
                var formDigest = await GetFormDigest(httpClient);

                // Validate that the FormDigest was retrieved successfully
                if(string.IsNullOrEmpty(formDigest))
                {
                    // No FormDigest available
                    return null;
                }

                // Construct a new HTTP message
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "web/GetFolderByServerRelativeUrl('" + serverRelativeUrl + "')/Files/Add(url='" + fileName + "',overwrite=true)?$select=ETag"))
                {
                    // Add the FormDiges to the request header
                    httpRequest.Headers.Add("X-RequestDigest", formDigest);                    

                    // Open the local file to upload
                    using (var fileContent = new StreamContent(System.IO.File.OpenRead(localDatabasePath)))
                    {
                        // Set the BODY content to the file byes
                        httpRequest.Content = fileContent;

                        // Send the bytes of the local file to the upload location on SharePoint
                        var response = await httpClient.SendAsync(httpRequest);

                        // Verify if the file was uploaded successfully
                        if (!response.IsSuccessStatusCode)
                        {
                            // Upload failed
                            return null;
                        }

                        // Upload was successful. Parse the result of the request.
                        var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());

                        // Validate if a ETag node exists in the result
                        if (responseJson.TryGetValue("ETag", out JToken value))
                        {
                            // ETag node exists, return it
                            return value.Value<string>();
                        }
                    }
                }

                return null;
            }
            catch(Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Downloads a file from SharePoint
        /// </summary>
        /// <param name="localDatabasePath">Full path to where to download the file to</param>
        /// <param name="serverRelativeUrl">Server relative URL where the file should be downloaded from. Should include the filename.</param>
        /// <param name="httpClient">HttpClient to use for the SharePoint communication</param>
        /// <returns>File instance representing the uploaded file if successful, NULL if it failed</returns>
        public static async Task<bool> DownloadFile(string localDatabasePath, string serverRelativeUrl, HttpClient httpClient)
        {
            try
            {
                // Request the file contents
                using (var response = await httpClient.GetStreamAsync("web/GetFileByServerRelativeUrl('" + serverRelativeUrl + "')/$value"))
                {
                    // Open the local file
                    using (var fileStream = System.IO.File.Create(localDatabasePath))
                    {
                        // Copy the downloaded bytes to the local file
                        await response.CopyToAsync(fileStream);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a file at the provided location in SharePoint
        /// </summary>
        /// <param name="serverRelativeFilePath">Server relative URL to the file to delete</param>
        /// <param name="httpClient">HttpClient to use for the SharePoint communication</param>
        /// <returns>True if successful, false if failed</returns>
        public static async Task<bool> DeleteFile(string serverRelativeFilePath, HttpClient httpClient)
        {
            try
            {
                // Get a FormDigest to send to SharePoint
                var formDigest = await GetFormDigest(httpClient);

                // Validate that the FormDigest was retrieved successfully
                if (string.IsNullOrEmpty(formDigest))
                {
                    // No FormDigest available
                    return false;
                }

                // Construct a new HTTP message
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "web/GetFileByServerRelativeUrl('" + serverRelativeFilePath + "')"))
                {
                    // Add the FormDiges to the request header
                    httpRequest.Headers.Add("X-RequestDigest", formDigest);

                    // Disable concurrency control
                    httpRequest.Headers.Add("If-Match", "*");

                    // Instruct to perform a DELETE operation   
                    httpRequest.Headers.Add("X-HTTP-Method", "DELETE");

                    // Send the request to SharePoint
                    var response = await httpClient.SendAsync(httpRequest);

                    // Verify if the request was processed successfully
                    if (!response.IsSuccessStatusCode)
                    {
                        // Request failed
                        return false;
                    }

                    // Request was successful
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a folder at the provided location in SharePoint
        /// </summary>
        /// <param name="serverRelativeFolderPath">Server relative URL to the folder to delete</param>
        /// <param name="httpClient">HttpClient to use for the SharePoint communication</param>
        /// <returns>True if successful, false if failed</returns>
        public static async Task<bool> DeleteFolder(string serverRelativeFolderPath, HttpClient httpClient)
        {
            try
            {
                // Get a FormDigest to send to SharePoint
                var formDigest = await GetFormDigest(httpClient);

                // Validate that the FormDigest was retrieved successfully
                if (string.IsNullOrEmpty(formDigest))
                {
                    // No FormDigest available
                    return false;
                }

                // Construct a new HTTP message
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "web/GetFolderByServerRelativeUrl('" + serverRelativeFolderPath + "')"))
                {
                    // Add the FormDiges to the request header
                    httpRequest.Headers.Add("X-RequestDigest", formDigest);

                    // Disable concurrency control
                    httpRequest.Headers.Add("If-Match", "*");

                    // Instruct to perform a DELETE operation   
                    httpRequest.Headers.Add("X-HTTP-Method", "DELETE");

                    // Send the request to SharePoint
                    var response = await httpClient.SendAsync(httpRequest);

                    // Verify if the request was processed successfully
                    if (!response.IsSuccessStatusCode)
                    {
                        // Request failed
                        return false;
                    }

                    // Request was successful
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Renames a folder at the provided location in SharePoint
        /// </summary>
        /// <param name="newFolderName">The new name to assign to the folder</param>
        /// <param name="serverRelativeFolderPath">Server relative URL to the folder to rename</param>
        /// <param name="httpClient">HttpClient to use for the SharePoint communication</param>
        /// <returns>True if successful, false if failed</returns>
        public static async Task<bool> RenameFolder(string newFolderName, string serverRelativeFolderPath, HttpClient httpClient)
        {
            try
            {
                // Get a FormDigest to send to SharePoint
                var formDigest = await GetFormDigest(httpClient);

                // Validate that the FormDigest was retrieved successfully
                if (string.IsNullOrEmpty(formDigest))
                {
                    // No FormDigest available
                    return false;
                }
               
                // Define the server relative url of the parent folder in which the folder resides
                var parentPath = serverRelativeFolderPath.Remove(serverRelativeFolderPath.LastIndexOf('/'));

                // Construct a new HTTP message
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "web/GetFolderByServerRelativeUrl('" + serverRelativeFolderPath + "')/moveto(newurl='" + parentPath + "/" + newFolderName + "')"))
                {
                    // Add the FormDiges to the request header
                    httpRequest.Headers.Add("X-RequestDigest", formDigest);

                    // Disable concurrency control
                    httpRequest.Headers.Add("If-Match", "*");

                    // Instruct to perform a MERGE operation   
                    httpRequest.Headers.Add("X-HTTP-Method", "MERGE");

                    // Provide the POST body content
                    //httpRequest.Content = new StringContent("{ '__metadata': { 'type': 'SP.Folder' }, 'Name': '" + newFolderName + "' }", System.Text.Encoding.UTF8, "application/json");                    

                    // Send the request to SharePoint
                    var response = await httpClient.SendAsync(httpRequest);

                    // Verify if the request was processed successfully
                    if (!response.IsSuccessStatusCode)
                    {
                        // Request failed
                        return false;
                    }

                    // Request was successful
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Renames a file at the provided location in SharePoint
        /// </summary>
        /// <param name="newFileName">The new name to assign to the file</param>
        /// <param name="serverRelativeFilePath">Server relative URL to the file to rename</param>
        /// <param name="httpClient">HttpClient to use for the SharePoint communication</param>
        /// <returns>True if successful, false if failed</returns>
        public static async Task<bool> RenameFile(string newFileName, string serverRelativeFilePath, HttpClient httpClient)
        {
            try
            {
                // Get a FormDigest to send to SharePoint
                var formDigest = await GetFormDigest(httpClient);

                // Validate that the FormDigest was retrieved successfully
                if (string.IsNullOrEmpty(formDigest))
                {
                    // No FormDigest available
                    return false;
                }

                // Define the server relative url of the folder in which the file resides
                var parentPath = serverRelativeFilePath.Remove(serverRelativeFilePath.LastIndexOf('/'));

                // Construct a new HTTP message
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "web/GetFileByServerRelativeUrl('" + serverRelativeFilePath + "')/moveto(newurl='" + parentPath + "/" + newFileName + "',flags=0)"))
                {
                    // Add the FormDiges to the request header
                    httpRequest.Headers.Add("X-RequestDigest", formDigest);

                    // Disable concurrency control
                    httpRequest.Headers.Add("If-Match", "*");

                    // Instruct to perform a MERGE operation   
                    httpRequest.Headers.Add("X-HTTP-Method", "MERGE");

                    // Send the request to SharePoint
                    var response = await httpClient.SendAsync(httpRequest);

                    // Verify if the request was processed successfully
                    if (!response.IsSuccessStatusCode)
                    {
                        // Request failed
                        return false;
                    }

                    // Request was successful
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a new folder at the provided location in SharePoint
        /// </summary>
        /// <param name="folderName">Name of the new folder to create</param>
        /// <param name="serverRelativeUrl">Server relative URL to the location where to create the new folder</param>
        /// <param name="httpClient">HttpClient to use for the SharePoint communication</param>
        /// <returns>Server relative URL of the new folder if successful or NULL if failed to create the new folder</returns>
        public static async Task<string> CreateFolder(string folderName, string serverRelativeUrl, HttpClient httpClient)
        {
            try
            {
                // Get a FormDigest to send to SharePoint
                var formDigest = await GetFormDigest(httpClient);

                // Validate that the FormDigest was retrieved successfully
                if (string.IsNullOrEmpty(formDigest))
                {
                    // No FormDigest available
                    return null;
                }

                // Construct a new HTTP message
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "web/GetFolderByServerRelativeUrl('" + serverRelativeUrl + "')/Folders/Add('" + folderName + "')?$select=ServerRelativeUrl"))
                {
                    // Add the FormDiges to the request header
                    httpRequest.Headers.Add("X-RequestDigest", formDigest);

                    // Send the request to SharePoint
                    var response = await httpClient.SendAsync(httpRequest);

                    // Verify if the request was processed successfully
                    if (!response.IsSuccessStatusCode)
                    {
                        // Request failed
                        return null;
                    }

                    // Request was successful. Parse the result of the request.
                    var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());

                    // Validate if a ServerRelativeUrl node exists in the result
                    if (responseJson.TryGetValue("ServerRelativeUrl", out JToken value))
                    {
                        // ServerRelativeUrl node exists, return it
                        return value.Value<string>();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// Requests a FormDigest from SharePoint which is needed when requesting changes to SharePoint
        /// </summary>
        /// <param name="httpClient">HttpClient to use for the SharePoint communication</param>
        /// <returns>FormDirect if successful or NULL if unable to retrieve the FormDigest</returns>
        public static async Task<string> GetFormDigest(HttpClient httpClient)
        {
            try
            {
                // Request a RequestDigest to allow uploading to SharePoint
                using (var response = await httpClient.PostAsync("contextInfo", new StringContent("Hello")))
                {
                    // Verify if the RequestDigest was retrieved successfully
                    if (!response.IsSuccessStatusCode)
                    {
                        // Request failed
                        return null;
                    }

                    // Request was successful. Parse the result of the request.
                    var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());

                    // Validate if a FormDigestValue node exists in the result
                    if (responseJson.TryGetValue("FormDigestValue", out JToken value))
                    {
                        // FormDigestValue node exists, return it
                        return value.Value<string>();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// Creates a SharePoint ClientContext based on a Configuration file specific for a SharePoint synchronization
        /// </summary>
        /// <param name="databaseConfig">Configuration set to be specific for a SharePoint synchronization</param>
        /// <returns>SharePoint HttpClient or NULL if unable to establish one based on the provided configuration</returns>
        public static HttpClient CreateSharePointHttpClient(Configuration databaseConfig)
        {
            // Collect the SharePoint variables required to connect
            var sharePointUri = new Uri(databaseConfig.RemoteDatabasePath);
            var sharePointClientId = databaseConfig.RefreshToken.Remove(databaseConfig.RefreshToken.IndexOf(';'));
            var sharePointClientSecret = databaseConfig.RefreshToken.Remove(0, databaseConfig.RefreshToken.IndexOf(';') + 1);

            return CreateSharePointHttpClient(sharePointUri, sharePointClientId, sharePointClientSecret);
        }

        /// <summary>
        /// Creates a SharePoint ClientContext based on the provided SharePoint Uri, ClientId and ClientSecret
        /// </summary>
        /// <param name="sharePointUri">Uri of the SharePoint site to connect to</param>
        /// <param name="sharePointClientId">ClientId to use for the Low Trust to connect to SharePoint</param>
        /// <param name="sharePointClientSecret">ClientSecret to use for the Low Trust to connect to SharePoint</param>
        /// <returns>SharePoint HttpClient or NULL if unable to establish one based on the provided configuration</returns>
        public static HttpClient CreateSharePointHttpClient(Uri sharePointUri, string sharePointClientId, string sharePointClientSecret)
        {
            // Get the realm for the SharePoint site
            var realm = TokenHelper.GetRealmFromTargetUrl(sharePointUri);

            // Get the access token for the URL
            var accessToken = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, sharePointUri.Authority, realm, sharePointClientId, sharePointClientSecret).AccessToken;

            // Connect to SharePoint
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = Utilities.GetProxySettings(),
                PreAuthenticate = true,
                UseDefaultCredentials = false,
                Credentials = Utilities.GetProxyCredentials()
            };

            // Set the base URI to use for all calls
            var httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(sharePointUri.OriginalString + "/_api/")
            };

            // Configure the HTTP headers for each request
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json; odata=nometadata");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var assemblyVersion = Assembly.GetCallingAssembly().GetName().Version;
            httpClient.DefaultRequestHeaders.Add("User-Agent", string.Format("KoenZomers KeePass OneDriveSync v{0}.{1}.{2}", assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build));

            return httpClient;
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
        /// Test the connection with the provided HttpClient
        /// </summary>
        /// <param name="httpClient">The HttpClient to use to test the connection</param>
        /// <param name="databaseConfig">If config is provided, the drive name will be updated with the actual title (optional)</param>
        /// <returns>True if connection successful, False if the test failed</returns>
        public static async Task<bool> TestConnection(HttpClient httpClient, Configuration databaseConfig = null)
        {
            try
            {
                // Perform a simple get operation to test the access to SharePoint
                using (var response = await httpClient.GetAsync("web?$select=Title"))
                {

                    // Verify if the request was successful
                    if (!response.IsSuccessStatusCode)
                    {
                        // Request failed
                        return false;
                    }

                    // Request was successful. Check if a database config was provided which we can update with the actual site title.
                    if (databaseConfig == null)
                    {
                        // No database config was provided. No need to parse the result.
                        return true;
                    }

                    // Database config was provided. Parse the result of the request.
                    var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());

                    // Validate if a Title node exists in the result
                    if (responseJson.TryGetValue("Title", out JToken value))
                    {
                        // Title node exists, update the database config with the site title
                        databaseConfig.OneDriveName = value.Value<string>();
                    }
                }

                return true;                
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Ensures that the provided database config contains information to connect to SharePoint. If not, it will prompt for the end user to provide the details.
        /// </summary>
        /// <param name="databaseConfig">Databaseconfig to check for the presence of SharePoint authentication information</param>
        /// <returns>True if succeeded to get SharePoint authentication information, false if failed</returns>
        public static async Task<bool> EnsureSharePointCredentials(Configuration databaseConfig)
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

                        using (var httpClient = CreateSharePointHttpClient(databaseConfig))
                        {
                            if (! await TestConnection(httpClient, databaseConfig))
                            {
                                MessageBox.Show("Connection failed. Please ensure you are able to connect to the SharePoint farm", "Connecting to SharePoint", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                retryGettingApiInstance = true;
                            }
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

            return true;
        }

        /// <summary>
        /// Download a KeePass database from SharePoint
        /// </summary>
        /// <param name="databaseConfig">Configuration of the database to sync</param>
        /// <param name="updateStatus">Action to write status messages to to display in the UI</param>
        /// <returns>Path to the local KeePass database or NULL if the process has been aborted</returns>
        public static async Task<string> OpenFromSharePoint(Configuration databaseConfig, Action<string> updateStatus)
        {
            if(! await EnsureSharePointCredentials(databaseConfig))
            {
                return null;
            }

            using (var httpClient = CreateSharePointHttpClient(databaseConfig))
            {
                // Ask the user where to store the database on SharePoint
                var sharePointDocumentLibraryPickerDialog = new Forms.SharePointDocumentLibraryPickerDialog(httpClient)
                {
                    ExplanationText = "Select the KeePass database to open. Right click for additional options.",
                    AllowEnteringNewFileName = false
                };
                await sharePointDocumentLibraryPickerDialog.LoadDocumentLibraryItems();

                var result = sharePointDocumentLibraryPickerDialog.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrEmpty(sharePointDocumentLibraryPickerDialog.SelectedDocumentLibraryServerRelativeUrl))
                {
                    updateStatus("Open KeePass database from SharePoint aborted");
                    return null;
                }
                databaseConfig.RemoteFolderId = sharePointDocumentLibraryPickerDialog.SelectedDocumentLibraryServerRelativeUrl;
                databaseConfig.RemoteFileName = sharePointDocumentLibraryPickerDialog.FileName;

                // Show the save as dialog to select a location locally where to store the KeePass database
                var saveFiledialog = new SaveFileDialog
                {
                    Filter = "KeePass databases (*.kdbx)|*.kdbx|All Files (*.*)|*.*",
                    Title = "Select where to store the KeePass database locally",
                    CheckFileExists = false,
                    FileName = sharePointDocumentLibraryPickerDialog.FileName
                };

                var saveFileDialogResult = saveFiledialog.ShowDialog();
                if (saveFileDialogResult != DialogResult.OK || string.IsNullOrEmpty(saveFiledialog.FileName))
                {
                    updateStatus("Open KeePass database from SharePoint aborted");
                    return null;
                }

                // Download the KeePass database to the selected location
                updateStatus("Downloading KeePass database");

                // Retrieve the KeePass database from SharePoint
                var serverRelativeSharePointUrl = string.Concat(databaseConfig.RemoteFolderId, "/", databaseConfig.RemoteFileName);
                var downloadSuccessful = await DownloadFile(saveFiledialog.FileName, serverRelativeSharePointUrl, httpClient);

                // Get the ETag of the database so we can determine if it's in sync
                databaseConfig.ETag = await GetEtagOfFile(httpClient, serverRelativeSharePointUrl);

                return downloadSuccessful ? saveFiledialog.FileName : null;
            }
        }
    }
} 