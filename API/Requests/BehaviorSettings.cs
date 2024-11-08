using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests;

/// <summary>
/// Behavior settings determines the suspicion of each recorded action and configure exam analytics.Behavior settings should reflect the type of exam given(e.g., allowing head movement on an open-note exam) to achieve the desired results in the Proctorio Review Center.
/// </summary>
public class BehaviorSettings
{

    [JsonPropertyName("frame_metrics")]
    public FrameMetricsRequest FrameMetrics { get; set; } = new FrameMetricsRequest();

    [JsonPropertyName("exam_metrics")]
    public ExamMetricsRequest ExamMetrics { get; set; } = new ExamMetricsRequest();
}

/// <summary>
/// The severity of each metric sets the weight of a suspicious behavior in relation to the other behaviors. These metrics are calculated with each image. Each characteristic is derived for every image, regardless if the setting is enabled or not.
/// The severity of these metrics is set when generating the Reviewer URL, but that can be changed at any time by the Reviewer in the Proctorio Review Center, which will result in a re-calculation of the suscpicion level.
/// The weight can be set from 0 to 5. If set to 0, then this is considered "off," and the behaviors will not count towards the suspicion level or be shown in the Incident Log.
/// </summary>
public class FrameMetricsRequest
{

    /// <summary>
    /// Depending on the weight of this frame metric, Proctorio will flag when the candidate leaves the exam page. Range (0,5).
    /// </summary>
    [JsonPropertyName("navigating_away")]
    public uint NavigatingAway { get; set; } = 1;

    /// <summary>
    /// Depending on the weight of this frame metric, Proctorio will flag anytime the candidate types within the exam window. Range (0,5).
    /// </summary>
    [JsonPropertyName("keystrokes")]
    public uint Keystrokes { get; set; }

    /// <summary>
    /// Depending on the weight of this frame metric, Proctorio will flag when the candidate copies, cuts, or pastes within the exam window. Range (0,5).
    /// </summary>
    [JsonPropertyName("copy_paste")]
    public uint CopyPaste { get; set; }

    /// <summary>
    /// Depending on the weight of this frame metric, Proctorio will flag when the candidate changes the browser size while taking an exam. Range (0,5).
    /// </summary>
    [JsonPropertyName("browser_resize")]
    public uint BrowserResize { get; set; }

    /// <summary>
    /// Depending on the weight of this frame metric, Proctorio will flag when the ambient noise reaches a level above the white noise of the exam environment. Range (0,5).
    /// </summary>
    [JsonPropertyName("audio_levels")]
    public uint AudioLevels { get; set; }

    /// <summary>
    /// Depending on the weight of this frame metric, Proctorio will flag when the candidate moves their head away from the exam window. Range (0,5).
    /// </summary>
    [JsonPropertyName("head_movement")]
    public uint HeadMovement { get; set; }

    /// <summary>
    /// Depending on the weight of this frame metric, Proctorio will flag when multiple people look at an exam. Range (0,5).
    /// </summary>
    [JsonPropertyName("multiple_faces")]
    public uint MultipleFaces { get; set; }

    /// <summary>
    /// Depending on the weight of this frame metric, Proctorio will flag candidates who didn't interact with the keyboard and mouse for 20-30 seconds and whose face isn't clearly visible in the video feed. Range (0,5).
    /// </summary>
    [JsonPropertyName("leaving_room")]
    public uint LeavingRoom { get; set; }
}

/// <summary>
/// Exam Metrics are calculated based on particular behavior in comparison to the rest of the group and are factored into the suspicion level.
/// Candidates with significantly different behaviors than the rest of the group will be highlighted.
/// </summary>
public class ExamMetricsRequest
{
    /// <summary>
    /// Abnormal amounts of navigating away will highlight the candidates who are using external applications or materials differently than the rest of the group.
    /// </summary>
    [JsonPropertyName("navigating_away")]
    public bool NavigatingAway { get; set; } = true;

    /// <summary>
    /// Abnormal amount of keystrokes will highlight the candidates who are relying on copy and paste or struggling with free response questions.
    /// </summary>
    [JsonPropertyName("keystrokes")]
    public bool Keystrokes { get; set; }

    /// <summary>
    /// Abnormal amount of copy/paste activity will highlight the candidates who may have taken material from the exam or brought answers into the exam repeatedly.
    /// </summary>
    [JsonPropertyName("copy_paste")]
    public bool CopyPaste { get; set; }

    /// <summary>
    /// Abnormal amount of browser resize will highlight the candidates who may have had notes or other material hidden behind the exam window.
    /// </summary>
    [JsonPropertyName("browser_resize")]
    public bool BrowserResize { get; set; }

    /// <summary>
    /// Abnormal changes in audio levels will highlight the candidates that had significant changes in audio activity throughout the exam.
    /// </summary>
    [JsonPropertyName("audio_levels")]
    public bool AudioLevels { get; set; }

    /// <summary>
    /// Abnormal amount of head movement will highlight the candidates who looked away from the camera significantly more or significantly less than the rest of the group.
    /// </summary>
    [JsonPropertyName("head_movement")]
    public bool HeadMovement { get; set; }

    /// <summary>
    /// Abnormal number of detected faces will highlight the candidates who may have received help from someone during the exam.
    /// </summary>
    [JsonPropertyName("multiple_faces")]
    public bool MultipleFaces { get; set; }

    /// <summary>
    /// Abnormal amount of mouse movement will highlight the candidates who interacted with the exam page less than the rest of the group.
    /// </summary>
    [JsonPropertyName("mouse_movement")]
    public bool MouseMovement { get; set; }

    /// <summary>
    /// Abnormal amount of scrolling will highlight the candidates who interacted with the exam page less than the rest of the group.
    /// </summary>
    [JsonPropertyName("scrolling")]
    public bool Scrolling { get; set; }

    /// <summary>
    /// Abnormal amount of clicking will highlight the candidates who interacted with the exam page less than the rest of the group.
    /// </summary>
    [JsonPropertyName("clicking")]
    public bool Clicking { get; set; }

    /// <summary>
    /// Abnormal exam duration will highlight the candidates who have finished significantly faster or significantly slower than the rest of the group.
    /// </summary>
    [JsonPropertyName("exam_duration")]
    public bool ExamDuration { get; set; }

    /// <summary>
    /// Start Times will highlight the candidates whose LMS start time does not match the Proctorio start time.
    /// </summary>
    [JsonPropertyName("start_time")]
    public bool StartTime { get; set; }

    /// <summary>
    /// End Times will highlight the candidates whose LMS end time does not match the Proctorio end time.
    /// </summary>
    [JsonPropertyName("end_time")]
    public bool EndTime { get; set; }

    /// <summary>
    /// Exam Collusion will highlight the candidates who took the exam at the same time on the same network.
    /// </summary>
    [JsonPropertyName("exam_collusion")]
    public bool ExamCollusion { get; set; }
}