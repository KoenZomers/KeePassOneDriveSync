using System;
using System.Text.Json.Serialization;

namespace KoenZomersKeePassOneDriveSync.Providers.MicrosoftGraph
{
    internal sealed class GraphDriveItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("cTag")]
        public string CTag { get; set; }

        [JsonPropertyName("eTag")]
        public string ETag { get; set; }

        [JsonPropertyName("size")]
        public long? Size { get; set; }

        [JsonPropertyName("createdDateTime")]
        public DateTimeOffset? CreatedDateTime { get; set; }

        [JsonPropertyName("lastModifiedDateTime")]
        public DateTimeOffset? LastModifiedDateTime { get; set; }

        [JsonPropertyName("folder")]
        public GraphDriveItemFolderFacet Folder { get; set; }

        [JsonPropertyName("file")]
        public GraphDriveItemFileFacet File { get; set; }

        [JsonPropertyName("parentReference")]
        public GraphItemReference ParentReference { get; set; }
    }
}
