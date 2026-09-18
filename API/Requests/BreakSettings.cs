using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests;

/// <summary>
/// Candidate is allowed to take a break during the exam.
/// </summary>
public class BreakSettings
{
    /// <summary>
    /// Defines the number of breaks allowed.
    /// </summary>
    [JsonPropertyName("number")]
    [Range(1, 10, ErrorMessage = "Number of breaks allowed can't be less than 1 and more than 10.")]
    public int Number { get; set; }

    /// <summary>
    /// Defines the duration of the break.
    /// </summary>
    [JsonPropertyName("duration")]
    [Range(1, 30, ErrorMessage = "Duration of break(s) allowed can't be less than 1 and more than 30.")]
    public int Duration { get; set; }

    /// <summary>
    /// Indicates if the duration of the break is shared between breaks (true) or defined per break (false).
    /// </summary>
    [JsonPropertyName("cumulative")]
    public bool? Cumulative { get; set; }

    /// <summary>
    /// Action to take if the break time is exceeded.
    /// </summary>
    /// <remarks>
    /// 0 - Break time can be exceeded. 1 - If time of the break is exceeded Candidate will be removed from the exam
    /// </remarks>
    [JsonPropertyName("on_time_exceeded")]
    public OnTimeExceededRequest? OnTimeExceeded { get; set; }

    /// <summary>
    /// Indicates if a desk scan is required after the break.
    /// </summary>
    [JsonPropertyName("desk_scan_after_break")]
    public bool? DeskScanAfterBreak { get; set; }
}

/// <summary>
/// Action to take if the break time is exceeded.
/// </summary>
public enum OnTimeExceededRequest
{
    /// <summary>
    /// Attempt will be marked in the Review Center specifying that the alloted break time has been exceeded.
    /// </summary>
    MarkAttemptOnly = 0,

    /// <summary>
    /// Candidate will be kicked out of the exam upon exceeding the alloted break time
    /// </summary>
    KickOut
}