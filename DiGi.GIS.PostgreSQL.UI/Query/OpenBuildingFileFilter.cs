using System.Collections.Generic;
using System.Linq;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Builds the filter string of the add-files dialog for an open building dataset: its own file type first, then All Files, so a release published in a format nobody foresaw can still be picked (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#55).
        /// <para>The extensions come from <see cref="OpenBuildingFileType(string?)"/>; several of them share one entry (<c>*.gz;*.geojsonl</c>), which is how the open file dialog takes a list of patterns. A dataset with no known type gets All Files alone, not a duplicate of it.</para>
        /// </summary>
        /// <param name="datasetKey">The dataset key the tray row fixed, or null.</param>
        /// <returns>The filter in the <c>name|pattern|name|pattern</c> form the open file dialog expects.</returns>
        public static string OpenBuildingFileFilter(string? datasetKey)
        {
            (string fileTypeName, string[] extensions) = OpenBuildingFileType(datasetKey);

            const string allFiles = "All Files (*.*)|*.*";

            if (extensions.Contains("*"))
            {
                return allFiles;
            }

            IEnumerable<string> patterns = extensions.Select(x => $"*.{x}");
            string pattern = string.Join(";", patterns);

            return $"{fileTypeName} ({pattern})|{pattern}|{allFiles}";
        }
    }
}
