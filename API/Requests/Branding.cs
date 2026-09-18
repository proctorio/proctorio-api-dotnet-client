using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests;

/// <summary>
/// Options to customize some user interface controls on the Candidate UI.
/// </summary>
public class Branding
{
    /// <summary>
    /// The hex color code specifies the primary color for some user-interface controls. The value must be a six-digit code, without the #.
    /// Allows a learning platform to align its own color scheme with Proctorio colors and emphasize its brand in the header. Must be a string value.
    /// </summary>
    [JsonPropertyName("primary_color")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "When used, the primary_color must contain exactly 6 characters.")]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "The primary_color must contain an alphanumeric value.")]
    public string? PrimaryColor { get; set; }
    
    /// <summary>
    /// The hex color code specifies the secondary color for some user-interface controls. The value must be a six-digit code, without the #.
    /// Allows a learning platform to align its own color scheme with Proctorio colors and emphasize its brand. Must be a string value.
    /// </summary>
    [JsonPropertyName("secondary_color")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "When used, the secondary_color must contain exactly 6 characters.")]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "The secondary_color must contain an alphanumeric value.")]
    public string? SecondaryColor { get; set; }

    /// <summary>
    /// Custom logo endpoint URL. Http Method: GET. The response should be an image. CORS safe. Height of the logo will be 32px, width is flexible.
    /// </summary>
    [JsonPropertyName("logo_url")]
    [StringLength(600, MinimumLength = 1, ErrorMessage = "When used, the logo_url cannot be empty or exceed 600 characters.")]
    public string? LogoUrl { get; set; }
}