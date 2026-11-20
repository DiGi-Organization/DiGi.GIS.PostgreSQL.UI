using System.Collections.Generic;
using System.Text.Json;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Reads the references a <c>building2d/referencesbycountyid</c> read answered with, as returned by <see cref="JsonResponseAsync"/>.
        /// <para>No body - the 404 or 204 of a county part with no buildings - is no reference, so the result is empty; a body that is not a JSON array of strings is a failed read, so the result is null and never empty (DiGi.GIS.PostgreSQL.UI#51).</para>
        /// </summary>
        /// <param name="json">The response body. This value can be null.</param>
        /// <returns>The references, empty when there is no body, or null when the body does not parse.</returns>
        public static List<string>? References(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return [];
            }

            try
            {
                return JsonSerializer.Deserialize<List<string>>(json!);
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
