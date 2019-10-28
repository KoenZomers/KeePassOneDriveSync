using Newtonsoft.Json;
using System;

namespace KoenZomersKeePassOneDriveSync.Providers.MicrosoftGraph
{
    /// <summary>
    /// Response message from Microsoft Graph after requesting a device login
    /// </summary>
    public class MicrosoftGraphDeviceCodeResponse
    {
        /// <summary>
        /// A short string shown to the user that's used to identify the session on a secondary device
        /// </summary>
        [JsonProperty("user_code", NullValueHandling = NullValueHandling.Ignore)]
        public string UserCode { get; set; }

        /// <summary>
        /// A long string used to verify the session between the client and the authorization server. The client uses this parameter to request the access token from the authorization server.
        /// </summary>
        [JsonProperty("device_code", NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceCode { get; set; }

        /// <summary>
        /// The URI the user should go to with the user_code in order to sign in
        /// </summary>
        [JsonProperty("verification_uri", NullValueHandling = NullValueHandling.Ignore)]
        public Uri VerificationUri { get; set; }

        /// <summary>
        /// The number of seconds before the device_code and user_code expire
        /// </summary>
        [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpiresIn { get; set; }

        /// <summary>
        /// The number of seconds the client should wait between polling requests
        /// </summary>
        [JsonProperty("interval", NullValueHandling = NullValueHandling.Ignore)]
        public int? Interval { get; set; }

        /// <summary>
        /// A human-readable string with instructions for the user. This can be localized by including a query parameter in the request of the form ?mkt=xx-XX, filling in the appropriate language culture code.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
