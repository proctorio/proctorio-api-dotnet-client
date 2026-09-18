using Proctorio.Client.API.Requests;
using Proctorio.Client.API.Responses;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API;

/// <summary>
/// Proctorio API Client wrapper.
/// </summary>
public class ProctorioAPIClient
{
    /// <summary>
    /// Shared client used when the caller does not supply one. A single instance is reused for the
    /// lifetime of the process to avoid socket exhaustion; the pooled connection lifetime keeps it
    /// responsive to DNS changes.
    /// </summary>
    private static readonly HttpClient SharedHttpClient = new HttpClient(new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(5)
    });

    private readonly string _baseUrl;
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Proctorio APIClient constructor.
    /// </summary>
    /// <param name="baseUrl">Api endpoint. For ex: https://{{region}}{{endpoint}}.com</param>
    /// <param name="consumerKey">Consumer key.</param>
    /// <param name="consumerSecret">Consumer secret.</param>
    /// <param name="options">Json serializer options.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ProctorioAPIClient(string baseUrl, string consumerKey, string consumerSecret, JsonSerializerOptions? options = null)
        : this(null, baseUrl, consumerKey, consumerSecret, options)
    {
    }

    /// <summary>
    /// Proctorio APIClient constructor taking a caller supplied <see cref="HttpClient"/>, for use with
    /// dependency injection or <c>IHttpClientFactory</c>. The client is not disposed by this class.
    /// </summary>
    /// <param name="httpClient">Client used for all requests. When null, a shared internal client is used.</param>
    /// <param name="baseUrl">Api endpoint. For ex: https://{{region}}{{endpoint}}.com</param>
    /// <param name="consumerKey">Consumer key.</param>
    /// <param name="consumerSecret">Consumer secret.</param>
    /// <param name="options">Json serializer options.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ProctorioAPIClient(HttpClient? httpClient, string baseUrl, string consumerKey, string consumerSecret, JsonSerializerOptions? options = null)
    {
        if (string.IsNullOrEmpty(baseUrl)) throw new ArgumentNullException(nameof(baseUrl));
        if (string.IsNullOrEmpty(consumerKey)) throw new ArgumentNullException(nameof(consumerKey));
        if (string.IsNullOrEmpty(consumerSecret)) throw new ArgumentNullException(nameof(consumerSecret));

        _baseUrl = baseUrl;
        _apiKey = Helpers.GetApiKey(consumerKey, consumerSecret);
        _httpClient = httpClient ?? SharedHttpClient;
        _jsonOptions = BuildJsonOptions(options);
    }

    /// <summary>
    /// Copies the caller's options so they are not mutated, and omits null properties from serialized
    /// payloads so that unset optional parameters are not sent to the API. An explicit
    /// <see cref="JsonSerializerOptions.DefaultIgnoreCondition"/> on the supplied options is respected.
    /// </summary>
    private static JsonSerializerOptions BuildJsonOptions(JsonSerializerOptions? options)
    {
        JsonSerializerOptions effectiveOptions = options == null
            ? new JsonSerializerOptions()
            : new JsonSerializerOptions(options);

        if (effectiveOptions.DefaultIgnoreCondition == JsonIgnoreCondition.Never)
            effectiveOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        return effectiveOptions;
    }

    /// <summary>
    /// Sends a request to the given endpoint. The caller owns the returned response and must dispose it.
    /// </summary>
    private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string endpoint, string? jsonContent, CancellationToken cancellationToken)
    {
        string url = $"{_baseUrl}{endpoint}";
        using HttpRequestMessage request = new HttpRequestMessage(method, url);
        if (jsonContent != null)
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        Helpers.SetHeaders(request, _apiKey);

        return await _httpClient.SendAsync(request, cancellationToken);
    }

    private async Task<string> SendAndReadResponse(HttpMethod method, string endpoint, string? jsonContent, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await SendRequest(method, endpoint, jsonContent, cancellationToken);
        string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
            return responseContent;

        throw new HttpRequestException(responseContent, null, response.StatusCode);
    }

    /// <summary>
    /// Deserializes a list response, treating an empty body (e.g. a 204 with nothing to return) the
    /// same as an empty list rather than letting <see cref="JsonSerializer"/> throw on empty input.
    /// </summary>
    private List<T> DeserializeList<T>(string responseContent)
    {
        if (string.IsNullOrWhiteSpace(responseContent))
            return [];

        return JsonSerializer.Deserialize<List<T>>(responseContent, _jsonOptions) ?? [];
    }

    private async Task<string> GenerateLaunchUrl<T>(T launchRequest, string endpoint, CancellationToken cancellationToken)
    {
        string jsonContent = JsonSerializer.Serialize(launchRequest, _jsonOptions);
        return await SendAndReadResponse(HttpMethod.Post, endpoint, jsonContent, cancellationToken);
    }

    /// <summary>
    /// Generates a launch URL for a Candidate taking an exam.
    /// </summary>
    public async Task<string> GenerateCandidateUrl(CandidateLaunchRequest candidateLaunchRequestParameters, CancellationToken cancellationToken = default)
    {
        string endpoint = "/v2/candidate/launch";
        string result = await GenerateLaunchUrl(candidateLaunchRequestParameters, endpoint, cancellationToken);
        return result;
    }

    /// <summary>
    /// Generates a launch URL for a Reviewer accessing the Review Center.
    /// </summary>
    public async Task<string> GenerateReviewUrl(ReviewerLaunchRequest reviewerLaunchRequestParameters, CancellationToken cancellationToken = default)
    {
        string endpoint = "/v2/reviewer/launch";
        string result = await GenerateLaunchUrl(reviewerLaunchRequestParameters, endpoint, cancellationToken);
        return result;
    }

    /// <summary>
    /// Generates a launch URL for a Live proctoring session.
    /// </summary>
    public async Task<string> GenerateLiveUrl(LiveLaunchRequest liveLaunchRequestParameters, CancellationToken cancellationToken = default)
    {
        string endpoint = "/v2/live/launch";
        string result = await GenerateLaunchUrl(liveLaunchRequestParameters, endpoint, cancellationToken);
        return result;
    }

    /// <summary>
    /// Retrieves a summary of active webhook subscriptions.
    /// </summary>
    public async Task<List<SubscribeResponse>> ListWebhooks(CancellationToken cancellationToken = default)
    {
        string endpoint = "/v2/whks/list";
        string responseContent = await SendAndReadResponse(HttpMethod.Get, endpoint, null, cancellationToken);
        return DeserializeList<SubscribeResponse>(responseContent);
    }

    /// <summary>
    /// Retrieves the list of webhook types available to subscribe to (e.g. "suspicion_calculation").
    /// </summary>
    public async Task<List<string>> ListAvailableWebhookTypes(CancellationToken cancellationToken = default)
    {
        string endpoint = "/v2/whks/list?type=available";
        string responseContent = await SendAndReadResponse(HttpMethod.Get, endpoint, null, cancellationToken);
        return DeserializeList<string>(responseContent);
    }

    /// <summary>
    /// Subscribes to the suspicion calculation webhook (webhook type 1).
    /// </summary>
    public async Task<SubscribeResponse> SubscribeToSuspicionCalculationWebhook(SuspicionCalculationSubscribeRequest subscribeRequestParameters, CancellationToken cancellationToken = default)
    {
        string endpoint = "/v2/whks/subscribe/1";
        string jsonContent = JsonSerializer.Serialize(subscribeRequestParameters, _jsonOptions);
        string responseContent = await SendAndReadResponse(HttpMethod.Post, endpoint, jsonContent, cancellationToken);
        return JsonSerializer.Deserialize<SubscribeResponse>(responseContent, _jsonOptions)
            ?? throw new JsonException("Subscribe response could not be deserialized.");
    }

    /// <summary>
    /// Terminates a previously established webhook subscription.
    /// </summary>
    public async Task<string> UnsubscribeWebhook(UnsubscribeRequest unsubscribeRequestParameters, CancellationToken cancellationToken = default)
    {
        string endpoint = "/v2/whks/unsubscribe";
        string jsonContent = JsonSerializer.Serialize(unsubscribeRequestParameters, _jsonOptions);
        return await SendAndReadResponse(HttpMethod.Post, endpoint, jsonContent, cancellationToken);
    }
};