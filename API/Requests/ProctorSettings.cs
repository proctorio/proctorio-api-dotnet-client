using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests;

/// <summary>
/// Settings related to live proctoring options such as allowing breaks, desk scans, interruptions, and kick-outs.
/// </summary>
public class ProctorSettings
{
    /// <summary>
    /// This option allows a proctor to give a candidate a break by selecting "Give break" in the options menu.
    /// This option will only be available when a candidate has been claimed by proctor.
    /// </summary>
    [JsonPropertyName("break_allowed")]
    public bool BreakAllowed { get; set; }

    /// <summary>
    /// This option allows a proctor to initiate a desk scan by selecting "Initiate Desk Scan" in the options menu.
    /// Initiate a desk scan will prompt the candidate to scan their desk or environment during their exam.
    /// This option will only be available when a candidate has been claimed by a proctor.
    /// /// </summary>
    [JsonPropertyName("desk_scan_allowed")]
    public bool DeskScanAllowed { get; set; }

    /// <summary>
    /// This option allows a proctor to interrupt/resume a candidate's attempt.
    /// This option will only be available when a candidate has been claimed by proctor.
    /// </summary>
    [JsonPropertyName("interrupt_allowed")]
    public bool InterruptAllowed { get; set; }

    /// <summary>
    /// This option allows a proctor to kick-out a candidate from their attempt.
    /// This option will only be available when a candidate has been claimed by proctor.
    /// </summary>
    [JsonPropertyName("kickout_allowed")]
    public bool KickoutAllowed { get; set; }
}