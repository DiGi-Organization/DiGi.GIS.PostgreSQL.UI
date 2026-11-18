using System;
using System.Text.Json;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Reads the session token out of the body a login or refresh of the user Web API answered with.
        /// <para>Both answer an anonymous object, which ASP.NET Core writes camelCase (<c>{"token":"…"}</c>), so the property is matched without regard to case rather than against the spelling either side uses today.</para>
        /// </summary>
        /// <param name="json">The response body. This value can be null.</param>
        /// <returns>The token, or null when the body is missing, is not an object or carries no non-blank string token.</returns>
        public static string? TokenString(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            try
            {
                using JsonDocument jsonDocument = JsonDocument.Parse(json!);
                if (jsonDocument.RootElement.ValueKind != JsonValueKind.Object)
                {
                    return null;
                }

                foreach (JsonProperty jsonProperty in jsonDocument.RootElement.EnumerateObject())
                {
                    if (!string.Equals(jsonProperty.Name, "token", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (jsonProperty.Value.ValueKind != JsonValueKind.String)
                    {
                        return null;
                    }

                    string? tokenString = jsonProperty.Value.GetString();
                    return string.IsNullOrWhiteSpace(tokenString) ? null : tokenString;
                }

                return null;
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
