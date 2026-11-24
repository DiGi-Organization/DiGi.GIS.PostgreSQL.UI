using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Collects the source files of an open building dataset below a folder, so a release that comes as a tree - Microsoft ships one file in each of its <c>quadkey=&lt;digits&gt;</c> folders - is added in one step (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#55).
        /// <para>Only the files of the dataset's type are returned (<see cref="OpenBuildingFileType(string?)"/>): the Microsoft catalog <c>dataset-links.csv</c> beside the release folders is not one, and neither is a manifest or a report. The extension is compared without regard to case, a folder that cannot be read is skipped rather than failing the whole read, and the paths come back sorted by ordinal comparison, the order the options list keeps.</para>
        /// </summary>
        /// <param name="datasetKey">The dataset key the tray row fixed, or null.</param>
        /// <param name="directory">The folder to read, with all its sub-folders.</param>
        /// <returns>The full paths of the matching files, or null when the folder is empty text or does not exist.</returns>
        public static List<string>? OpenBuildingSourcePaths(string? datasetKey, string? directory)
        {
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                return null;
            }

            string[] extensions = OpenBuildingFileType(datasetKey).Extensions;
            bool any = extensions.Contains("*");

            EnumerationOptions enumerationOptions = new()
            {
                RecurseSubdirectories = true,
                IgnoreInaccessible = true
            };

            List<string> result = [];
            foreach (string path in Directory.EnumerateFiles(directory, "*", enumerationOptions))
            {
                string extension = Path.GetExtension(path).TrimStart('.');
                if (any || extensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                {
                    result.Add(path);
                }
            }

            result.Sort(StringComparer.Ordinal);
            return result;
        }
    }
}
