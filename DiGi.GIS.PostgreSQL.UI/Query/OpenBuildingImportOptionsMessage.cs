using DiGi.GIS.PostgreSQL.Classes;
using System.Collections.Generic;
using System.IO;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Names why a set of open building import options cannot start a run, or answers null when it can (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#38).
        /// <para>The same rules the core task enforces are asked here first, so the operator is told what is missing while the options dialog is still open, instead of a run that starts and then fails into a log line. The core task stays the final gate - this is the earlier, friendlier copy of it.</para>
        /// <para>The checks are ordered from what the release is to what it covers: the dataset key, the two columns the register keys on, the source files, the quadkey the Microsoft reader needs per run, and exactly one scope form - a county list or a bounding box, never both and never neither.</para>
        /// </summary>
        /// <param name="postgreSQLOpenBuildingImportTaskOptions">The options to check, or null.</param>
        /// <returns>The message naming the first problem found, or null when the options can start a run.</returns>
        public static string? OpenBuildingImportOptionsMessage(PostgreSQLOpenBuildingImportTaskOptions? postgreSQLOpenBuildingImportTaskOptions)
        {
            if (postgreSQLOpenBuildingImportTaskOptions is null)
            {
                return "No options were given.";
            }

            string? datasetKey = postgreSQLOpenBuildingImportTaskOptions.DatasetKey;
            if (string.IsNullOrWhiteSpace(datasetKey))
            {
                return "No dataset was chosen.";
            }

            string? tableName = PostgreSQL.Query.OpenBuildingTableName(datasetKey);
            if (tableName is null)
            {
                return $"The dataset key {datasetKey.Trim()} names no open building dataset.";
            }

            if (string.IsNullOrWhiteSpace(postgreSQLOpenBuildingImportTaskOptions.Version))
            {
                return "The release needs a version - the register keys on it.";
            }

            if (postgreSQLOpenBuildingImportTaskOptions.DownloadDate is null)
            {
                return "The release needs a download date - the register keys on it.";
            }

            HashSet<string>? sourcePaths = postgreSQLOpenBuildingImportTaskOptions.SourcePaths;
            if (sourcePaths is null || sourcePaths.Count == 0)
            {
                return "No source file was named.";
            }

            foreach (string path in sourcePaths)
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    return "A source file path is empty.";
                }

                if (!File.Exists(path))
                {
                    return $"The source file does not exist: {path}";
                }
            }

            if (tableName == PostgreSQL.Constants.TableName.Microsoft_Building2D && string.IsNullOrWhiteSpace(postgreSQLOpenBuildingImportTaskOptions.Quadkey))
            {
                // The quadkey is the Microsoft chunk's source identity: the reader is handed it per run and
                // the rows it stores name the chunk through it.
                return "The Microsoft dataset needs the quadkey of its source chunk.";
            }

            bool hasCountyScope = postgreSQLOpenBuildingImportTaskOptions.CountyIds is not null && postgreSQLOpenBuildingImportTaskOptions.CountyIds.Count != 0;
            bool hasBoundingBoxScope = postgreSQLOpenBuildingImportTaskOptions.BoundingBox2D is not null;

            if (hasCountyScope == hasBoundingBoxScope)
            {
                // Both is ambiguous - which one truncated the partitions? - and neither has no run.
                return "The scope has to be a county list or a bounding box, exactly one of the two.";
            }

            return null;
        }
    }
}
