using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests
{
    /// <summary>
    /// Candidate launch request.
    /// </summary>
    public class CandidateLaunchRequest : LaunchRequest
    {
        public CandidateLaunchRequest(string userId, 
            string launchUrl,
            string examStart,
            string examTake,
            string examEnd,
            ExamSettings examSettings
           ) : base(userId)
        {
            LaunchUrl = launchUrl;
            ExamStart = examStart;
            ExamTake = examTake;
            ExamEnd = examEnd;
            ExamSettings = examSettings;

            var validationResult = Helpers.Validate(this);
            if (!validationResult.IsValid)
                throw new ArgumentException(JsonSerializer.Serialize(validationResult.ValidationResults));
        }

        /// <summary>
        /// Must contain a valid absolute URL, that fully launches to the exam start page with no additional authentication. The "LaunchUrl" value should be included in the "ExamStart" regex pattern, alongside any of the redirects.
        /// </summary>
        [JsonPropertyName("launch_url")]
        [Required]
        [StringLength(600, MinimumLength = 1, ErrorMessage = "The launch_url value cannot be empty or exceed 600 characters.")]
        public string LaunchUrl { get; set; }

        /// <summary>
        /// Regular expression to match the exam start page. Any pages before this will be considered pre-exam pages and will be ignored. This is the URL that the Candidate is on before they begin the exam. The "LaunchUrl" value should be included in the "ExamStart" regex pattern, alongside any of the redirects.
        /// </summary>
        [JsonPropertyName("exam_start")]
        [Required]
        [StringLength(600, MinimumLength = 1, ErrorMessage = "The exam_start value cannot be empty or exceed 600 characters.")]
        public string ExamStart { get; set; }

        /// <summary>
        /// Must be a regular expression to match the in-exam page URLs (the URL of the exam), and any redirects. In cases where there are questions on multiple pages, this is important. Anything else visited that does not match this or the exam_end parameter will be considered leaving the exam and the session will be considered complete.
        /// <summary>
        [JsonPropertyName("exam_take")]
        [Required]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "The exam_take value cannot be empty or exceed 1000 characters.")]
        public string ExamTake { get; set; }

        /// <summary>
        /// Must be a regular expression to match the exam end page (the URL the Candidate is taken to once the exam has been completed) and any possible redirect. This triggers the end of the proctoring session and considers that the exam has been submitted. Anything else visited that does not match this or the exam_take parameter will be considered leaving the exam and the proctoring session will end but the attempt won't be considered as gracefully submitted.
        /// </summary>
        [JsonPropertyName("exam_end")]
        [Required]
        [StringLength(600, MinimumLength = 1, ErrorMessage = "The exam_end value cannot be empty or exceed 600 characters.")]
        public string ExamEnd { get; set; }

        /// <summary>
        /// The exam settings control the lockdown, recording, and verification requirements for the exam.Every exam is different; some may allow the Candidates to use other websites or applications, while others will prevent these functions.
        /// </summary>
        [JsonPropertyName("exam_settings")]
        public ExamSettings ExamSettings { get; set; }

        /// <summary>
        /// Number of seconds before the Candidate URL is no longer valid. The default value for this parameter is 18000 seconds. If a value is not passed, the default value will be applied. Must be an integer value.
        /// </summary>
        [JsonPropertyName("expire")]
        [Range(1, 18000, ErrorMessage = "When used, the expire value can't be less than 1 and more than 18000 seconds.")]
        public int? Expire { get; set; } = 18000;

        /// <summary>
        /// [Deprecated] The RedirectUrl parameter is optional. If provided it will allow the Candidate to be redirected to that URL, by clicking the "Click here" hyperlink in case they refreshed the page or clicked the back button during the exam. Please check redirect_settings for the new way of handling redirects.
        /// </summary>
        [JsonPropertyName("redirect_url")]
        [StringLength(600, MinimumLength = 1, ErrorMessage = "The redirect_url value cannot be empty or exceed 600 characters.")]
        [Obsolete("This parameter is deprecated and will be removed in future versions.")]
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// "Url used to pre-authenticate Candidate on MobileExam application."
        /// </summary>
        [JsonPropertyName("pre_auth")]
        [StringLength(600, MinimumLength = 1, ErrorMessage = "When used, the pre_auth value cannot be empty or exceed 600 characters.")]
        public string? PreAuth { get; set; }

        /// <summary>
        /// The redirect_settings parameter is optional. If provided, the Candidate will be redirected to provided URLs depending on exam end.
        /// </summary>
        [JsonPropertyName("redirect_settings")]
        public RedirectSettings? RedirectSettings { get; set; }

        /// <summary>
        /// The unique identifier for a specific attempt of the Candidate. Must contain an alphanumeric (hyphens also acceptable) value. The "attempt_id" value should be reused when generating a new Candidate URL, for the Candidate that is resuming the same attempt on the learning platform.
        /// </summary>
        [JsonPropertyName("attempt_id")]
        [StringLength(36, MinimumLength = 1, ErrorMessage = "When used, the attempt_id value cannot be empty or exceed 36 characters.")]
        [RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "When used, the attempt_id must contain an alphanumeric (hyphens also acceptable) value.")]
        public string? AttemptId { get; set; }
        /// <summary>
        /// The "domain" parameter is optional. By utilizing "domain", the exam pages as well as the Reviewer Center will load with your desired domain. 
        /// <para/>
        /// The URL will no longer point to the https://getproctorio.com page. Instead, the Candidates/Reviewers will be directed to the new route you provided in the parameter, example: https://yourdomain.com. 
        /// <para/>
        /// This allows the utilization of additional cross-origin security mechanisms, which use the SameSite cookies or X-Frame-Options: SAMEORIGIN header. It will also provide the ability to prevent data loss in session or local storage related to storage partitioning browser functionality. 
        /// <para/>
        /// The https://getproctorio.com page has the following functionalities: Check if a Candidate/Reviewer has the supported browser installed. Check if a Candidate/Reviewer has the Proctorio extension installed. To keep these functionalities, a link to https://getproctorio.com with content describing the purpose of getproctorio.com is necessary on the institution's page.
        /// </summary>
        [JsonPropertyName("domain")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "When used, the domain value cannot be empty or exceed 100 characters.")]
        public string? Domain { get; set; }

        /// <summary>
        /// Only to be used when \"roster_url\" and \"user_id\" do not contain Candidates name. The \"display_name\" information is NOT saved, and will not be available in the Review Center for Reviewer or for Proctor. It is used only for the Live ID verification and Exam Agreement on the Candidate side.
        /// </summary>
        [JsonPropertyName("display_name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "When used, the display_name value cannot be empty or exceed 100 characters.")]
        public string? DisplayName { get; set; }
        
        /// <summary>
        /// Extension Allowlist endpoint URL. Http Method: GET. The response should be a JSON stringified array. For example: '[extensionID1,extensionID2]'. \r\n\r\nThe extension_allowlist_url can be used for Candidates that have force enabled extensions in the browser by institution. These extensions can't be disabled manually by Candidate. \r\n\r\nThe 'extensionID' value should correspond to ID of the extension that will be allowed to remain active during Proctored attempt.
        /// </summary>
        [JsonPropertyName("extension_allowlist_url")]
        [StringLength(600, MinimumLength = 1, ErrorMessage = "When used, the extension_allowlist_url value cannot be empty or exceed 600 characters.")]
        public string? ExtensionAllowlistUrl { get; set; }
    }
}
