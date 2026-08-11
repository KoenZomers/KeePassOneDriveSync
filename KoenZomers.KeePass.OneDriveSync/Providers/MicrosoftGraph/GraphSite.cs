using System.Text.Json.Serialization;

namespace KoenZomersKeePassOneDriveSync.Providers.MicrosoftGraph
{
    internal sealed class GraphSite
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("webUrl")]
        public string WebUrl { get; set; }
    }
}
