using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Proctorio.Client.API;

/// <summary>
/// Internal helper methods used by the Proctorio API client.
/// </summary>
public static class Helpers
{
    /// <summary>
    /// Derives the api_key header value from a consumer key and secret.
    /// </summary>
    public static string GetApiKey(string key, string secret)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(secret);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            string api_key = key + hash;
            return api_key;
        }
    }

    /// <summary>
    /// Adds the api_key header to an outgoing request.
    /// </summary>
    public static void SetHeaders(HttpRequestMessage request, string _apiKey)
    {
        request.Headers.Add("api_key", _apiKey);
    }

    /// <summary>
    /// Validates an instance against its DataAnnotations attributes.
    /// </summary>
    public static ValidationOutput Validate(object instance)
    {
        List<ValidationResult> validationResults = new List<ValidationResult>();
        ValidationContext context = new ValidationContext(instance, serviceProvider: null, items: null);
        bool isValid = Validator.TryValidateObject(instance, context, validationResults, true);
        return new ValidationOutput(isValid, validationResults);
    }
}

/// <summary>
/// Result of validating an object against its DataAnnotations attributes.
/// </summary>
public class ValidationOutput
{
    /// <summary>
    /// Whether the validated object satisfied all of its DataAnnotations attributes.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// The validation errors found, if any.
    /// </summary>
    public List<ValidationResult> ValidationResults { get; set; } = new List<ValidationResult>();

    /// <summary>
    /// Creates a new validation result.
    /// </summary>
    public ValidationOutput(bool isValid, List<ValidationResult> validationResults)
    {
        IsValid = isValid;
        ValidationResults = validationResults;
    }
}
