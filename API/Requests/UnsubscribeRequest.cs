using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests;

/// <summary>
/// The unsubscribe request marks the termination of a previously established subscription to Proctorio webhook notifications.
/// </summary>
public class UnsubscribeRequest
{
    /// <summary>
    /// Creates a request to terminate a webhook subscription.
    /// </summary>
    public UnsubscribeRequest(string id)
    {
        Id = id;

        ValidationOutput validationResult = Helpers.Validate(this);
        if (!validationResult.IsValid)
            throw new ArgumentException(JsonSerializer.Serialize(validationResult.ValidationResults));
    }

    /// <summary>
    /// This request must contain a unique value for the specific webhook subscription being removed.
    /// </summary>
    [JsonPropertyName("id")]
    [Required]
    [StringLength(35, MinimumLength = 35, ErrorMessage = "The id value must be exactly 35 characters.")]
    public string Id { get; set; }
}
