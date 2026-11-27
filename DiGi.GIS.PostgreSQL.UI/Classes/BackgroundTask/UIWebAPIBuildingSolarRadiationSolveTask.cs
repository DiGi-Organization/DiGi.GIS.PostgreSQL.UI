using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Solar.Classes;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.PostgreSQL.UI.Windows;
using DiGi.GIS.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// A solar radiation solve run on this machine through the GIS Web API and scoped from the user interface (DiGi.GIS.PostgreSQL.UI#48): the county parts, radii, ceilings, tile, batch and timeout settings are asked for through the same <see cref="PostgreSQLBuildingSolarRadiationSolveOptionsWindow"/> the server row uses, then a user signs in through <see cref="UserSignInWindow"/>, and only then is the run handed to <see cref="WebAPIBuildingSolarRadiationSolveTask"/>.
    /// <para>The password goes from the window straight into one sign-in request on this thread and is not kept; the token lives on the task for the run and is renewed by it. A refused sign-in ends the run with nothing solved.</para>
    /// </summary>
    public class UIWebAPIBuildingSolarRadiationSolveTask : WebAPIBuildingSolarRadiationSolveTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIWebAPIBuildingSolarRadiationSolveTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The GIS Web API manager used to sign in, read the counties, buildings, models and weather and to write the results.</param>
        public UIWebAPIBuildingSolarRadiationSolveTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            // The dialogs are windows, and this runs on a thread pool thread, where a window cannot be created at all.
            // Without an application there is no user interface thread to move them to.
            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no WPF application is running - the solve options cannot be asked for", nameof(UIWebAPIBuildingSolarRadiationSolveTask));
                return false;
            }

            PostgreSQLBuildingSolarRadiationSolveOptions ??= new();

            // Every county part, read on this thread and shown on the user interface thread.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await ReadCountyReferencesAsync(PostgreSQLBuildingSolarRadiationSolveOptions.CommandTimeout, cancellationToken);
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: counties could not be retrieved - the solve cannot be scoped", nameof(UIWebAPIBuildingSolarRadiationSolveTask));
                return false;
            }

            PostgreSQLBuildingSolarRadiationSolveOptions? postgreSQLBuildingSolarRadiationSolveOptions = null;
            string? email = null;
            string? password = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLBuildingSolarRadiationSolveOptionsWindow postgreSQLBuildingSolarRadiationSolveOptionsWindow = new(PostgreSQLBuildingSolarRadiationSolveOptions, administrativeAreal2DReferences);
                if (postgreSQLBuildingSolarRadiationSolveOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                UserSignInWindow userSignInWindow = new();
                if (userSignInWindow.ShowDialog() is not bool dialogResult_SignIn || !dialogResult_SignIn)
                {
                    return;
                }

                postgreSQLBuildingSolarRadiationSolveOptions = postgreSQLBuildingSolarRadiationSolveOptionsWindow.PostgreSQLBuildingSolarRadiationSolveOptions;
                email = userSignInWindow.Email;
                password = userSignInWindow.Password;
            });

            // A cancelled dialog leaves the options of an earlier run as they were - the window works on a copy.
            if (postgreSQLBuildingSolarRadiationSolveOptions is null)
            {
                Serilog.Modify.Log("{Type}: solve options or sign-in were cancelled - nothing was solved", nameof(UIWebAPIBuildingSolarRadiationSolveTask));
                CancelRun();
                return false;
            }

            string? tokenString = await gISWebAPIManager.UserTokenStringAsync(email, password, cancellationToken);
            if (tokenString is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: sign-in was refused or could not be completed - nothing was solved", nameof(UIWebAPIBuildingSolarRadiationSolveTask));
                return false;
            }

            PostgreSQLBuildingSolarRadiationSolveOptions = postgreSQLBuildingSolarRadiationSolveOptions;
            TokenString = tokenString;

            try
            {
                return await base.ExecuteAsync(progress, cancellationToken);
            }
            finally
            {
                // The session is the run's; a later run signs in again.
                TokenString = null;
            }
        }
    }
}
