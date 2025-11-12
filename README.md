# Proctorio API .NET Client

A .NET client library for integrating with the Proctorio API. This library provides a simple and type-safe way to generate launch URLs for Proctorio's exam proctoring services.

## Features

- **Type-safe API client** with full IntelliSense support
- **Support for all launch types:**
  - Candidate launch URLs (for test-takers)
  - Reviewer launch URLs (for reviewing exam recordings)
  - Live launch URLs (for live proctoring)
- **Comprehensive exam settings** for lockdown, recording, and verification
- **Behavior settings** for configuring exam analytics and suspicion detection
- **Webhook support** for V2 and V3 webhook requests

## Requirements

- .NET 8.0 or higher

## Installation

```bash
dotnet add package Proctorio.Client
```

Or add it manually to your `.csproj` file:

```xml
<PackageReference Include="Proctorio.Client" Version="2.0.0" />
```

## Getting Started

### 1. Initialize the Client

```csharp
using Proctorio.Client.API;
using Proctorio.Client.API.Requests;

var client = new ProctorioAPIClient(
    baseUrl: "https://[region][endpoint].proctorio.com",
    consumerKey: "your-consumer-key",
    consumerSecret: "your-consumer-secret"
);
```

### 2. Configure Exam Settings

```csharp
var examSettings = new ExamSettings
{
    RecordVideo = true,
    RecordAudio = true,
    RecordScreen = true,
    RecordWebTraffic = true,
    VerifyVideo = true,
    DisableClipboard = true,
    DisableRightClick = true,
    DisablePrinting = true
};
```

### 3. Generate Launch URLs

#### Candidate Launch URL

Generate a URL for test-takers to start their proctored exam:

```csharp
var candidateRequest = new CandidateLaunchRequest(
    userId: "student123",
    launchUrl: "https://your-lms.com/exam/start",
    examStart: "https://your-lms\\.com/exam/start.*",
    examTake: "https://your-lms\\.com/exam/question.*",
    examEnd: "https://your-lms\\.com/exam/submit",
    examSettings: examSettings
)
{
    Expire = 3600 // URL expires in 1 hour
    ExamTag= "exam1"
};

string candidateUrl = await client.GenerateCandidateUrl(candidateRequest);
// Redirect the student to this URL
```

#### Reviewer Launch URL

Generate a URL for instructors to review exam recordings:

```csharp
var reviewerRequest = new ReviewerLaunchRequest(
    userId: "instructor456",
    examSettings: examSettings
)
{
    ExamTag= "exam1",
    Expire = 3600,
    BehaviorSettings = new BehaviorSettings
    {
        FrameMetrics = new FrameMetricsRequest
        {
            NavigatingAway = 5,
            MultiplePersons = 4,
            NoFace = 3
        },
        ExamMetrics = new ExamMetricsRequest
        {
            LookAway = 3,
            HeadMovement = 2
        }
    }
};

string reviewerUrl = await client.GenerateReviewUrl(reviewerRequest);
// Redirect the reviewer to this URL
```

#### Live Launch URL

Generate a URL for live proctoring sessions:

```csharp
var liveRequest = new LiveLaunchRequest(userId: "proctor789")
{
    ExamTag= "exam1"
    Expire = 3600, // 1 hours
    BehaviorSettings = new BehaviorSettings
    {
        FrameMetrics = new FrameMetricsRequest
        {
            NavigatingAway = 5,
            MultiplePersons = 5
        }
    }
};

string liveUrl = await client.GenerateLiveUrl(liveRequest);
// Redirect the proctor to this URL
```

## Exam Settings Reference

The `ExamSettings` class controls lockdown, recording, and verification requirements:

### Recording Settings

| Property | Type | Description |
|----------|------|-------------|
| `RecordVideo` | `bool` | Record webcam video for the entire exam |
| `RecordAudio` | `bool` | Record microphone audio for the entire exam |
| `RecordScreen` | `bool` | Record the screen |
| `RecordWebTraffic` | `bool` | Capture screenshots and URLs of websites visited |
| `RecordDesk` | `RecordDeskSettingRequest` | Require desk scan (Off, Once, Periodically) |

### Verification Settings

| Property | Type | Description |
|----------|------|-------------|
| `VerifyVideo` | `bool` | Ensure webcam is working and not virtualized |
| `VerifyAudio` | `bool` | Ensure microphone is working and not muted |
| `VerifyDesktop` | `bool` | Ensure desktop recording is working |
| `VerifyId` | `VerifyIdSettingRequest` | Require photo ID verification (NotRequired, Auto, Human) |
| `VerifySignature` | `bool` | Require candidate to sign an agreement |

### Lockdown Settings

| Property | Type | Description |
|----------|------|-------------|
| `FullScreen` | `FullScreenSettingRequest` | Force fullscreen mode (Off, Lenient, Moderate, Severe) |
| `DisableClipboard` | `bool` | Disable copy/paste functionality |
| `DisableRightClick` | `bool` | Disable right-click context menu |
| `DisablePrinting` | `bool` | Disable printing |
| `DisableDownloads` | `bool` | Prevent file downloads |
| `DisableCache` | `bool` | Disable browser cache |
| `DisableNewWindows` | `bool` | Prevent opening new windows/tabs |

## Behavior Settings Reference

The `BehaviorSettings` class configures exam analytics and suspicion detection:

### Frame Metrics

Controls the weight (0-5) of behaviors detected in each video frame:

- `NavigatingAway` - Leaving the exam page
- `Keystrokes` - Typing within the exam
- `CopyPaste` - Copy/cut/paste actions
- `BrowserResize` - Changing browser size
- `AudioLevels` - Changes in audio levels 
- `HeadMovement` - Candidates moves their head away from the exam window
- `MultipleFaces` - More than one face detected
- `LeavingRoom` - No interaction with the keyboard and mouse for 20-30 seconds and whose face isn't clearly visible in the video feed


### Exam Metrics

Enables/disables exam metrics of overall exam behaviors:

- `NavigatingAway` - Abnormal amounts of navigating away
- `Keystrokes` - Typing within the exam
- `CopyPaste` - Copy/cut/paste actions
- `BrowserResize` - Abnormal changing browser size
- `AudioLevels` - Abnormal changes in audio levels 
- `HeadMovement` - Abnormal amount of head movement
- `MultipleFaces` - More than one face detected
- `MouseMovement` - Abnormal amount of mouse movement 
- `Scrolling` - Abnormal amount of scrolling 
- `Clicking` - Abnormal amount of clicking 
- `ExamDuration` -  Abnormal exam duration 

**Note:** Setting set to `false` disables the exam metric.

## Custom JSON Serialization

You can provide custom JSON serialization options:

```csharp
var jsonOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

var client = new ProctorioAPIClient(
    baseUrl: "https://[region][endpoint].proctorio.com",
    consumerKey: "your-consumer-key",
    consumerSecret: "your-consumer-secret",
    options: jsonOptions
);
```

## Error Handling

The client throws exceptions for various error conditions:

```csharp
try
{
    string url = await client.GenerateCandidateUrl(candidateRequest);
}
catch (ArgumentNullException ex)
{
    // Missing required parameters
    Console.WriteLine($"Invalid argument: {ex.Message}");
}
catch (ArgumentException ex)
{
    // Validation failed
    Console.WriteLine($"Validation error: {ex.Message}");
}
catch (HttpRequestException ex)
{
    // API request failed
    Console.WriteLine($"API error: {ex.Message}");
    Console.WriteLine($"Status code: {ex.StatusCode}");
}
```

## Webhooks

The library also includes webhook request models for handling Proctorio webhooks:

```csharp
using Proctorio.Client.Webhooks.Requests.V3;

// Handle incoming webhook
public IActionResult HandleWebhook([FromBody] WebhookRequest webhook)
{
    // Process webhook data
    var examId = webhook.ExamId;
    var userId = webhook.UserId;
    // ... process webhook
    
    return Ok();
}
```

## Regular Expression Patterns

When configuring candidate launch requests, use proper regex patterns:

```csharp
// Match exam start page and redirects
exam_start: @"https://your-lms\.com/(exam/start|login\?redirect=exam).*"

// Match all exam question pages
exam_take: @"https://your-lms\.com/exam/(question|quiz)/\d+.*"

// Match exam submission page
exam_end: @"https://your-lms\.com/exam/(submit|complete)"
```

**Important:** The `launchUrl` value should be included in the `exam_start` regex pattern.

## Best Practices

1. **Secure your credentials:** Never hardcode API keys in your source code. Use environment variables or secure configuration management.

2. **Validate settings:** The library automatically validates request parameters. Handle validation exceptions appropriately.

3. **Set appropriate expiration times:** Candidate URLs should have sufficient time for the exam duration, while reviewer URLs can have shorter expiration times.

4. **Configure behavior settings thoughtfully:** Adjust behavior weights based on exam type.

5. **Test regex patterns:** Thoroughly test your URL patterns to ensure they correctly match all exam pages.

## License

This project is licensed under the Apache License 2.0. See the [LICENSE](LICENSE) file for details.

## Support

For API documentation and support, visit [Proctorio Developer Portal](https://proctorio.com/developers).

## Repository

- GitHub: [https://github.com/proctorio/proctorio-api-dotnet-client](https://github.com/proctorio/proctorio-api-dotnet-client)

---

**Copyright © 2024 Proctorio Inc**
