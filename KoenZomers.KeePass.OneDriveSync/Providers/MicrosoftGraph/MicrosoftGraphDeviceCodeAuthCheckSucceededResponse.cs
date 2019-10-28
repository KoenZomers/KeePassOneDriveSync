using Newtonsoft.Json;

namespace KoenZomersKeePassOneDriveSync.Providers.MicrosoftGraph
{
    /// <summary>
    /// Response message from Microsoft Graph when succeeding verification on if the device session has authenticated
    /// </summary>
    public class MicrosoftGraphDeviceCodeAuthCheckSucceededResponse
    {
        /// <summary>
        /// Type of token received. Must be Bearer.
        /// </summary>
        [JsonProperty("token_type", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenType { get; set; }

        /// <summary>
        /// Scopes that were granted
        /// </summary>
        [JsonProperty("scope", NullValueHandling = NullValueHandling.Ignore)]
        public string Scope { get; set; }

        /// <summary>
        /// Time in seconds when this token will expire
        /// </summary>
        [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExpiresIn { get; set; }

        /// <summary>
        /// Time in seconds when this token will expire 
        /// </summary>
        [JsonProperty("ext_expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExtExpiresIn { get; set; }

        /// <summary>
        /// The access token
        /// </summary>
        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }

        /// <summary>
        /// The refresh token
        /// </summary>
        [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }
    }
}
