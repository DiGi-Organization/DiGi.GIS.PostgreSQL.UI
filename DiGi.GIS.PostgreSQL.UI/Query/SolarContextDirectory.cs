using System;
using System.IO;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Gets the folder the solar urban-context export is offered by default: <c>reports/solar_context</c> beside this application's executable.
        /// <para>The export refuses to run without a folder - there is no sensible location on a server it could pick on its own (DiGi.GIS.PostgreSQL#136) - so the dialog proposes this one. It is the deployed counterpart of <c>user files/reports</c>, the git-ignored place for generated reports, because the build flattens <c>user files</c> into the output root.</para>
        /// </summary>
        /// <param name="baseDirectory">The directory the folder is resolved against, or null to use this application's own output. A test supplies one so the answer does not depend on where the test host runs.</param>
        /// <returns>The full path of the default folder. It is not created here; the dialog creates it when it is accepted.</returns>
        public static string SolarContextDirectory(string? baseDirectory = null)
        {
            return Path.Combine(string.IsNullOrWhiteSpace(baseDirectory) ? AppContext.BaseDirectory : baseDirectory!, Constants.DirectoryName.Reports, Constants.DirectoryName.SolarContext);
        }
    }
}
