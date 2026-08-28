using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests;

/// <summary>
/// Must contain values to the specific client's api endpoint.
/// </summary>
public class ClientApiInfo(string url)
{

    /// <summary>
    /// Represents client endpoint url.
    /// </summary>
    [JsonPropertyName("url")]
    [Required]
    [StringLength(600, MinimumLength = 1, ErrorMessage = "The url value cannot be empty or exceed 600 characters.")]
    public string Url { get; set; } = url;

    /// <summary>
    /// Key parameter represents the client's API key value. This parameter is optional.
    /// </summary>
    [JsonPropertyName("key")]
    [StringLength(50, ErrorMessage = "When used, the key value cannot exceed 50 characters.")]
    public string? Key { get; set; }
}
