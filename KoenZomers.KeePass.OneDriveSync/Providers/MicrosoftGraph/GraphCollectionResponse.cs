using System.Text.Json.Serialization;

namespace KoenZomersKeePassOneDriveSync.Providers.MicrosoftGraph
{
    internal sealed class GraphCollectionResponse<T>
    {
        [JsonPropertyName("value")]
        public T[] Value { get; set; }
    }
}
