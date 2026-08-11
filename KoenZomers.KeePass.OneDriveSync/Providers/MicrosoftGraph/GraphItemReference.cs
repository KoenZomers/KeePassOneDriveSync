using System.Text.Json.Serialization;

namespace KoenZomersKeePassOneDriveSync.Providers.MicrosoftGraph
{
    internal sealed class GraphItemReference
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("driveId")]
        public string DriveId { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }
    }
}
