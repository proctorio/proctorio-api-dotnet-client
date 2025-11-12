using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests;

/// <summary>
/// The exam settings control the lockdown, recording, and verification requirements for the exam. 
/// <summary>
/// <remarks>
/// Every exam is different; some may allow the Candidates to use other websites or applications, while others will prevent these functions.
/// </remarks>
public class ExamSettings 
{
    /// <summary>
    /// Requires the Candidate to have a webcam and will record the video for the entire duration of the exam.
    /// </summary>
    [JsonPropertyName("record_video")]
    public bool RecordVideo { get; set; }

    /// <summary>
    /// Requires the Candidate to have a microphone and will record audio for the entire exam duration of the exam.
    /// </summary>
    [JsonPropertyName("record_audio")]
    public bool RecordAudio { get; set; }

    /// <summary>
    /// Will capture and record the full desktop screen for the entire duration of the exam.
    /// </summary>
    [JsonPropertyName("record_screen")]
    public bool RecordScreen { get; set; }

    /// <summary>
    /// Will capture screenshots and URLs of any websites visited during the exam.
    /// </summary>
    [JsonPropertyName("record_web_traffic")]
    public bool RecordWebTraffic { get; set; }

    /// <summary>
    /// Requires the Candidate to perform a desk scan at the start of the exam or at the start of the exam and at random intervals during the exam, depending on subsetting.
    /// </summary>
    /// <remarks>
    /// Requires the following settings to be set to true: RecordVideo, RecordAudio
    /// </remarks>
    [JsonPropertyName("record_desk")]
    public RecordDeskSettingRequest RecordDesk { get; set; } = RecordDeskSettingRequest.Off;

    /// <summary>
    /// Will ensure that the webcam is working and it is not virtualized or broken. 
    /// </summary>  
    /// <remarks>
    /// Requires the following settings to be set to true: RecordVideo.
    /// </remarks>
    [JsonPropertyName("verify_video")]
    public bool VerifyVideo { get; set; }

    /// <summary>
    /// Will ensure that the microphone is working and it is not virtualized or muted. 
    /// </summary>
    /// <remarks>
    /// Requires the following settings to be set to true: RecordAudio.
    /// </remarks>
    [JsonPropertyName("verify_audio")]
    public bool VerifyAudio { get; set; }

    /// <summary>
    /// Will ensure that the desktop recording is working and is being properly recorded.
    /// </summary>
    /// <remarks>
    /// Requires the following settings to be set to true: RecordScreen.
    /// </remarks>
    [JsonPropertyName("verify_desktop")]
    public bool VerifyDesktop { get; set; }

    /// <summary>
    /// Requires the Candidate to present photo identification prior to starting the exam which will be automatically scanned or reviewed and verified by a Proctorio agent before they are allowed into the exam, depending on subsetting
    /// </summary>
    /// <remarks>
    /// Requires the following settings to be set to true: RecordVideo, VerifyVideo.
    /// </remarks>
    [JsonPropertyName("verify_id")]
    public VerifyIdSettingRequest VerifyId { get; set; } = VerifyIdSettingRequest.NotRequired;

    /// <summary>
    /// Requires the Candidate to sign an agreement before exam start.
    /// </summary>
    [JsonPropertyName("verify_signature")]
    public bool VerifySignature { get; set; }

    /// <summary>
    /// Forces the exam in fullscreen, preventing access to other applications and websites. Navigating away from the exam page for more than 15/30 seconds results in removal from the exam, or in instant removal, depending on subsetting
    /// </summary>
    [JsonPropertyName("full_screen")]
    public FullScreenSettingRequest FullScreen { get; set; } = FullScreenSettingRequest.Off;

    /// <summary>
    /// Disables copy/paste functionality.
    /// </summary>
    [JsonPropertyName("disable_clipboard")]
    public bool DisableClipboard { get; set; }

    /// <summary>
    /// Disables new tabs or windows during the exam/disables tabs or windows during the exam except links embedded in the exam page depending on subsetting.
    /// </summary>
    [JsonPropertyName("tabs")]
    public TabsSettingRequest Tabs { get; set; } = TabsSettingRequest.Allowed;

    /// <summary>
    /// Forces all other tabs and windows to be closed before the exam starts.
    /// </summary>
    [JsonPropertyName("close_tabs")]
    public bool CloseTabs { get; set; }

    /// <summary>
    /// Forces the Candidate to disable all but one monitor before starting the exam and prevents them from connecting additional monitors during the exam.
    /// </summary>
    [JsonPropertyName("one_screen")]
    public bool OneScreen { get; set; }

    /// <summary>
    /// Disables printing exam content to prevent exam distribution.
    /// </summary>
    [JsonPropertyName("disable_printing")]
    public bool DisablePrinting { get; set; }

    /// <summary>
    /// Prevents the Candidate from downloading files through the browser.
    /// </summary>
    [JsonPropertyName("block_downloads")]
    public bool BlockDownloads { get; set; }

    /// <summary>
    /// Empties system temporary files after the exam is submitted.
    /// </summary>
    [JsonPropertyName("clear_cache")]
    public bool ClearCache { get; set; }

    /// <summary>
    /// Disables right click functionalities.
    /// </summary>
    [JsonPropertyName("disable_right_click")]
    public bool DisableRightClick { get; set; }

    /// <summary>
    /// Provides the Candidate with an on-screen calculator with basic/scientific/graphing funcitons, depending on subsetting
    /// </summary>
    [JsonPropertyName("calculator")]
    public CalculatorSettingRequest Calculator { get; set; } = CalculatorSettingRequest.Off;

    /// <summary>
    /// Provides the Candidate with a scratch pad and drawing tools on-screen.
    /// </summary>
    [JsonPropertyName("whiteboard")]
    public bool Whiteboard { get; set; }

    /// <summary>
    /// Will prevent the Candidate from using other browser extensions during the exam except for allowlisted ones.
    /// </summary>
    [JsonPropertyName("disable_extensions")]
    public bool DisableExtensions { get; set; }
    /// <summary>
    /// This setting will allow a Proctor to monitor the Candidate during the exam and allow immediate intervention. Also, the full recording and report will be available via the Proctorio Review Center. Requires the following settings to be set to true: record_video, verify_video.
    /// </summary>
    [JsonPropertyName("live_proctor")]
    public bool LiveProctor { get; set; }
    /// <summary>
    /// Allows exam to be taken on mobile device, using MobileExam application. Requires use of pre_auth in launch request. Not compatible with verify_id: 2 setting
    /// </summary>
    [JsonPropertyName("mobile")]
    public bool Mobile { get; set; }
    /// <summary>
    /// Prevents use of other applications during exam. Requires use of the Secure Companion application.
    /// </summary>
    [JsonPropertyName("advanced_program_detection")]
    public bool AdvancedProgramDetection { get; set; }
    /// <summary>
    /// Logs hardware changes during exam in the Review Center. Requires use of the Secure Companion application.
    /// </summary>
    [JsonPropertyName("advanced_hardware_detection")]
    public bool AdvancedHardwareDetection { get; set; }
    /// <summary>
    /// Prevents use of virtual machine to take exam. Requires use of the Secure Companion application.
    /// </summary>
    [JsonPropertyName("advanced_vm_detection")]
    public bool AdvancedVmDetection { get; set; }
    /// <summary>
    /// List maximum of 5 allowed applications that can remain open when advanced_program_detection is used. Applications are identified by bundle ID on MacOS.
    /// </summary>
    [JsonPropertyName("allowed_macos_apps")]
    public List<string> AllowedMacosApps { get; set; } = new List<string>();
    /// <summary>
    /// List maximum of 5 allowed applications that can remain open when advanced_program_detection is used. Applications are identified by binary_name, product_name and company_name on Windows.
    /// </summary>
    [JsonPropertyName("allowed_windows_apps")]
    public List<WindowsAllowedApp> AllowedWindowsApps { get; set; } = new List<WindowsAllowedApp>();
    /// <summary>
    /// Sets the strictness level for validating allowed applications when advanced_program_detection, allowed_macos_apps and allowed_windows_apps are used.
    /// </summary>
    [JsonPropertyName("strict_apps_validation")]
    public StrictAppsValidationLevel StrictAppsValidation { get; set; } = StrictAppsValidationLevel.Default;
}

public enum VerifyIdSettingRequest
{
    /// <summary>
    /// Setting is turned off.
    /// </summary>
    NotRequired = 0,
    /// <summary>
    /// Requires the Candidate to present photo identification prior to starting the exam which will be automatically scanned.
    /// </summary>
    Auto,
    /// <summary>
    /// Requires the Candidate to present photo identification prior to starting the exam. The ID is then reviewed and verified by a Proctorio agent before they are allowed into the exam
    /// </summary>
    Live
}

public enum FullScreenSettingRequest
{
    /// <summary>
    /// Setting is turned off.
    /// </summary>
    Off = 0,
    /// <summary>
    /// Forces the exam in fullscreen, preventing access to other applications and websites. Navigating away from the exam page for more than 15 seconds (cumulatively) results in removal from the exam
    /// </summary>
    Moderate,
    /// <summary>
    /// Forces the exam in fullscreen, preventing access to other applications and websites. Navigating away from the exam page for more than 30 seconds (cumulatively) results in removal from the exam
    /// </summary>
    Lenient,
    /// <summary>
    /// Forces the exam in fullscreen, preventing access to other applications and websites. Navigating away from the exam page results in instant removal from the exam
    /// </summary>
    Severe
}
public enum TabsSettingRequest
{
    /// <summary>
    /// Setting is turned off.
    /// </summary>
    Allowed = 0,
    /// <summary>
    /// Disables new tabs or windows during the exam
    /// </summary>
    NoTabs,
    /// <summary>
    /// Disables new tabs or windows during the exam except links embedded in the exam page.
    /// </summary>
    LinksOnly
}

public enum RecordDeskSettingRequest
{
    /// <summary>
    /// Setting is turned off.
    /// </summary>
    Off = 0,
    /// <summary>
    /// Requires the Candidate to perform a desk scan at the start of the exam and at random intervals during the exam.
    /// </summary>
    RecordDeskPeriodic,
    /// <summary>
    /// Requires the Candidate to perform a desk scan at the start of the exam.
    /// </summary>
    RecordDesk
}

public enum CalculatorSettingRequest
{
    /// <summary>
    /// Setting is turned off.
    /// </summary>
    Off = 0,
    /// <summary>
    /// Provides the Candidate with an on-screen calculator with basic functions.
    /// </summary>
    Basic,
    /// <summary>
    /// Provides the Candidate with an on-screen calculator with scientific functions.
    /// </summary>
    Scientific,
    /// <summary>
    /// Provides the Candidate with an on-screen calculator with graphing functions.
    /// </summary>
    Graphing
}

public class WindowsAllowedApp
{
    /// <summary>
    /// The binary name of the application.
    /// </summary>
    [JsonPropertyName("binary_name")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "The binary_name value cannot be empty or exceed 50 characters.")]
    public string? BinaryName { get; set; }
    /// <summary>
    /// The product name of the application.
    /// </summary>
    [JsonPropertyName("product_name")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "The product_name value cannot be empty or exceed 50 characters.")]
    public string? ProductName { get; set; }
    /// <summary>
    /// The company name of the application.
    /// </summary>
    [JsonPropertyName("company_name")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "The company_name value cannot be empty or exceed 50 characters.")]
    public string? CompanyName { get; set; }
}

public enum StrictAppsValidationLevel
{
    Off,
    Default,
}