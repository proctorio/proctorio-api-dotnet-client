using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests;

public class RedirectSettings
{
    /// <summary>
    /// After graceful/successful submission of the exam Candidate will be redirected to graceful_submission_url.
    /// </summary>
    [JsonPropertyName("graceful_submission_url")]
    [StringLength(600, MinimumLength = 1, ErrorMessage = "When used, the graceful_submission_url value cannot be empty or exceed 600 characters.")]
    public string? GracefulSubmissionUrl { get; set; }
    /// <summary>
    /// After ungraceful exam end, Candidate will be redirected to ungraceful_submission_url when they click the "Okay" button within the "Attempt End" message.
    /// </summary>
    [JsonPropertyName("ungraceful_submission_url")]
    [StringLength(600, MinimumLength = 1, ErrorMessage = "When used, the ungraceful_submission_url value cannot be empty or exceed 600 characters.")]
    public string? UngracefulSubmissionUrl { get; set; }
}