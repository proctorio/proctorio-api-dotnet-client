using System.Text.Json.Serialization;

namespace Proctorio.Client.Webhooks.Requests.V3
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
        /// Generated key on your end and used to authorize the request. This is optional. You can choose a proper auth method for your API: api_key or signature validation.
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
        /// Suspicion calculation calculated based on different parameters including behavior settings.
        /// This is a static value calculated when the test taker submitted the exam.
        /// </summary>
        [JsonPropertyName("suspicion")]
        public double Suspicion { get; set; }

        /// <summary>
        /// Represents the user_id value sent in candidate launch request.
        /// </summary>
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Represents the object containing submission data. 
        /// </summary>
        [JsonPropertyName("submission_data")]
        public SubmissionData SubmissionData { get; set; }

        /// <summary>
        ///  Represents the information about attempt id. If not passed returns a random uuid.  
        /// </summary>
        [JsonPropertyName("attempt_id")]
        public string AttemptId { get; set; }

        /// <summary>
        /// Represents an object containing information about the percentage (3 decimals) of detected incidents during the exam
        /// </summary>
        [JsonPropertyName("flags")]
        public Flags Flags { get; set; }
    }

    /// <summary>
    /// Represents the object containing submission data.
    /// </summary>
    public class SubmissionData
    {
        /// <summary>
        /// Epoch timestamp (also known as Unix timestamp). Represents the date when the candidate ended the attempt. 
        /// </summary>
        /// <remarks>
        ///  In the event of an ungraceful submission, this date may differ from the platform's recorded submission time.
        /// <remarks>
        [JsonPropertyName("date")]
        public long Date { get; set; }

        /// <summary>
        ///  Proctorio unique number value represents the way the candidate ended the attempt, gracefully or ungracefully. For ex 1 = Submitted, 5 = Ended screen recording , etc.
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        ///  Represents the textual format of the Proctorio close_code message.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// When available, represents additional information regarding close code.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Represents an object containing information about the percentage (3 decimals) of detected incidents during the exam.
    /// </summary>
    public class Flags
    {
        /// <summary>
        ///  Returns a value when unfocus is detected.
        /// </summary>
        [JsonPropertyName("unfocus_detected")]
        public double UnfocusDetected { get; set; }

        /// <summary>
        /// Returns a value when a copy or paste command is detected.
        /// </summary>
        [JsonPropertyName("clipboard_detected")]
        public double ClipboardDetected { get; set; }

        /// <summary>
        /// Returns a value when a browser resize is detected.
        /// </summary>
        [JsonPropertyName("browser_resize_detected")]
        public double BrowserResizeDetected { get; set; }

        /// <summary>
        /// Returns a value when multiple faces are detected in the webcam feed.
        /// </summary>
        [JsonPropertyName("multiple_faces_detected")]
        public double MultipleFacesDetected { get; set; }

        /// <summary>
        /// Returns a value when the candidate didn’t interact with the keyboard and mouse for 20-30 seconds and whose face isn't clearly visible in the webcam feed.
        /// </summary>
        [JsonPropertyName("leaving_exam_area_detected")]
        public double LeavingExamAreaDetected { get; set; }

        /// <summary>
        /// Returns a value when the candidate is speaking during the exam.
        /// </summary>
        [JsonPropertyName("speaking_detected")]
        public double SpeakingDetected { get; set; }

        /// <summary>
        /// Returns a value when a candidate attempts to use AI during the exam.
        /// </summary>
        [JsonPropertyName("ai_detected")]
        public double AiDetected { get; set; }

        /// <summary>
        /// Returns a value when a candidate attempted/printed during the exam.
        /// </summary>
        [JsonPropertyName("printing_detected")]
        public double PrintingDetected { get; set; }

        /// <summary>
        /// Returns a value when a screenshot command is detected.
        /// </summary>
        [JsonPropertyName("screenshot_detected")]
        public double ScreenshotDetected { get; set; }

        /// <summary>
        ///  Returns a value when a candidate changed the hardware during the exam.
        /// </summary>
        [JsonPropertyName("hardware_change_detected")]
        public double HardwareChangeDetected { get; set; }

        /// <summary>
        /// Returns a value when external action is detected.
        /// </summary>
        [JsonPropertyName("external_action_detected")]
        public double ExternalActionDetected { get; set; }

        /// <summary>
        /// Returns a value when the webcam feed is obscured. 
        /// </summary>
        [JsonPropertyName("webcam_obscured_detected")]
        public double WebcamObscuredDetected { get; set; }

        /// <summary>
        /// Returns a value when a mobile device is detected in the webcam feed.
        /// </summary>
        [JsonPropertyName("mobile_phone_detected")]
        public double MobilePhoneDetected { get; set; }
    }
}