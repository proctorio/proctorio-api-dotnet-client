using System.Text.Json.Serialization;
using Proctorio.Client.API.Requests;

namespace Proctorio.Client.API.Responses;

/// <summary>
/// Represents a subscribed webhook, as returned by the list and subscribe endpoints.
/// </summary>
public class SubscribeResponse
{
    /// <summary>
    /// Unique identifier for the webhook subscription.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// 1 - suspicion calculation webhook type.
    /// </summary>
    [JsonPropertyName("webhook_type")]
    public int WebhookType { get; set; }

    /// <summary>
    /// The client endpoint URL the webhook is subscribed to.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// The client's API key value, if provided.
    /// </summary>
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    /// <summary>
    /// Additional data associated with the subscription.
    /// </summary>
    [JsonPropertyName("data")]
    public ScoreHookResponseData? Data { get; set; }

    /// <summary>
    /// Version of the webhook the subscription is using.
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }
}

/// <summary>
/// Additional data returned for a suspicion calculation webhook subscription.
/// </summary>
public class ScoreHookResponseData
{
    /// <summary>
    /// The behavior settings the subscription was created with.
    /// </summary>
    [JsonPropertyName("behavior_settings")]
    public BehaviorSettings? BehaviorSettings { get; set; }
}
