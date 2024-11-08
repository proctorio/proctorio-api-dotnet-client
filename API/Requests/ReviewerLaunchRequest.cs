using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests
{
    /// <summary>
    /// Reviewer launch request.
    /// </summary>
    public class ReviewerLaunchRequest : LaunchRequest
    {
        public ReviewerLaunchRequest(string userId, ExamSettings examSettings) : base(userId)
        {
            ExamSettings = examSettings;
            var validationResult = Helpers.Validate(this);
            if (!validationResult.IsValid)
                throw new ArgumentException(JsonSerializer.Serialize(validationResult.ValidationResults));
        }

        /// <summary>
        /// Exam settings.
        /// </summary>
        [JsonPropertyName("exam_settings")]
        public ExamSettings ExamSettings { get; set; }

        /// <summary>
        /// Number of seconds before the Reviewer URL is no longer valid. The default value for this parameter is 3600 seconds. If a value is not passed, the default value will be applied. Must be an integer value.
        /// </summary>
        [JsonPropertyName("expire")]
        [Range(1, 3600, ErrorMessage = "When used, the expire value can't be less than 1 and more than 3600 seconds.")]
        public int? Expire { get; set; } = 3600;

        /// <summary>
        /// Exam name.
        /// </summary>
        [JsonPropertyName("exam_name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "When used, the exam_name value cannot be empty or exceed 100 characters.")]
        public string? ExamName { get; set; }

        /// <summary>
        /// Behavior settings determines the suspicion of each recorded action and configure exam analytics. Behavior settings should reflect the type of exam given (e.g., allowing head movement on an open-note exam) to achieve the desired results in the Proctorio Review Center.
        /// </summary>
        [JsonPropertyName("behavior_settings")]
        public BehaviorSettings? BehaviorSettings { get; set; }
    }
}
