using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Proctorio.Client.Webhooks
{
    /// <summary>
    /// Verifies the signature on an incoming Proctorio webhook request.
    /// </summary>
    /// <remarks>
    /// The signature is <c>sha1(nonce + ":" + data + ":" + secret)</c>, where <c>data</c> is the JSON
    /// text of the request's <c>data</c> object exactly as it was transmitted, and <c>secret</c> is the
    /// value Proctorio shared with you. Because the hash covers the bytes on the wire, always validate
    /// against the raw request body: re-serializing a deserialized model will not reproduce the original
    /// text and the signature will not match.
    /// </remarks>
    public static class WebhookSignature
    {
        /// <summary>
        /// Validates the signature of a raw webhook request body.
        /// </summary>
        /// <param name="requestBody">The unmodified request body, exactly as received.</param>
        /// <param name="secret">The webhook secret shared by Proctorio.</param>
        /// <returns>
        /// True when the body carries a signature matching the secret. False when the signature does not
        /// match, or the body is not JSON containing <c>nonce</c>, <c>signature</c> and <c>data</c>.
        /// </returns>
        public static bool IsValid(string requestBody, string secret)
        {
            if (string.IsNullOrEmpty(requestBody)) return false;

            try
            {
                using JsonDocument document = JsonDocument.Parse(requestBody);
                JsonElement root = document.RootElement;
                if (root.ValueKind != JsonValueKind.Object) return false;

                if (!root.TryGetProperty("nonce", out JsonElement nonce) || nonce.ValueKind != JsonValueKind.String)
                    return false;
                if (!root.TryGetProperty("signature", out JsonElement signature) || signature.ValueKind != JsonValueKind.String)
                    return false;
                if (!root.TryGetProperty("data", out JsonElement data))
                    return false;

                return IsValid(nonce.GetString()!, data.GetRawText(), signature.GetString()!, secret);
            }
            catch (JsonException)
            {
                return false;
            }
        }

        /// <summary>
        /// Validates a signature against its individual parts.
        /// </summary>
        /// <param name="nonce">The <c>nonce</c> value from the request.</param>
        /// <param name="dataJson">The JSON text of the request's <c>data</c> object, as transmitted.</param>
        /// <param name="signature">The <c>signature</c> value from the request.</param>
        /// <param name="secret">The webhook secret shared by Proctorio.</param>
        public static bool IsValid(string nonce, string dataJson, string signature, string secret)
        {
            if (string.IsNullOrEmpty(signature)) return false;

            string expected = Compute(nonce, dataJson, secret);

            byte[] expectedBytes = Encoding.ASCII.GetBytes(expected);
            byte[] actualBytes = Encoding.ASCII.GetBytes(signature.ToLowerInvariant());
            return CryptographicOperations.FixedTimeEquals(expectedBytes, actualBytes);
        }

        /// <summary>
        /// Computes the expected signature for a webhook request as a lowercase hex string.
        /// </summary>
        /// <param name="nonce">The <c>nonce</c> value from the request.</param>
        /// <param name="dataJson">The JSON text of the request's <c>data</c> object, as transmitted.</param>
        /// <param name="secret">The webhook secret shared by Proctorio.</param>
        public static string Compute(string nonce, string dataJson, string secret)
        {
            ArgumentNullException.ThrowIfNull(nonce);
            ArgumentNullException.ThrowIfNull(dataJson);
            ArgumentNullException.ThrowIfNull(secret);

            byte[] payload = Encoding.UTF8.GetBytes($"{nonce}:{dataJson}:{secret}");
            byte[] hash = SHA1.HashData(payload);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        /// <summary>
        /// Compares a webhook's <c>api_key</c> against the expected value in constant time, for
        /// integrations that authorize with an API key instead of a signature.
        /// </summary>
        public static bool IsValidApiKey(string? apiKey, string expectedApiKey)
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(expectedApiKey)) return false;

            byte[] apiKeyBytes = Encoding.UTF8.GetBytes(apiKey);
            byte[] expectedBytes = Encoding.UTF8.GetBytes(expectedApiKey);
            return CryptographicOperations.FixedTimeEquals(apiKeyBytes, expectedBytes);
        }
    }
}
