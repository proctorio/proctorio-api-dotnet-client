using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Proctorio.Client.API;

public static class Helpers
{
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

    public static void SetHeaders(HttpRequestMessage request, string _apiKey)
    {
        request.Headers.Add("api_key", _apiKey);
    }

    public static ValidationOutput Validate(object instance)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(instance, serviceProvider: null, items: null);
        bool isValid = Validator.TryValidateObject(instance, context, validationResults, true);
        return new ValidationOutput(isValid, validationResults);
    }
}

public class ValidationOutput
{
    public bool IsValid { get; set; }
    public List<ValidationResult> ValidationResults { get; set; } = new List<ValidationResult>();

    public ValidationOutput(bool isValid, List<ValidationResult> validationResults)
    {
        IsValid = isValid;
        ValidationResults = validationResults;
    }
}
