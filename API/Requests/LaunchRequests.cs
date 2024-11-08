using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests;

/// <summary>
/// Abstract class for proctorio api. 
/// </summary>
public abstract class LaunchRequest
{
    protected LaunchRequest(string userId)
    {
        UserId = userId;
    }

    /// <summary>
    /// Must contain an alphanumeric (hyphens also acceptable) value, unique to this specific user.
    /// </summary>
    [JsonPropertyName("user_id")]
    [Required]
    [StringLength(36, MinimumLength = 1, ErrorMessage = "The user_id value cannot be empty or exceed 36 characters.")]
    [RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "The user_id must contain an alphanumeric (hyphens also acceptable) value.")]
    public string UserId { get; set; }

    /// <summary>
    /// This is the exam ID tag and will be added to the end of the URL. The eExamTag can't contain spacing and it can't contain NON-ASCII characters. 
    /// </summary>
    /// <remarks>
    /// If used, then Proctorio factors it into the response. If it is not used, a URL will be generated without the hash and needs to be added by the learning platform prior to usage. Proctorio recommends using the RoosterUrl when the ExamTag is not provided.
    /// </remarks>
    [JsonPropertyName("exam_tag")]
    public string? ExamTag { get; set; }

    /// <summary>
    /// The "domain" parameter is optional. By utilizing a custom domain, the exam pages as well as the Reviewer Center will loaded on your desired domain with your institution’s branding and design instead of getproctorio.com.
    /// </summary>
    [JsonPropertyName("domain")]
    public string? Domain { get; set; }

    /// <summary>
    /// Represents a course ID or a section ID value. This parameter is optional. It provides a more granular sorting of the exams.
    /// </summary>
    /// <remarks>
    /// Exams that have the same "exam_tag" but different "section_id", will be treated like different exams.
    /// The "SectionID" parameter is dependent upon the "RoosterUrl" parameter, and it can't be used without it.
    /// </remarks>
    [JsonPropertyName("section_id")]
    public string? SectionId { get; set; }

    /// <summary>
    /// Roster endpoint URL. Http Method: GET. The response should be a JSON stringified array. For example: '[[\"id1\",\"name1\"],[\"id2\",\"name2\"]]'\r\n\r\nThe \"id\" value should correspond to the \"userId\" parameter, be unique to the Candidate, and non-repeatable within the roster.\r\n\r\nThe roster endpoint is validated by Proctorio, so that means that if an invalid endpoint is provided or if \"userId\" doesn't match any \"id\" value inside the array, the attempts will be marked as \"Unmatched\" in the Review Center.\r\n\r\nThe roster_url is only fetched by the end user accessing the Review Center or the Exam Agreement page, client-side, not by Proctorio directly. That means that it can be secured with the session for that particular user. This is intentional, and as such there is no need for the PII to be passed to Proctorio at any point, whether that be a Candidate Launch request or a Reviewer Launch request.
    /// </summary>
    [JsonPropertyName("roster_url")]
    public string? RosterUrl { get; set; }
}




