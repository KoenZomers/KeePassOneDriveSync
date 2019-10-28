using Newtonsoft.Json;
using System;

namespace KoenZomersKeePassOneDriveSync.Providers.MicrosoftGraph
{
    /// <summary>
    /// Response message from Microsoft Graph when failing verification on if the device session has authenticated
    /// </summary>
    public class MicrosoftGraphDeviceCodeAuthCheckFailedResponse
    {
        /// <summary>
        /// Type of error
        /// </summary>
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        /// <summary>
        /// Detailed description of what went wrong
        /// </summary>
        [JsonProperty("error_description", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Code(s) identifying the type of error
        /// </summary>
        [JsonProperty("error_codes", NullValueHandling = NullValueHandling.Ignore)]
        public long[] ErrorCodes { get; set; }

        /// <summary>
        /// Date and time when the error occurred
        /// </summary>
        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Trace ID of the error
        /// </summary>
        [JsonProperty("trace_id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? TraceId { get; set; }

        /// <summary>
        /// Correlation ID of the error
        /// </summary>
        [JsonProperty("correlation_id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CorrelationId { get; set; }

        /// <summary>
        /// Url with more information on the error
        /// </summary>
        [JsonProperty("error_uri", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ErrorUri { get; set; }
    }
}
