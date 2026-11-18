using System;
using System.Text;
using System.Text.Json;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Reads when a session token expires from its <c>exp</c> claim, without validating it - the server does that; the client only needs to know when to renew it.
        /// </summary>
        /// <param name="tokenString">The JWT. This value can be null.</param>
        /// <returns>The expiry, or null when the value is not a three-part JWT whose payload carries a numeric <c>exp</c>.</returns>
        public static DateTimeOffset? TokenExpiresAt(string? tokenString)
        {
            if (string.IsNullOrWhiteSpace(tokenString))
            {
                return null;
            }

            string[] parts = tokenString!.Split('.');
            if (parts.Length != 3 || parts[1].Length == 0)
            {
                return null;
            }

            // Base64url without padding, as RFC 7515 writes it.
            string base64 = parts[1].Replace('-', '+').Replace('_', '/');
            base64 = base64.PadRight(base64.Length + ((4 - (base64.Length % 4)) % 4), '=');

            try
            {
                using JsonDocument jsonDocument = JsonDocument.Parse(Encoding.UTF8.GetString(System.Convert.FromBase64String(base64)));
                if (jsonDocument.RootElement.ValueKind != JsonValueKind.Object || !jsonDocument.RootElement.TryGetProperty("exp", out JsonElement jsonElement) || jsonElement.ValueKind != JsonValueKind.Number || !jsonElement.TryGetInt64(out long seconds))
                {
                    return null;
                }

                return DateTimeOffset.FromUnixTimeSeconds(seconds);
            }
            catch (Exception exception) when (exception is FormatException || exception is JsonException || exception is ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }
}
