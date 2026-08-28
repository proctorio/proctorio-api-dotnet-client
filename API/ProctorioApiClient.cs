using Proctorio.Client.API.Requests;
using Proctorio.Client.API.Responses;
using System.Text;
using System.Text.Json;

namespace Proctorio.Client.API;

/// <summary>
/// Proctorio API Client wrapper.
/// </summary>
public class ProctorioAPIClient
{
    private readonly string _baseUrl;
    private readonly string _apiKey;
    JsonSerializerOptions? options;
    /// <summary>
    /// Proctorio APIClient constructor.
    /// </summary>
    /// <param name="baseUrl">Api endpoint. For ex: https://{{region}}{{endpoint}}.com</param>
    /// <param name="consumerKey">Consumer key.</param>
    /// <param name="consumerSecret">Consumer secret.</param>
    /// <param name="options">Json serializer options.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ProctorioAPIClient(string baseUrl, string consumerKey, string consumerSecret, JsonSerializerOptions? options = null)
    {
        if (string.IsNullOrEmpty(baseUrl)) throw new ArgumentNullException(nameof(baseUrl));
        if (string.IsNullOrEmpty(consumerKey)) throw new ArgumentNullException(nameof(consumerKey));
        if (string.IsNullOrEmpty(consumerSecret)) throw new ArgumentNullException(nameof(consumerSecret));

        _baseUrl = baseUrl;
        _apiKey = Helpers.GetApiKey(consumerKey, consumerSecret);
        this.options = options == null ? new JsonSerializerOptions() : options;

    }

    private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string endpoint, string? jsonContent = null)
    {
        string url = $"{_baseUrl}{endpoint}";
        HttpRequestMessage request = new HttpRequestMessage(method, url);
        if (jsonContent != null)
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        Helpers.SetHeaders(request, _apiKey);
        using HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return response;
    }

    private async Task<string> SendAndReadResponse(HttpMethod method, string endpoint, string? jsonContent = null)
    {
        HttpResponseMessage response = await SendRequest(method, endpoint, jsonContent);
        string responseContent = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
            return responseContent;

        throw new HttpRequestException(responseContent, null, response.StatusCode);
    }

    private async Task<string> GenerateLaunchUrl<T>(T launchRequest, string endpoint)
    {
        string jsonContent = JsonSerializer.Serialize(launchRequest, options);
        return await SendAndReadResponse(HttpMethod.Post, endpoint, jsonContent);
    }

    /// <summary>
    /// Generates a launch URL for a Candidate taking an exam.
    /// </summary>
    public async Task<string> GenerateCandidateUrl(CandidateLaunchRequest candidateLaunchRequestParameters)
    {
        string endpoint = "/v2/candidate/launch";
        string result = await GenerateLaunchUrl(candidateLaunchRequestParameters, endpoint);
        return result;
    }

    /// <summary>
    /// Generates a launch URL for a Reviewer accessing the Review Center.
    /// </summary>
    public async Task<string> GenerateReviewUrl(ReviewerLaunchRequest reviewerLaunchRequestParameters)
    {
        string endpoint = "/v2/reviewer/launch";
        string result = await GenerateLaunchUrl(reviewerLaunchRequestParameters, endpoint);
        return result;
    }

    /// <summary>
    /// Generates a launch URL for a Live proctoring session.
    /// </summary>
    public async Task<string> GenerateLiveUrl(LiveLaunchRequest liveLaunchRequestParameters)
    {
        string endpoint = "/v2/live/launch";
        string result = await GenerateLaunchUrl(liveLaunchRequestParameters, endpoint);
        return result;
    }

    /// <summary>
    /// Retrieves a summary of active webhook subscriptions.
    /// </summary>
    public async Task<List<SubscribeResponse>> ListWebhooks()
    {
        string endpoint = "/v2/whks/list";
        string responseContent = await SendAndReadResponse(HttpMethod.Get, endpoint);
        return JsonSerializer.Deserialize<List<SubscribeResponse>>(responseContent, options) ?? [];
    }

    /// <summary>
    /// Retrieves the list of webhook types available to subscribe to (e.g. "suspicion_calculation").
    /// </summary>
    public async Task<List<string>> ListAvailableWebhookTypes()
    {
        string endpoint = "/v2/whks/list?type=available";
        string responseContent = await SendAndReadResponse(HttpMethod.Get, endpoint);
        return JsonSerializer.Deserialize<List<string>>(responseContent, options) ?? [];
    }

    /// <summary>
    /// Subscribes to the suspicion calculation webhook (webhook type 1).
    /// </summary>
    public async Task<SubscribeResponse> SubscribeToSuspicionCalculationWebhook(SuspicionCalculationSubscribeRequest subscribeRequestParameters)
    {
        string endpoint = "/v2/whks/subscribe/1";
        string jsonContent = JsonSerializer.Serialize(subscribeRequestParameters, options);
        string responseContent = await SendAndReadResponse(HttpMethod.Post, endpoint, jsonContent);
        return JsonSerializer.Deserialize<SubscribeResponse>(responseContent, options)
            ?? throw new JsonException("Subscribe response could not be deserialized.");
    }

    /// <summary>
    /// Terminates a previously established webhook subscription.
    /// </summary>
    public async Task<string> UnsubscribeWebhook(UnsubscribeRequest unsubscribeRequestParameters)
    {
        string endpoint = "/v2/whks/unsubscribe";
        string jsonContent = JsonSerializer.Serialize(unsubscribeRequestParameters, options);
        return await SendAndReadResponse(HttpMethod.Post, endpoint, jsonContent);
    }
};