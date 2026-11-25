using DiGi.GIS.PostgreSQL.Classes;
using System;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Names why a set of open building match options cannot start a run, or answers null when it can (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#57).
        /// <para>The rules the core task (<see cref="PostgreSQLOpenBuildingMatchTask"/>, DiGi-Organization/DiGi.GIS.PostgreSQL#127) enforces are asked here first, so the operator is told what is wrong while the options dialog is still open instead of from a refused run's log; the core task stays the final gate. The rule parameters are checked here only: the core task takes them as given, so a share above one or a negative area would run and quietly match nothing.</para>
        /// <para>The checks, in order: the datasets (null is all five footprint datasets; an empty set, an unknown key or GHS-OBAT - centroids, never matched - is refused), the five rule parameters (finite; the IoU threshold and the minimum overlap share between 0 and 1, the minimum area, the centroid distance and the boundary margin not negative), and exactly one scope form - a county list or a bounding box with an area.</para>
        /// </summary>
        /// <param name="postgreSQLOpenBuildingMatchTaskOptions">The options to check, or null.</param>
        /// <returns>The message naming the first problem found, or null when the options can start a run.</returns>
        public static string? OpenBuildingMatchOptionsMessage(PostgreSQLOpenBuildingMatchTaskOptions? postgreSQLOpenBuildingMatchTaskOptions)
        {
            if (postgreSQLOpenBuildingMatchTaskOptions is null)
            {
                return "No options were given.";
            }

            if (postgreSQLOpenBuildingMatchTaskOptions.DatasetKeys is not null)
            {
                if (postgreSQLOpenBuildingMatchTaskOptions.DatasetKeys.Count == 0)
                {
                    return "No dataset was chosen.";
                }

                foreach (string datasetKey in postgreSQLOpenBuildingMatchTaskOptions.DatasetKeys)
                {
                    string? tableName = PostgreSQL.Query.OpenBuildingTableName(datasetKey);
                    if (tableName is null)
                    {
                        return $"The dataset key {datasetKey?.Trim() ?? "(null)"} names no open building dataset.";
                    }

                    if (string.Equals(tableName, PostgreSQL.Constants.TableName.GHS_OBAT_Building, StringComparison.Ordinal))
                    {
                        return "GHS-OBAT holds centroids, not footprints, and is not matched.";
                    }
                }
            }

            if (postgreSQLOpenBuildingMatchTaskOptions.OpenBuildingMatchParameters is OpenBuildingMatchParameters openBuildingMatchParameters)
            {
                if (!double.IsFinite(openBuildingMatchParameters.IoUThreshold) || openBuildingMatchParameters.IoUThreshold < 0 || openBuildingMatchParameters.IoUThreshold > 1)
                {
                    return "The IoU threshold has to be between 0 and 1.";
                }

                if (!double.IsFinite(openBuildingMatchParameters.MinOverlapShare) || openBuildingMatchParameters.MinOverlapShare < 0 || openBuildingMatchParameters.MinOverlapShare > 1)
                {
                    return "The minimum overlap share has to be between 0 and 1.";
                }

                if (!double.IsFinite(openBuildingMatchParameters.MinArea) || openBuildingMatchParameters.MinArea < 0)
                {
                    return "The minimum area has to be 0 or more square metres.";
                }

                if (!double.IsFinite(openBuildingMatchParameters.MaxCentroidDistance) || openBuildingMatchParameters.MaxCentroidDistance < 0)
                {
                    return "The maximum centroid distance has to be 0 (off) or more metres.";
                }

                if (!double.IsFinite(openBuildingMatchParameters.BoundaryMargin) || openBuildingMatchParameters.BoundaryMargin < 0)
                {
                    return "The boundary margin has to be 0 or more metres.";
                }
            }

            bool hasCountyScope = postgreSQLOpenBuildingMatchTaskOptions.CountyIds is not null && postgreSQLOpenBuildingMatchTaskOptions.CountyIds.Count != 0;
            bool hasBoundingBoxScope = postgreSQLOpenBuildingMatchTaskOptions.BoundingBox2D is not null;

            if (hasCountyScope == hasBoundingBoxScope)
            {
                return "The scope has to be a county list or a bounding box, exactly one of the two.";
            }

            if (postgreSQLOpenBuildingMatchTaskOptions.BoundingBox2D is Geometry.Planar.Classes.BoundingBox2D boundingBox2D && (boundingBox2D.Min.X == boundingBox2D.Max.X || boundingBox2D.Min.Y == boundingBox2D.Max.Y))
            {
                // A zero-area box resolves to no county part: a finished run that matched nothing, not the mistake it is.
                return "The bounding box has no area - x1 differs from x2, and y1 from y2.";
            }

            return null;
        }
    }
}
