# Proctorio API .NET Client

A .NET client library for integrating with the [Proctorio](https://proctorio.com) API. It provides typed request/response models and a client wrapper for generating Candidate, Reviewer, and Live launch URLs, as well as managing webhook subscriptions.

## Requirements

- .NET 8.0

## Installation

Add a reference to the `Proctorio.Client` project, or install the NuGet package (once published):

```bash
dotnet add package Proctorio.Client
```

## Getting started

Instantiate `ProctorioAPIClient` with your API base URL and the consumer key/secret provided by Proctorio. The client automatically derives the required `api_key` header from your credentials.

```csharp
using Proctorio.Client.API;
using Proctorio.Client.API.Requests;

var client = new ProctorioAPIClient(
    baseUrl: "https://{region}.proctorapi.com",
    consumerKey: "your-consumer-key",
    consumerSecret: "your-consumer-secret"
);
```

Unset optional parameters are omitted from the request payload rather than sent as `null`.

### Supplying your own HttpClient

By default all instances share a single internally managed `HttpClient`, so creating a client per request will not exhaust sockets. To integrate with dependency injection or `IHttpClientFactory`, pass your own client as the first argument; its lifetime stays yours to manage.

```csharp
builder.Services.AddHttpClient<ProctorioAPIClient>((httpClient, _) =>
    new ProctorioAPIClient(
        httpClient,
        baseUrl: "https://{region}.proctorapi.com",
        consumerKey: "your-consumer-key",
        consumerSecret: "your-consumer-secret"
    ));
```

### Cancellation

Every API method accepts an optional `CancellationToken`.

```csharp
string candidateUrl = await client.GenerateCandidateUrl(candidateRequest, cancellationToken);
```

## Usage

### Generate a Candidate launch URL

```csharp
var examSettings = new ExamSettings
{
    RecordVideo = true,
    RecordAudio = true,
    FullScreen = FullScreenSettingRequest.Moderate,
    Tabs = TabsSettingRequest.NoTabs
};

var candidateRequest = new CandidateLaunchRequest(
    userId: "user-123",
    launchUrl: "https://example.com/exam/start",
    examStart: "https://example.com/exam/start",
    examTake: "https://example.com/exam/take",
    examEnd: "https://example.com/exam/end",
    examSettings: examSettings
);

string candidateUrl = await client.GenerateCandidateUrl(candidateRequest);
```

#### Regular expression patterns

`ExamStart`, `ExamTake`, and `ExamEnd` are regular expressions, not exact URLs. The `LaunchUrl` value should be included in the `ExamStart` pattern, alongside any redirects.

```csharp
// Match exam start page and redirects
exam_start: @"https://your-lms\.com/(exam/start|login\?redirect=exam).*"
// Match all exam question pages
exam_take: @"https://your-lms\.com/exam/(question|quiz)/\d+.*"
// Match exam submission page
exam_end: @"https://your-lms\.com/exam/(submit|complete)"
```

### Generate a Reviewer launch URL

```csharp
var reviewerRequest = new ReviewerLaunchRequest(
    userId: "reviewer-123",
    examSettings: examSettings
)
{
    ExamName = "Midterm Exam"
};

string reviewerUrl = await client.GenerateReviewUrl(reviewerRequest);
```

### Generate a Live proctoring launch URL

```csharp
var liveRequest = new LiveLaunchRequest(userId: "user-123")
{
    ExamName = "Live Proctored Exam",
    ProctorSettings = new ProctorSettings
    {
        BreakAllowed = true,
        InterruptAllowed = true,
        KickoutAllowed = true
    }
};

string liveUrl = await client.GenerateLiveUrl(liveRequest);
```

### Manage webhook subscriptions

```csharp
// List active subscriptions
List<SubscribeResponse> subscriptions = await client.ListWebhooks();

// List available webhook types
List<string> availableTypes = await client.ListAvailableWebhookTypes();

// Subscribe to the suspicion calculation webhook
var subscribeRequest = new SuspicionCalculationSubscribeRequest(
    client: new ClientApiInfo(url: "https://your-endpoint.com/webhook")
);
SubscribeResponse subscription = await client.SubscribeToSuspicionCalculationWebhook(subscribeRequest);

// Unsubscribe
string result = await client.UnsubscribeWebhook(new UnsubscribeRequest(id: subscription.Id!));
```

## Request models

| Model | Description |
| --- | --- |
| `CandidateLaunchRequest` | Generates a launch URL for a Candidate taking an exam. |
| `ReviewerLaunchRequest` | Generates a launch URL for a Reviewer accessing the Review Center. |
| `LiveLaunchRequest` | Generates a launch URL for Live proctoring sessions. |
| `ExamSettings` | Lockdown, recording, and verification settings applied to an exam. |
| `BehaviorSettings` | Frame and exam metrics used to calculate suspicion levels. |
| `ProctorSettings` | Controls available to a live Proctor (breaks, desk scans, interrupts, kick-outs). |
| `RedirectSettings` | URLs to redirect Candidates to after graceful/ungraceful submission. |
| `BreakSettings` | Configures Candidate break allowance during an exam. |
| `Branding` | Custom colors and logo for the Candidate/Reviewer UI. |
| `AllowedWindowsApp` | Identifies an allowlisted Windows application by binary/product/company name. |
| `ClientApiInfo` | Client endpoint URL and key used for webhook subscriptions. |
| `SuspicionCalculationSubscribeRequest` | Subscribes to the suspicion calculation webhook. |
| `UnsubscribeRequest` | Terminates a webhook subscription. |

All request models validate themselves against their `DataAnnotations` attributes on construction and throw an `ArgumentException` (containing the validation errors) if invalid.

## Webhooks

Incoming webhook payloads can be deserialized using the models in `Proctorio.Client.Webhooks.Requests.V2` and `Proctorio.Client.Webhooks.Requests.V3`, matching the subscribed webhook version. Each request includes a `nonce` and `signature` (or `api_key`) that should be validated before trusting the payload.

### Validating the signature

The signature is `sha1(nonce + ":" + data + ":" + secret)`, where `data` is the JSON text of the request's `data` object and `secret` is the value Proctorio shared with you. `WebhookSignature` computes and compares it for you, in constant time.

Validate against the **raw request body**. The hash covers the bytes as they were transmitted, so re-serializing a deserialized model will not reproduce the original text and the signature will not match.

```csharp
using Proctorio.Client.Webhooks;
using Proctorio.Client.Webhooks.Requests.V3;

app.MapPost("/webhook", async (HttpRequest httpRequest) =>
{
    using var reader = new StreamReader(httpRequest.Body);
    string body = await reader.ReadToEndAsync();

    if (!WebhookSignature.IsValid(body, secret))
        return Results.Unauthorized();

    var webhook = JsonSerializer.Deserialize<WebhookRequest>(body);
    // ... process the payload
    return Results.Ok();
});
```

If your endpoint authorizes with an API key instead, `WebhookSignature.IsValidApiKey` compares the request's `api_key` against your expected value in constant time.

Replay protection is still yours to implement: record the `nonce` of each accepted request and reject any you have already seen.

## Best practices

1. **Secure your credentials:** Never hardcode API keys in your source code. Use environment variables or secure configuration management.
2. **Validate settings:** The library automatically validates request parameters. Handle validation exceptions appropriately.
3. **Set appropriate expiration times:** Candidate URLs should have sufficient time for the exam duration, while reviewer URLs can have shorter expiration times.
4. **Configure behavior settings thoughtfully:** Adjust behavior weights based on exam type.
5. **Test regex patterns:** Thoroughly test your URL patterns to ensure they correctly match all exam pages.

## License

This project is licensed under the Apache License 2.0. See the [LICENSE](LICENSE) file for details.

## Support

For API documentation and support, visit the [Proctorio Developer Portal](https://proctorio.com/developers).

## Repository

- GitHub: [https://github.com/proctorio/proctorio-api-dotnet-client](https://github.com/proctorio/proctorio-api-dotnet-client)

---

**Copyright © 2024 Proctorio Inc**
