using System.Text.Json.Serialization;

namespace Proctorio.Client.Webhooks.Requests.V2
{
    /// <summary>
    /// Represents the data structure for a webhook request containing JSON objects.
    /// </summary>
    /// <remarks>
    /// This class serves as a container for the JSON payloads received via webhook.
    /// It holds the necessary properties to represent the data provided in the webhook
    /// responses for further processing or analysis.
    /// </remarks>
    public class WebhookRequest
    {
        /// <summary>
        /// Object which contains information about webhook data.
        /// </summary>
        [JsonPropertyName("data")]
        public WebhookRequestData Data { get; set; }

        /// <summary>
        /// Webhook type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// A nonce (number used once) is a unique value that is generated for each request.
        /// It helps to prevent replay attacks by ensuring that old requests cannot be reused.
        /// </summary>
        ///
        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        /// <summary>
        /// sha1 (nonce+”:”+”{{json stringified request.data(no spaces))}}”+”:”+{{secret}})- secret - value which proctorio initially shared with the client.
        /// </summary>
        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        /// <summary>
        /// Key is used on your end to authorize the request. This is optional. You can choose a proper auth method for your API: ApiKey or signature validation.
        /// </summary>
        [JsonPropertyName("api_key")]
        public string ApiKey { get; set; }
    }

    /// <summary>
    /// Object which contains information about webhook data.
    /// </summary>
    public class WebhookRequestData
    {
        /// <summary>
        /// Unique value for proctorio internal purposes.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Represents the information about attempt id. If not passed returns a random uuid.  
        /// </summary>
        [JsonPropertyName("attempt_id")]
        public string AttemptId { get; set; }

        /// <summary>
        /// suspicion Suspicion calculation calculated based on different parameters including behavior settings. This is a static value calculated when the test taker submitted the exam.
        /// </summary>
        [JsonPropertyName("suspicion")]
        public double Suspicion { get; set; }
    }
}