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
            string exam_start,
            string exam_take,
            string exam_end,
            ExamSettings examSettings
           ) : base(userId)
        {
            LaunchUrl = launchUrl;
            ExamStart = exam_start;
            ExamTake = exam_take;
            ExamEnd = exam_end;
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
        /// The RedirectUrl parameter is optional. If provided it will allow the Candidate to be redirected to that URL, by clicking the "Click here" hyperlink in case they refreshed the page or clicked the back button during the exam.
        /// </summary>
        [JsonPropertyName("redirect_url")]
        [StringLength(600, MinimumLength = 1, ErrorMessage = "The redirect_url value cannot be empty or exceed 600 characters.")]
        public string? RedirectUrl { get; set; }
    }
}
