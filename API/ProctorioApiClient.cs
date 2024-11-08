using Proctorio.Client.API.Requests;
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

    private async Task<HttpResponseMessage> SendRequest(string jsonContent, string endpoint)
    {
        string url = $"{_baseUrl}{endpoint}";
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(jsonContent, Encoding.UTF8, "application/json") };
        Helpers.SetHeaders(request, _apiKey);
        using HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return response;
    }

    private async Task<string> GenerateLaunchUrl<T>(T launchRequest, string endpoint)
    {
        try
        {
            string jsonContent = JsonSerializer.Serialize(launchRequest, options);
            HttpResponseMessage response = await SendRequest(jsonContent, endpoint);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return responseContent;
            }
            else
            {
                throw new HttpRequestException(responseContent, null, response.StatusCode);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> GenerateCandidateUrl(CandidateLaunchRequest candidateLaunchRequestParameters)
    {
        string endpoint = "/v2/candidate/launch";
        string result = await GenerateLaunchUrl(candidateLaunchRequestParameters, endpoint);
        return result;
    }
    public async Task<string> GenerateReviewUrl(ReviewerLaunchRequest reviewerLaunchRequestParameters)
    {
        string endpoint = "/v2/reviewer/launch";
        string result = await GenerateLaunchUrl(reviewerLaunchRequestParameters, endpoint);
        return result;
    }
    public async Task<string> GenerateLiveUrl(LiveLaunchRequest liveLaunchRequestParameters)
    {
        string endpoint = "/v2/live/launch";
        string result = await GenerateLaunchUrl(liveLaunchRequestParameters, endpoint);
        return result;
    }
};