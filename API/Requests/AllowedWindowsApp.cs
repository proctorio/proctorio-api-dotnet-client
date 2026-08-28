using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Proctorio.Client.API.Requests;

/// <summary>
/// Identifies a Windows application allowed to remain open when advanced_program_detection is used.
/// </summary>
public class AllowedWindowsApp
{
    /// <summary>
    /// The binary name of the allowed application.
    /// </summary>
    [JsonPropertyName("binary_name")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "binary_name cannot be empty or exceed 50 characters.")]
    public required string BinaryName { get; set; }

    /// <summary>
    /// The product name of the allowed application.
    /// </summary>
    [JsonPropertyName("product_name")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "product_name cannot be empty or exceed 50 characters.")]
    public required string ProductName { get; set; }

    /// <summary>
    /// The company name of the allowed application.
    /// </summary>
    [JsonPropertyName("company_name")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "company_name cannot be empty or exceed 50 characters.")]
    public required string CompanyName { get; set; }
}