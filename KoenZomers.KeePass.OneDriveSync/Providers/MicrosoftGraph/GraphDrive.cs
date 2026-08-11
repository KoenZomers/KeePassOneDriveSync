using System.Text.Json.Serialization;

namespace KoenZomersKeePassOneDriveSync.Providers.MicrosoftGraph
{
    internal sealed class GraphDrive
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("webUrl")]
        public string WebUrl { get; set; }
    }
}
