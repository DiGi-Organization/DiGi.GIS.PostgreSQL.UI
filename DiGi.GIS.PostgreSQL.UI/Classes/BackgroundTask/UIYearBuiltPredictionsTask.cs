using DiGi.Core.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.PostgreSQL.UI.Windows;
using DiGi.GIS.WebAPI.Classes;
using DiGi.GIS.YOLO.UI.Classes;
using DiGi.GIS.YOLO.UI.Enums;
using DiGi.WebAPI.Classes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// A Year Built prediction run that is scoped from the user interface: the counties, the scratch directory, the interpreter, the request concurrency and the scratch cleanup are asked for through <see cref="YearBuiltPredictionsOptionsWindow"/> each time the task is started.
    /// <para><b>A tray run has one shape, and it writes.</b> The pipeline's steps are not offered - DiGi-Organization/DiGi.GIS.YOLO.UI#8 made the six of them a single run per county and left the granular flags to the options class and the console app - so the dialog settles the scope and the run does the rest. Because OK now means the deployed data of every county selected, the scope is named back in a confirmation before anything is launched.</para>
    /// <para><b>The run happens in another process.</b> The pipeline needs a regressor, and the only implementation of it carries the machine learning closure - about a gigabyte of native libraries, against an application that publishes self-contained and single-file. The <c>IYearBuiltPredictor</c> seam exists to keep that weight out of hosts that only need to start a run, so this task writes the options out and hands them to <c>DiGi.GIS.YOLO.UI.ConsoleApp</c>, which already hosts the pipeline and is already exercised end to end.</para>
    /// <para>Two consequences of that are worth knowing before a run. <b>The runner authorizes with its own key</b>, read from the <c>GIS_WebAPI_Client.conf</c> beside its executable rather than from this application's - a run that ends in <see cref="YearBuiltPredictionExitCode.Authorization"/> is usually that file rather than the one this application uses. And <b>stopping the task kills the run rather than winding it down</b>: the whole process tree goes, the detector included, so a batch that was being written may be half written. Every step of the pipeline is idempotent and a stopped run is re-runnable, but its tallies are not a record of what was stored.</para>
    /// <para>The environment preflight runs here, before anything is launched, so a machine with no CPython carrying ultralytics says so in front of whoever opened the dialog instead of an hour later as an exit code. The pipeline repeats the check inside the run; that costs one interpreter start and is what makes the reason legible.</para>
    /// <para><b>Every refusal before launch names its reason on the task row.</b> Each check below runs before anything is started, logs its reason and throws a <see cref="BackgroundTaskFailureException"/> worded exactly as the log line (<see cref="Query.RenderRefusal(string, object[])"/> renders it), so nobody has to open the logs folder to learn why nothing started. A cancelled dialog or a declined confirmation is not a refusal - nobody scoped a run, so the task reports a completed run that started nothing. A run that fails after launch is reported with its exit code, the last <c>[ERROR]</c>/<c>[FATAL]</c> line the runner printed and the step that failed (<see cref="FailureMessage(YearBuiltPredictionExitCode, IEnumerable{string})"/>).</para>
    /// </summary>
    public class UIYearBuiltPredictionsTask : ReportableBackgroundTask<long>, IGISPostgreSQLUIObject
    {
        private readonly GISWebAPIManager GISWebAPIManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIYearBuiltPredictionsTask"/> class.
        /// </summary>
        /// <param name="GISWebAPIManager">The <see cref="WebAPI.Classes.GISWebAPIManager"/> instance the county rows behind the dialog are read with. The run itself authorizes with the runner's own key, not with this one.</param>
        public UIYearBuiltPredictionsTask(GISWebAPIManager GISWebAPIManager)
        {
            this.GISWebAPIManager = GISWebAPIManager;
        }

        /// <summary>
        /// Gets or sets the path of the headless runner. When null it is resolved by <see cref="Query.YearBuiltPredictionConsoleAppPath"/>, which probes this application's own output, the runner's folder beside it, and the runner's build output in a workspace checkout.
        /// </summary>
        public string? ConsoleAppPath { get; set; } = null;

        /// <summary>
        /// Gets or sets the options the dialog opens with, and which it writes back to when it is closed with OK. When null the defaults are used, which name no county and therefore ask for nothing.
        /// </summary>
        public YearBuiltPredictionPipelineOptions? YearBuiltPredictionPipelineOptions { get; set; } = null;

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            // Asked before the dialog rather than after it: without the runner nothing can be started at all, and
            // discovering that after the counties have been chosen wastes the only part of this the operator does.
            string? path_ConsoleApp = Query.YearBuiltPredictionConsoleAppPath(ConsoleAppPath);
            if (string.IsNullOrWhiteSpace(path_ConsoleApp))
            {
                string template = "{FileName} was not found beside this application or in the workspace - the Year Built prediction run cannot be started";
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, template, Constants.FileName.YearBuiltPredictionConsoleApp);
                throw new BackgroundTaskFailureException(Query.RenderRefusal(template, Constants.FileName.YearBuiltPredictionConsoleApp));
            }

            // The dialog is a window, and this runs on a thread pool thread, where a window cannot be created at
            // all. Without an application there is no user interface thread to move it to.
            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                string template = "No WPF application is running - the Year Built prediction options cannot be asked for";
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, template);
                throw new BackgroundTaskFailureException(Query.RenderRefusal(template));
            }

            HttpClient? httpClient_AdministrativeAreal2D = GISWebAPIManager.CreateHttpClient<AdministrativeAreal2DController>(nameof(AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync), out string? path_AdministrativeAreal2D);
            if (httpClient_AdministrativeAreal2D is null || string.IsNullOrWhiteSpace(path_AdministrativeAreal2D))
            {
                string template = "County references could not be requested - the Year Built prediction run cannot be scoped";
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, template);
                throw new BackgroundTaskFailureException(Query.RenderRefusal(template));
            }

            PostOptions postOptions = new() { RequestResult = true };

            // The endpoint is a HttpGet action and its administrativearealtype parameter is not nullable - omitting it binds to Country, not County.
            string requestUri_AdministrativeAreal2D = new UrlBuilder(path_AdministrativeAreal2D).AddParameter("administrativearealtype", (int)AdministrativeArealType.County).ToString();

            PostResponse<List<AdministrativeAreal2DReference>?> postResponse_AdministrativeAreal2DReferences = await DiGi.WebAPI.Query.GetAsync<List<AdministrativeAreal2DReference>>(httpClient_AdministrativeAreal2D, requestUri_AdministrativeAreal2D, postOptions);
            if (postResponse_AdministrativeAreal2DReferences is null || !postResponse_AdministrativeAreal2DReferences.Succeeded || postResponse_AdministrativeAreal2DReferences.Result is not List<AdministrativeAreal2DReference> administrativeAreal2DReferences || administrativeAreal2DReferences.Count == 0)
            {
                string template = "County references could not be retrieved - the Year Built prediction run cannot be scoped";
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, template);
                throw new BackgroundTaskFailureException(Query.RenderRefusal(template));
            }

            // Read on this thread, shown on the user interface thread. Reading inside the callback below would
            // hold the interface still for the whole of the query.
            YearBuiltPredictionPipelineOptions? yearBuiltPredictionPipelineOptions = null;

            application.Dispatcher.Invoke(() =>
            {
                // The coverage markers are read from the deployed API in the background - two requests for the whole list - and
                // filled in once they arrive, so they never delay the dialog; a failed read marks nothing. Every part id is asked
                // for, so each piece of a multi-part county gets its own marker.
                List<int> countyIds_Coverage = [.. administrativeAreal2DReferences.ConvertAll(x => x.Id)];

                YearBuiltPredictionsOptionsWindow yearBuiltPredictionsOptionsWindow = new(YearBuiltPredictionPipelineOptions, administrativeAreal2DReferences, token => Query.PredictedYearBuiltCoverageFactorsAsync(GISWebAPIManager, countyIds_Coverage, token));

                if (yearBuiltPredictionsOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                // Raised here rather than in the dialog. Every tray run now writes - the steps are no longer
                // offered, so OK means the whole six step flow over the deployed data of whichever counties are
                // selected - and that is worth naming them back once before it starts. Keeping it out of the
                // window also keeps the window testable: a fact that raises its OK click would otherwise hang on
                // a modal box nothing can answer.
                if (!Confirmed(yearBuiltPredictionsOptionsWindow.YearBuiltPredictionPipelineOptions, administrativeAreal2DReferences))
                {
                    return;
                }

                yearBuiltPredictionPipelineOptions = yearBuiltPredictionsOptionsWindow.YearBuiltPredictionPipelineOptions;
            });

            // A cancelled dialog, or a declined confirmation, leaves the options of an earlier run as they were - the
            // window works on a copy - and ends the run here rather than starting one nobody scoped. It is not a
            // failure to report: the row shows a completed run that started nothing, rather than a reason nobody was
            // refused for.
            if (yearBuiltPredictionPipelineOptions is null)
            {
                Serilog.Modify.Log("Year built prediction options were cancelled - nothing was run");
                return true;
            }

            // The two processes do not share a working directory - this one runs from wherever the tray application
            // was started, the runner from beside its own executable - so a relative path names a different folder
            // on each side, and neither would report anything wrong. The committed template ships a relative scratch
            // directory, so this is the ordinary case rather than a corner of one. Made absolute before anything
            // reads them, and written back, so the dialog says next time where the run actually went.
            // ModelPath is deliberately left alone: the weights sit beside the runner, and resolving them against
            // this application would be resolving them against the wrong thing.
            yearBuiltPredictionPipelineOptions.ScratchDirectory = Query.FullPath(yearBuiltPredictionPipelineOptions.ScratchDirectory);
            yearBuiltPredictionPipelineOptions.WorkingDirectory = Query.FullPath(yearBuiltPredictionPipelineOptions.WorkingDirectory);

            YearBuiltPredictionPipelineOptions = yearBuiltPredictionPipelineOptions;

            if (yearBuiltPredictionPipelineOptions.RunPrediction)
            {
                // The weights are named relative to the runner, not to this application, so the path in the options
                // is not one this process can probe. It is resolved against the runner below and handed to the
                // preflight only when it is actually found: the preflight counts a model it cannot open as a reason
                // not to run at all, so passing a path this process simply cannot see would refuse a run that would
                // have worked. Not finding it here says nothing about the run - the runner repeats the preflight
                // with its own resolution, which is the one that decides.
                string? modelPath = Query.ConsoleAppFilePath(path_ConsoleApp, yearBuiltPredictionPipelineOptions.ModelPath);
                if (modelPath is null && !string.IsNullOrWhiteSpace(yearBuiltPredictionPipelineOptions.ModelPath))
                {
                    Serilog.Modify.Log("The weights at {ModelPath} could not be found from here, so only the interpreter was checked - the runner checks the model itself", yearBuiltPredictionPipelineOptions.ModelPath);
                }

                // Gated here rather than left to the runner: this application is where the operator is standing, so
                // a machine with no interpreter carrying ultralytics can say why in front of them instead of
                // exporting a county of imagery first and then answering with an exit code.
                DiGi.YOLO.Classes.YOLOEnvironmentResult yOLOEnvironmentResult = DiGi.YOLO.Query.YOLOEnvironmentResult(yearBuiltPredictionPipelineOptions.PythonPath, modelPath, yearBuiltPredictionPipelineOptions.WorkingDirectory, cancellationToken);
                if (!yOLOEnvironmentResult.Runnable)
                {
                    string template = "This machine cannot run the detector - {Messages}";
                    string messages = string.Join("; ", yOLOEnvironmentResult.Messages ?? []);
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, template, messages);
                    throw new BackgroundTaskFailureException(Query.RenderRefusal(template, messages));
                }
            }

            string? path_Options = WriteOptions(yearBuiltPredictionPipelineOptions, out string? writeOptionsReason);
            if (string.IsNullOrWhiteSpace(path_Options))
            {
                throw new BackgroundTaskFailureException(writeOptionsReason!);
            }

            // Every line the runner prints is logged already; the sink is kept so a run that fails after launch
            // names the last thing the runner said - and the step that failed - on the task row, rather than the
            // generic "reported failure without an exception" text a bare false return produces.
            ConcurrentQueue<string> lines = new();

            // The whole tree is killed when the task is stopped - the detector is a grandchild - so a batch that was
            // being written may be half written; every step is idempotent and a stopped run is re-runnable.
            YearBuiltPredictionExitCode? yearBuiltPredictionExitCode = await Query.ConsoleAppExitCodeAsync(path_ConsoleApp!, [path_Options!], "Year built prediction", progress, lines.Enqueue, cancellationToken);

            if (yearBuiltPredictionExitCode == YearBuiltPredictionExitCode.Succeeded)
            {
                return true;
            }

            // A cancellation is not a failure with a reason to state: the operator asked for it, and the base wraps
            // it as a cancellation rather than a failure.
            if (yearBuiltPredictionExitCode == YearBuiltPredictionExitCode.Cancelled)
            {
                return false;
            }

            if (yearBuiltPredictionExitCode is not YearBuiltPredictionExitCode yearBuiltPredictionExitCode_Value)
            {
                // The two causes - it never started, or it was lost while it was being watched - are told apart in
                // the log by ConsoleAppExitCodeAsync; the row only has to say which of them ended the run.
                throw new BackgroundTaskFailureException("The Year Built prediction run could not be started, or was lost while it was being watched - see the log beside this application.");
            }

            throw new BackgroundTaskFailureException(FailureMessage(yearBuiltPredictionExitCode_Value, lines));
        }

        /// <summary>
        /// Builds the message a failed Year Built prediction run is reported with, so the task row names the last reason instead of the generic "reported failure without an exception" text.
        /// <para>The runner marks its standard output with prefixes: <c>[ERROR]</c> and <c>[FATAL]</c> for what went wrong, <c>[PROGRESS]</c> for the item counts, <c>[INFO]</c>/<c>[NOTE]</c> for narration, and two spaces plus a dash for each entry under a <c>… failed step(s):</c> header. The last <c>[ERROR]</c>/<c>[FATAL]</c> line is the cause the run ended with, and the step entries under it - when it is such a header - say which step died; everything else is narration the log already carries. Without a named cause the exit code alone is reported.</para>
        /// </summary>
        /// <param name="exitCode">The exit code the runner ended with. It is never <see cref="YearBuiltPredictionExitCode.Succeeded"/> or <see cref="YearBuiltPredictionExitCode.Cancelled"/> here - both are handled by the caller.</param>
        /// <param name="lines">The standard output lines the runner printed, in order.</param>
        /// <returns>The failure message for the task row.</returns>
        internal static string FailureMessage(YearBuiltPredictionExitCode exitCode, IEnumerable<string> lines)
        {
            string? final = null;
            string? step = null;

            foreach (string line in lines)
            {
                if (line.StartsWith("[ERROR] ", StringComparison.Ordinal) || line.StartsWith("[FATAL] ", StringComparison.Ordinal))
                {
                    // Both prefixes are "[X] " - eight characters - so the message starts at the same offset.
                    final = line[(line.IndexOf(' ') + 1)..];
                    step = null;
                }
                else if (line.StartsWith("  - ", StringComparison.Ordinal) && final?.EndsWith("failed step(s):", StringComparison.Ordinal) == true)
                {
                    step = line["  - ".Length..];
                }
            }

            string description = Core.Query.Description(exitCode) ?? exitCode.ToString();

            // The step that died is the most specific thing the row can say, so it goes first, with the header it
            // was listed under; then the last cause alone; then the exit code, which is the whole reason when the
            // runner printed nothing the row can quote.
            if (!string.IsNullOrWhiteSpace(step))
            {
                return string.Format(CultureInfo.InvariantCulture, "The Year Built prediction run did not finish - {0} {1} - {2}. See the log beside this application.", final, step, description);
            }

            if (!string.IsNullOrWhiteSpace(final))
            {
                return string.Format(CultureInfo.InvariantCulture, "The Year Built prediction run did not finish - {0} - {1}. See the log beside this application.", final, description);
            }

            return string.Format(CultureInfo.InvariantCulture, "The Year Built prediction run did not finish - {0}. See the log beside this application.", description);
        }

        private static bool Confirmed(YearBuiltPredictionPipelineOptions yearBuiltPredictionPipelineOptions, List<AdministrativeAreal2DReference> administrativeAreal2DReferences)
        {
            HashSet<int>? countyIds = yearBuiltPredictionPipelineOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                // The dialog refuses an empty selection itself, so this is not the message for it - it is the
                // one thing that must never be confirmed.
                return false;
            }

            // Named back rather than counted. A run is scoped by identifier, the two parts of a multi-part county
            // differ by nothing else, and the identifier is what each written row is filed under.
            List<string> names = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                if (countyIds.Contains(administrativeAreal2DReference.Id))
                {
                    names.Add(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} {1} (id {2})", administrativeAreal2DReference.Code, administrativeAreal2DReference.Name, administrativeAreal2DReference.Id));
                }
            }

            const int count_Listed = 10;

            string listed = names.Count <= count_Listed
                ? string.Join(Environment.NewLine, names)
                : string.Join(Environment.NewLine, names.GetRange(0, count_Listed)) + string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}and {1} more", Environment.NewLine, names.Count - count_Listed);

            string message = string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "This run writes to the stored data of these counties:{0}{0}{1}{0}{0}It updates the detection columns, the year built data and the predicted year built column.{0}{0}Start it?",
                Environment.NewLine,
                listed);

            return System.Windows.MessageBox.Show(message, "Predict year built", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.OK;
        }

        /// <summary>
        /// Writes the options of a scoped run beside its scratch directory - the record of what the run was asked to do - and reports why it could not be written when it could not.
        /// </summary>
        /// <param name="yearBuiltPredictionPipelineOptions">The options the run is handed.</param>
        /// <param name="reason">The refusal to write, worded as it is logged, or null when the options were written.</param>
        /// <returns>The path of the written options file; null when it could not be written.</returns>
        private static string? WriteOptions(YearBuiltPredictionPipelineOptions yearBuiltPredictionPipelineOptions, out string? reason)
        {
            reason = null;

            // Beside the run's own imagery rather than in a temporary folder: it is the only record of what a run
            // was asked to do, it is worth having when the answer looks wrong, and one file per scratch directory
            // needs no cleaning up. It carries no key - the options class deliberately declares none.
            string? directory = yearBuiltPredictionPipelineOptions.ScratchDirectory;
            if (string.IsNullOrWhiteSpace(directory))
            {
                reason = "No scratch directory - the Year Built prediction options have nowhere to be written";
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, reason);
                return null;
            }

            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (yearBuiltPredictionPipelineOptions.ToJsonObject() is not JsonObject jsonObject)
                {
                    reason = "The Year Built prediction options could not be serialized";
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, reason);
                    return null;
                }

                string path = System.IO.Path.Combine(directory, DiGi.GIS.YOLO.UI.Constants.FileName.YearBuiltPredictionPipelineOptions);
                File.WriteAllText(path, jsonObject.ToString());

                return path;
            }
            catch (Exception exception)
            {
                string template = "The Year Built prediction options could not be written into {Directory}";
                reason = Query.RenderRefusal(template, directory);
                Serilog.Modify.Log(exception, template, directory);
                return null;
            }
        }
    }
}
