using KoenZomers.KeePass.OneDriveSync;
using KoenZomers.KeePass.OneDriveSync.Enums;
using KoenZomersKeePassOneDriveSync.Providers.MicrosoftGraph;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
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
            if (databaseConfig.CloudStorageType == CloudStorageType.SharePointOnPremises)
            {
                return await SyncUsingSharePointOnPremisesPlatform(databaseConfig, localKeePassDatabasePath, forceSync, updateStatus);
            }

            if (!await EnsureSharePointCredentials(databaseConfig))
            {
                return false;
            }

            using (var graphClient = await CreateSharePointGraphClient(databaseConfig))
            {
                if (graphClient == null)
                {
                    updateStatus(string.Format("Failed to connect to SharePoint for database {0}", databaseConfig.KeePassDatabase.Name));
                    return false;
                }

                var site = await graphClient.GetSiteByUrl(new Uri(databaseConfig.RemoteDatabasePath));
                databaseConfig.OneDriveName = site.DisplayName;

                if (string.IsNullOrEmpty(databaseConfig.RemoteDriveId) && string.IsNullOrEmpty(databaseConfig.RemoteFolderId) && string.IsNullOrEmpty(databaseConfig.RemoteFileName))
                {
                    var sharePointDocumentLibraryPickerDialog = new Forms.SharePointDocumentLibraryPickerDialog(graphClient, site.Id)
                    {
                        FileName = !string.IsNullOrEmpty(databaseConfig.RemoteFileName) ? databaseConfig.RemoteFileName : new System.IO.FileInfo(localKeePassDatabasePath).Name
                    };
                    await sharePointDocumentLibraryPickerDialog.LoadDocumentLibraryItems();
                    var result = sharePointDocumentLibraryPickerDialog.ShowDialog();
                    if (result != DialogResult.OK || string.IsNullOrEmpty(sharePointDocumentLibraryPickerDialog.SelectedDriveId) || string.IsNullOrEmpty(sharePointDocumentLibraryPickerDialog.SelectedFolderId))
                    {
                        return false;
                    }

                    databaseConfig.RemoteDriveId = sharePointDocumentLibraryPickerDialog.SelectedDriveId;
                    databaseConfig.RemoteFolderId = sharePointDocumentLibraryPickerDialog.SelectedFolderId;
                    databaseConfig.RemoteItemId = sharePointDocumentLibraryPickerDialog.SelectedFileId;
                    databaseConfig.RemoteFileName = sharePointDocumentLibraryPickerDialog.FileName;
                    Configuration.Save();
                }

                GraphDriveItem sharePointItem = null;
                if (!string.IsNullOrEmpty(databaseConfig.RemoteItemId))
                {
                    try
                    {
                        sharePointItem = await graphClient.GetDriveItem(databaseConfig.RemoteDriveId, databaseConfig.RemoteItemId);
                    }
                    catch (HttpRequestException)
                    {
                        databaseConfig.RemoteItemId = null;
                    }
                }

                if (sharePointItem == null && !string.IsNullOrEmpty(databaseConfig.RemoteFolderId) && !string.IsNullOrEmpty(databaseConfig.RemoteFileName))
                {
                    sharePointItem = await graphClient.GetItemInFolder(databaseConfig.RemoteDriveId, databaseConfig.RemoteFolderId, databaseConfig.RemoteFileName);
                    if (sharePointItem != null)
                    {
                        databaseConfig.RemoteItemId = sharePointItem.Id;
                        Configuration.Save();
                    }
                }

                if (sharePointItem == null)
                {
                    updateStatus(string.Format("Database {0} does not exist yet on SharePoint, uploading it now", databaseConfig.KeePassDatabase.Name));

                    var newUploadResult = await graphClient.UploadFileAs(databaseConfig.KeePassDatabase.IOConnectionInfo.Path, databaseConfig.RemoteFileName, databaseConfig.RemoteDriveId, databaseConfig.RemoteFolderId);

                    updateStatus(string.Format(newUploadResult == null ? "Failed to upload the KeePass database {0}" : "Successfully uploaded the new KeePass database {0} to SharePoint", databaseConfig.KeePassDatabase.Name));

                    databaseConfig.LocalFileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);
                    if (newUploadResult != null)
                    {
                        databaseConfig.RemoteItemId = newUploadResult.Id;
                        databaseConfig.LastCheckedAt = DateTime.Now;
                        databaseConfig.LastSyncedAt = DateTime.Now;
                        databaseConfig.ETag = newUploadResult.CTag ?? newUploadResult.ETag;
                        databaseConfig.RemoteLastModifiedAt = newUploadResult.LastModifiedDateTime;
                    }
                    Configuration.Save();
                    return false;
                }

                var remoteChangeTag = sharePointItem.CTag ?? sharePointItem.ETag;
                if (!forceSync &&
                    remoteChangeTag == databaseConfig.ETag &&
                    databaseConfig.RemoteLastModifiedAt.HasValue &&
                    sharePointItem.LastModifiedDateTime == databaseConfig.RemoteLastModifiedAt &&
                    Utilities.GetDatabaseFileHash(localKeePassDatabasePath) == databaseConfig.LocalFileHash)
                {
                    updateStatus(string.Format("KeePass database {0} is in sync", databaseConfig.KeePassDatabase.Name));

                    databaseConfig.LastCheckedAt = DateTime.Now;
                    Configuration.Save();

                    return false;
                }

                updateStatus(string.Format("Downloading KeePass database {0} from SharePoint", databaseConfig.KeePassDatabase.Name));

                var temporaryKeePassDatabasePath = System.IO.Path.GetTempFileName();
                var downloadSuccessful = await graphClient.DownloadItemAndSaveAs(databaseConfig.RemoteDriveId, sharePointItem.Id, temporaryKeePassDatabasePath);

                if (!downloadSuccessful)
                {
                    updateStatus(string.Format("Failed to download the KeePass database {0} from SharePoint", databaseConfig.KeePassDatabase.Name));

                    return false;
                }

                updateStatus(string.Format("KeePass database {0} downloaded, going to sync", databaseConfig.KeePassDatabase.Name));

                var syncSuccessful = KeePassDatabase.MergeDatabases(databaseConfig, temporaryKeePassDatabasePath);

                string localDatabaseToUpload;
                if (!syncSuccessful)
                {
                    updateStatus(string.Format("Failed to synchronize the KeePass database {0}", databaseConfig.KeePassDatabase.Name));

                    var confirm = MessageBox.Show(string.Format("Unable to merge the KeePass database {0}. Did you just change the master password for this KeePass database? If so and you would like to OVERWRITE the KeePass database stored on your SharePoint site with your local database, select Yes, otherwise select No.", databaseConfig.KeePassDatabase.Name), "Confirm overwriting your KeePass database", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                    if (confirm != DialogResult.Yes) return false;

                    updateStatus(string.Format("Uploading the local KeePass database {0} to SharePoint", databaseConfig.KeePassDatabase.Name));

                    localDatabaseToUpload = databaseConfig.KeePassDatabase.IOConnectionInfo.Path;
                }
                else
                {
                    updateStatus(string.Format("Uploading the merged KeePass database {0} to SharePoint", databaseConfig.KeePassDatabase.Name));

                    localDatabaseToUpload = temporaryKeePassDatabasePath;
                }

                var uploadResult = await graphClient.UpdateFile(localDatabaseToUpload, databaseConfig.RemoteDriveId, sharePointItem.Id);

                System.IO.File.Delete(temporaryKeePassDatabasePath);

                databaseConfig.RemoteItemId = uploadResult.Id;
                databaseConfig.ETag = uploadResult.CTag ?? uploadResult.ETag;
                databaseConfig.RemoteLastModifiedAt = uploadResult.LastModifiedDateTime;
                databaseConfig.LocalFileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);
                databaseConfig.LastCheckedAt = DateTime.Now;
                databaseConfig.LastSyncedAt = DateTime.Now;
                Configuration.Save();
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
                using (var responseJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                {
                    // Validate if ETag node exists in the result
                    JsonElement value;
                    if (responseJson.RootElement.TryGetProperty("ETag", out value))
                    {
                        // ETag node exists, return it
                        return value.GetString();
                    }
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
        public static async Task<string> UploadFile(string localDatabasePath, string serverRelativeUrl, string fileName, HttpClient httpClient, bool asNewFile)
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

                if(!asNewFile)
                {
                    // Send a check out request for the file, in case require checkout is enabled on the SharePoint Document Library
                    using (var checkOutRequest = new HttpRequestMessage(HttpMethod.Post, "web/GetFileByServerRelativeUrl('" + serverRelativeUrl + "/" + fileName + "')/CheckOut()"))
                    {
                        checkOutRequest.Headers.Add("X-RequestDigest", formDigest);
                        await httpClient.SendAsync(checkOutRequest);
                    }
                }

                // Construct a new HTTP message
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, asNewFile ? "web/GetFolderByServerRelativeUrl('" + serverRelativeUrl + "')/Files/Add(url='" + fileName + "',overwrite=true)?$select=ETag" :
                                                                                              "web/GetFileByServerRelativeUrl('" + serverRelativeUrl + "/" + fileName + "')/$value"))
                {
                    // Add the FormDiges to the request header
                    httpRequest.Headers.Add("X-RequestDigest", formDigest);
                    
                    if (!asNewFile)
                    {
                        // Add the header to indicate an update to an existing file should be done
                        httpRequest.Headers.Add("X-HTTP-Method", "PUT");
                    }

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

                        // Send a check in request of the file, in case require checkout is enabled on the SharePoint Document Library
                        using (var checkInRequest = new HttpRequestMessage(HttpMethod.Post, "web/GetFileByServerRelativeUrl('" + serverRelativeUrl + "/" + fileName + "')/CheckIn(comment='" + (asNewFile ? "Added" : "Updated") + " by " + httpClient.DefaultRequestHeaders.UserAgent + "',checkintype=0)"))
                        {
                            checkInRequest.Headers.Add("X-RequestDigest", formDigest);
                            await httpClient.SendAsync(checkInRequest);
                        }

                        if (asNewFile)
                        {
                            // Parse the result of the upload new file request to get the ETag
                            using (var responseJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                            {
                                // Validate if a ETag node exists in the result
                                JsonElement value;
                                if (responseJson.RootElement.TryGetProperty("ETag", out value))
                                {
                                    // ETag node exists, return it
                                    return value.GetString();
                                }
                            }
                        }
                        else
                        {
                            // Updating a file does not support retrieval of the new ETag, so send out another request to get it
                            var eTag = await GetEtagOfFile(httpClient, serverRelativeUrl + "/" + fileName);
                            return eTag;
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
            catch (Exception)
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
                    using (var responseJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                    {
                        // Validate if a ServerRelativeUrl node exists in the result
                        JsonElement value;
                        if (responseJson.RootElement.TryGetProperty("ServerRelativeUrl", out value))
                        {
                            // ServerRelativeUrl node exists, return it
                            return value.GetString();
                        }
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
                    using (var responseJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                    {
                        // Validate if a FormDigestValue node exists in the result
                        JsonElement value;
                        if (responseJson.RootElement.TryGetProperty("FormDigestValue", out value))
                        {
                            // FormDigestValue node exists, return it
                            return value.GetString();
                        }
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
        /// Creates a Microsoft Graph SharePoint client based on a Configuration file specific for a SharePoint synchronization.
        /// </summary>
        /// <param name="databaseConfig">Configuration set to be specific for a SharePoint synchronization</param>
        /// <returns>SharePoint Graph client or NULL if authentication failed</returns>
        public static async Task<SharePointGraphClient> CreateSharePointGraphClient(Configuration databaseConfig)
        {
            var httpClient = await Utilities.GetSharePointGraphHttpClient(databaseConfig);
            return httpClient == null ? null : new SharePointGraphClient(httpClient);
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
            httpClient.DefaultRequestHeaders.Add("User-Agent", string.Format("KoenZomers KeePass OneDriveSync v{0}.{1}.{2}.{3}", assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build, assemblyVersion.Revision));

            return httpClient;
        }

        /// <summary>
        /// Form to request the SharePoint details to connect to
        /// </summary>
        /// <param name="databaseConfig">Configuration which needs to map to a SharePoint environment</param>
        /// <returns>True if successful, false if failed to receive a SharePoint configuration</returns>
        private static bool RequestSharePointDetails(Configuration databaseConfig, bool useMicrosoftGraph = true)
        {
            var sharePointCredentialsDialog = new Forms.SharePointCredentialsForm(useMicrosoftGraph);
            var result = sharePointCredentialsDialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return false;
            }

            databaseConfig.RemoteDatabasePath = sharePointCredentialsDialog.SharePointUrl;
            if (useMicrosoftGraph)
            {
                if (!string.IsNullOrEmpty(databaseConfig.RefreshToken) && databaseConfig.RefreshToken.IndexOf(';') != -1)
                {
                    databaseConfig.RefreshToken = null;
                }
                databaseConfig.RemoteDriveId = null;
                databaseConfig.RemoteFolderId = null;
                databaseConfig.RemoteItemId = null;
                databaseConfig.RemoteFileName = null;
            }
            else
            {
                databaseConfig.RefreshToken = string.Format("{0};{1}", sharePointCredentialsDialog.SharePointClientId, sharePointCredentialsDialog.SharePointClientSecret);
            }
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
                    using (var responseJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                    {
                        // Validate if a Title node exists in the result
                        JsonElement value;
                        if (responseJson.RootElement.TryGetProperty("Title", out value))
                        {
                            // Title node exists, update the database config with the site title
                            databaseConfig.OneDriveName = value.GetString();
                        }
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
        /// Test the connection with the provided SharePoint Graph client.
        /// </summary>
        /// <param name="graphClient">The SharePoint Graph client to use to test the connection</param>
        /// <param name="databaseConfig">If config is provided, the drive name will be updated with the actual title (optional)</param>
        /// <returns>True if connection successful, False if the test failed</returns>
        public static async Task<bool> TestConnection(SharePointGraphClient graphClient, Configuration databaseConfig = null)
        {
            try
            {
                if (databaseConfig == null || string.IsNullOrEmpty(databaseConfig.RemoteDatabasePath))
                {
                    return false;
                }

                var site = await graphClient.GetSiteByUrl(new Uri(databaseConfig.RemoteDatabasePath));
                databaseConfig.OneDriveName = site.DisplayName;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async Task<bool> SyncUsingSharePointOnPremisesPlatform(Configuration databaseConfig, string localKeePassDatabasePath, bool forceSync, Action<string> updateStatus)
        {
            if (!await EnsureSharePointOnPremisesCredentials(databaseConfig))
            {
                return false;
            }

            using (var httpClient = CreateSharePointHttpClient(databaseConfig))
            {
                if (string.IsNullOrEmpty(databaseConfig.RemoteFolderId) && string.IsNullOrEmpty(databaseConfig.RemoteFileName))
                {
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

                if (string.IsNullOrEmpty(databaseConfig.OneDriveName))
                {
                    await TestConnection(httpClient, databaseConfig);
                    Configuration.Save();
                }

                var serverRelativeSharePointUrl = string.Concat(databaseConfig.RemoteFolderId, "/", databaseConfig.RemoteFileName);
                var eTag = await GetEtagOfFile(httpClient, serverRelativeSharePointUrl);

                if (eTag == null)
                {
                    updateStatus(string.Format("Database {0} does not exist yet on SharePoint, uploading it now", databaseConfig.KeePassDatabase.Name));
                    eTag = await UploadFile(databaseConfig.KeePassDatabase.IOConnectionInfo.Path, databaseConfig.RemoteFolderId, databaseConfig.RemoteFileName, httpClient, true);
                    updateStatus(string.Format(eTag == null ? "Failed to upload the KeePass database {0}" : "Successfully uploaded the new KeePass database {0} to SharePoint", databaseConfig.KeePassDatabase.Name));

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

                if (!forceSync &&
                    eTag == databaseConfig.ETag &&
                    Utilities.GetDatabaseFileHash(localKeePassDatabasePath) == databaseConfig.LocalFileHash)
                {
                    updateStatus(string.Format("KeePass database {0} is in sync", databaseConfig.KeePassDatabase.Name));
                    databaseConfig.LastCheckedAt = DateTime.Now;
                    Configuration.Save();
                    return false;
                }

                updateStatus(string.Format("Downloading KeePass database {0} from SharePoint", databaseConfig.KeePassDatabase.Name));
                var temporaryKeePassDatabasePath = System.IO.Path.GetTempFileName();
                var downloadSuccessful = await DownloadFile(temporaryKeePassDatabasePath, serverRelativeSharePointUrl, httpClient);

                if (!downloadSuccessful)
                {
                    updateStatus(string.Format("Failed to download the KeePass database {0} from SharePoint", databaseConfig.KeePassDatabase.Name));
                    return false;
                }

                updateStatus(string.Format("KeePass database {0} downloaded, going to sync", databaseConfig.KeePassDatabase.Name));
                var syncSuccessful = KeePassDatabase.MergeDatabases(databaseConfig, temporaryKeePassDatabasePath);

                string localDatabaseToUpload;
                if (!syncSuccessful)
                {
                    updateStatus(string.Format("Failed to synchronize the KeePass database {0}", databaseConfig.KeePassDatabase.Name));

                    var confirm = MessageBox.Show(string.Format("Unable to merge the KeePass database {0}. Did you just change the master password for this KeePass database? If so and you would like to OVERWRITE the KeePass database stored on your SharePoint site with your local database, select Yes, otherwise select No.", databaseConfig.KeePassDatabase.Name), "Confirm overwriting your KeePass database", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                    if (confirm != DialogResult.Yes) return false;

                    updateStatus(string.Format("Uploading the local KeePass database {0} to SharePoint", databaseConfig.KeePassDatabase.Name));
                    localDatabaseToUpload = databaseConfig.KeePassDatabase.IOConnectionInfo.Path;
                }
                else
                {
                    updateStatus(string.Format("Uploading the merged KeePass database {0} to SharePoint", databaseConfig.KeePassDatabase.Name));
                    localDatabaseToUpload = temporaryKeePassDatabasePath;
                }

                var uploadResult = await UploadFile(localDatabaseToUpload, databaseConfig.RemoteFolderId, databaseConfig.RemoteFileName, httpClient, false);
                System.IO.File.Delete(temporaryKeePassDatabasePath);

                databaseConfig.ETag = uploadResult;
                return true;
            }
        }

        private static async Task<bool> EnsureSharePointOnPremisesCredentials(Configuration databaseConfig)
        {
            if (string.IsNullOrEmpty(databaseConfig.RefreshToken) || databaseConfig.RefreshToken.IndexOf(';') == -1 || string.IsNullOrEmpty(databaseConfig.RemoteDatabasePath))
            {
                bool retryGettingApiInstance;
                do
                {
                    retryGettingApiInstance = false;
                    try
                    {
                        var requestSharePointDetailsSuccessful = RequestSharePointDetails(databaseConfig, useMicrosoftGraph: false);

                        if (!requestSharePointDetailsSuccessful)
                        {
                            return false;
                        }

                        using (var httpClient = CreateSharePointHttpClient(databaseConfig))
                        {
                            if (!await TestConnection(httpClient, databaseConfig))
                            {
                                MessageBox.Show("Connection failed. Please ensure you are able to connect to the SharePoint farm", "Connecting to SharePoint", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                retryGettingApiInstance = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        var errorMessage = new System.Text.StringBuilder();
                        errorMessage.Append("Failed to connect to SharePoint:");
                        errorMessage.AppendLine();
                        errorMessage.AppendLine(e.Message);

                        if (e.InnerException != null)
                        {
                            errorMessage.AppendLine(e.InnerException.Message);

                            if (e.InnerException.Message.Contains("remote name could not be resolved"))
                            {
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
        /// Ensures that the provided database config contains information to connect to SharePoint. If not, it will prompt for the end user to provide the details.
        /// </summary>
        /// <param name="databaseConfig">Databaseconfig to check for the presence of SharePoint authentication information</param>
        /// <returns>True if succeeded to get SharePoint authentication information, false if failed</returns>
        public static async Task<bool> EnsureSharePointCredentials(Configuration databaseConfig)
        {
            if (databaseConfig.CloudStorageType == CloudStorageType.SharePointOnPremises)
            {
                return await EnsureSharePointOnPremisesCredentials(databaseConfig);
            }

            if (string.IsNullOrEmpty(databaseConfig.RemoteDatabasePath) || (!string.IsNullOrEmpty(databaseConfig.RefreshToken) && databaseConfig.RefreshToken.IndexOf(';') != -1))
            {
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

                        using (var graphClient = await CreateSharePointGraphClient(databaseConfig))
                        {
                            if (graphClient == null || !await TestConnection(graphClient, databaseConfig))
                            {
                                MessageBox.Show("Connection failed. Please ensure you are able to connect to the SharePoint Online site", "Connecting to SharePoint", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                retryGettingApiInstance = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        var errorMessage = new System.Text.StringBuilder();
                        errorMessage.Append("Failed to connect to SharePoint:");
                        errorMessage.AppendLine();
                        errorMessage.AppendLine(e.Message);

                        if (e.InnerException != null)
                        {
                            errorMessage.AppendLine(e.InnerException.Message);

                            if (e.InnerException.Message.Contains("remote name could not be resolved"))
                            {
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
            if (databaseConfig.CloudStorageType == CloudStorageType.SharePointOnPremises)
            {
                return await OpenFromSharePointOnPremises(databaseConfig, updateStatus);
            }

            if (!await EnsureSharePointCredentials(databaseConfig))
            {
                return null;
            }

            using (var graphClient = await CreateSharePointGraphClient(databaseConfig))
            {
                if (graphClient == null)
                {
                    updateStatus("Failed to connect to SharePoint");
                    return null;
                }

                var site = await graphClient.GetSiteByUrl(new Uri(databaseConfig.RemoteDatabasePath));
                databaseConfig.OneDriveName = site.DisplayName;

                var sharePointDocumentLibraryPickerDialog = new Forms.SharePointDocumentLibraryPickerDialog(graphClient, site.Id)
                {
                    ExplanationText = "Select the KeePass database to open. Right click for additional options.",
                    AllowEnteringNewFileName = false
                };
                await sharePointDocumentLibraryPickerDialog.LoadDocumentLibraryItems();

                var result = sharePointDocumentLibraryPickerDialog.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrEmpty(sharePointDocumentLibraryPickerDialog.SelectedDriveId) || string.IsNullOrEmpty(sharePointDocumentLibraryPickerDialog.SelectedFileId))
                {
                    updateStatus("Open KeePass database from SharePoint aborted");
                    return null;
                }
                databaseConfig.RemoteDriveId = sharePointDocumentLibraryPickerDialog.SelectedDriveId;
                databaseConfig.RemoteFolderId = sharePointDocumentLibraryPickerDialog.SelectedFolderId;
                databaseConfig.RemoteItemId = sharePointDocumentLibraryPickerDialog.SelectedFileId;
                databaseConfig.RemoteFileName = sharePointDocumentLibraryPickerDialog.FileName;

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

                updateStatus("Downloading KeePass database");

                var downloadSuccessful = await graphClient.DownloadItemAndSaveAs(databaseConfig.RemoteDriveId, databaseConfig.RemoteItemId, saveFiledialog.FileName);
                var sharePointItem = await graphClient.GetDriveItem(databaseConfig.RemoteDriveId, databaseConfig.RemoteItemId);
                databaseConfig.ETag = sharePointItem.CTag ?? sharePointItem.ETag;
                databaseConfig.RemoteLastModifiedAt = sharePointItem.LastModifiedDateTime;
                Configuration.Save();

                return downloadSuccessful ? saveFiledialog.FileName : null;
            }
        }

        private static async Task<string> OpenFromSharePointOnPremises(Configuration databaseConfig, Action<string> updateStatus)
        {
            if (!await EnsureSharePointOnPremisesCredentials(databaseConfig))
            {
                return null;
            }

            using (var httpClient = CreateSharePointHttpClient(databaseConfig))
            {
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

                updateStatus("Downloading KeePass database");

                var serverRelativeSharePointUrl = string.Concat(databaseConfig.RemoteFolderId, "/", databaseConfig.RemoteFileName);
                var downloadSuccessful = await DownloadFile(saveFiledialog.FileName, serverRelativeSharePointUrl, httpClient);

                databaseConfig.ETag = await GetEtagOfFile(httpClient, serverRelativeSharePointUrl);

                return downloadSuccessful ? saveFiledialog.FileName : null;
            }
        }
    }
} 
