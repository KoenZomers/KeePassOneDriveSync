using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KoenZomersKeePassOneDriveSync.Providers.MicrosoftGraph
{
    internal sealed class SharePointGraphClient : IDisposable
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        private readonly HttpClient _httpClient;

        public SharePointGraphClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GraphSite> GetSiteByUrl(Uri siteUrl)
        {
            var relativePath = EscapeGraphPath(siteUrl.AbsolutePath.Trim('/'));
            var requestUrl = string.IsNullOrEmpty(relativePath) ? string.Format("sites/{0}:/", siteUrl.Host) : string.Format("sites/{0}:/{1}", siteUrl.Host, relativePath);
            return await GetAsync<GraphSite>(requestUrl);
        }

        public async Task<GraphDrive[]> GetSiteDrives(string siteId)
        {
            var response = await GetAsync<GraphCollectionResponse<GraphDrive>>(string.Format("sites/{0}/drives?$select=id,name,webUrl", siteId));
            return response.Value ?? new GraphDrive[0];
        }

        public Task<GraphDriveItem> GetDriveRootItem(string driveId)
        {
            return GetAsync<GraphDriveItem>(string.Format("drives/{0}/root?$select={1}", driveId, DriveItemSelect));
        }

        public async Task<GraphDriveItem[]> GetDriveRootChildren(string driveId)
        {
            var response = await GetAsync<GraphCollectionResponse<GraphDriveItem>>(string.Format("drives/{0}/root/children?$select={1}", driveId, DriveItemSelect));
            return response.Value ?? new GraphDriveItem[0];
        }

        public async Task<GraphDriveItem[]> GetDriveItemChildren(string driveId, string itemId)
        {
            var response = await GetAsync<GraphCollectionResponse<GraphDriveItem>>(string.Format("drives/{0}/items/{1}/children?$select={2}", driveId, itemId, DriveItemSelect));
            return response.Value ?? new GraphDriveItem[0];
        }

        public Task<GraphDriveItem> GetDriveItem(string driveId, string itemId)
        {
            return GetAsync<GraphDriveItem>(string.Format("drives/{0}/items/{1}?$select={2}", driveId, itemId, DriveItemSelect));
        }

        public async Task<GraphDriveItem> GetItemInFolder(string driveId, string folderId, string fileName)
        {
            var children = await GetDriveItemChildren(driveId, folderId);
            return children.FirstOrDefault(item => item.File != null && string.Equals(item.Name, fileName, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<bool> DownloadItemAndSaveAs(string driveId, string itemId, string localPath)
        {
            using (var response = await _httpClient.GetAsync(string.Format("drives/{0}/items/{1}/content", driveId, itemId)))
            {
                await EnsureSuccess(response);

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = File.Create(localPath))
                {
                    await responseStream.CopyToAsync(fileStream);
                }
            }

            return true;
        }

        public async Task<GraphDriveItem> UploadFileAs(string localPath, string fileName, string driveId, string parentFolderId)
        {
            var requestUrl = string.Format("drives/{0}/items/{1}:/{2}:/content", driveId, parentFolderId, EscapeGraphPath(fileName));
            return await PutFileContent(requestUrl, localPath);
        }

        public Task<GraphDriveItem> UpdateFile(string localPath, string driveId, string itemId)
        {
            return PutFileContent(string.Format("drives/{0}/items/{1}/content", driveId, itemId), localPath);
        }

        public async Task<GraphDriveItem> CreateFolder(string folderName, string driveId, string parentFolderId)
        {
            using (var content = new StringContent("{\"name\":\"" + EscapeJson(folderName) + "\",\"folder\":{},\"@microsoft.graph.conflictBehavior\":\"fail\"}", Encoding.UTF8, "application/json"))
            using (var response = await _httpClient.PostAsync(string.Format("drives/{0}/items/{1}/children", driveId, parentFolderId), content))
            {
                await EnsureSuccess(response);
                return JsonSerializer.Deserialize<GraphDriveItem>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions);
            }
        }

        public async Task DeleteItem(string driveId, string itemId)
        {
            using (var response = await _httpClient.DeleteAsync(string.Format("drives/{0}/items/{1}", driveId, itemId)))
            {
                await EnsureSuccess(response);
            }
        }

        public async Task RenameItem(string newName, string driveId, string itemId)
        {
            using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), string.Format("drives/{0}/items/{1}", driveId, itemId)))
            {
                request.Content = new StringContent("{\"name\":\"" + EscapeJson(newName) + "\"}", Encoding.UTF8, "application/json");
                using (var response = await _httpClient.SendAsync(request))
                {
                    await EnsureSuccess(response);
                }
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private async Task<T> GetAsync<T>(string requestUrl)
        {
            using (var response = await _httpClient.GetAsync(requestUrl))
            {
                await EnsureSuccess(response);
                return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions);
            }
        }

        private async Task<GraphDriveItem> PutFileContent(string requestUrl, string localPath)
        {
            using (var fileContent = new StreamContent(File.OpenRead(localPath)))
            {
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                using (var response = await _httpClient.PutAsync(requestUrl, fileContent))
                {
                    await EnsureSuccess(response);
                    return JsonSerializer.Deserialize<GraphDriveItem>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions);
                }
            }
        }

        private static async Task EnsureSuccess(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(string.Format("Microsoft Graph request failed with HTTP {0} {1}: {2}", (int)response.StatusCode, response.ReasonPhrase, responseContent));
        }

        private static string EscapeGraphPath(string path)
        {
            return string.Join("/", path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Select(segment => Uri.EscapeDataString(Uri.UnescapeDataString(segment))));
        }

        private static string EscapeJson(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private const string DriveItemSelect = "id,name,cTag,eTag,size,createdDateTime,lastModifiedDateTime,folder,file,parentReference";
    }
}
