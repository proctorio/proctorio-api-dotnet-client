using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests;

/// <summary>
/// The subscribe request formalizes the initiation of a subscription to the suspicion calculation webhook.
/// </summary>
public class SuspicionCalculationSubscribeRequest
{
    /// <summary>
    /// Creates a subscribe request for the suspicion calculation webhook.
    /// </summary>
    public SuspicionCalculationSubscribeRequest(ClientApiInfo client)
    {
        Client = client;

        ValidationOutput validationResult = Helpers.Validate(this);
        if (!validationResult.IsValid)
            throw new ArgumentException(JsonSerializer.Serialize(validationResult.ValidationResults));
    }

    /// <summary>
    /// Must contain values to the specific client's api endpoint.
    /// </summary>
    [JsonPropertyName("client")]
    [Required]
    public ClientApiInfo Client { get; set; }

    /// <summary>
    /// Behavior settings determines the suspicion of each recorded action and configure exam analytics.
    /// </summary>
    [JsonPropertyName("behavior_settings")]
    public BehaviorSettings? BehaviorSettings { get; set; }

    /// <summary>
    /// Version of the suspicion calculation webhook to subscribe to. Currently supported versions are "2" and "3".
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }
}
