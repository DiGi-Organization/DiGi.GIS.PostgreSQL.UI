using DiGi.Core.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.GIS.PostgreSQL.UI.Enums;
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
    /// A YOLO detector training run started from the tray: dataset build, label check, training, validation on the Test split and the detector evaluation that gates the new weights, with the inputs asked for through <see cref="YOLOTrainingOptionsWindow"/> each time the task is started.
    /// <para><b>Two scenarios.</b> <see cref="YOLOTrainingScenario.Retrain"/> continues from the deployed model.pt and appends the chosen counties to the existing dataset folder; <see cref="YOLOTrainingScenario.Fresh"/> starts from the pretrained yolo26x.pt and builds the dataset fresh into a new folder. The scenario sets defaults only (<see cref="Create.YOLOTrainingRunOptions(YOLOTrainingScenario, string?, YOLOTrainingRunOptions?)"/>).</para>
    /// <para><b>The run happens in another process</b>, <c>DiGi.GIS.YOLO.UI.ConsoleApp --train</c>, for the same reason as <see cref="UIYearBuiltPredictionsTask"/>: this application publishes self-contained and single-file and does not carry the runner's closure. The options are written beside the run folder - not inside it, because the runner refuses a run whose folder already exists - and that file is the record of what the run was asked to do, so it is never overwritten: a run with the Training step writes <c>&lt;RunName&gt;.YOLOTrainingRunOptions.json</c> (a taken run name is refused before launch), a resume writes <c>&lt;RunName&gt;.resume-&lt;yyyyMMdd_HHmmss&gt;.YOLOTrainingRunOptions.json</c> so the original run's file is untouched, and a run without training always writes <c>yyyyMMdd_HHmmss.YOLOTrainingRunOptions.json</c>, whatever the run name box says (<see cref="Create.YOLOTrainingRunOptionsFile(string, string?, bool, string, DateTimeOffset, bool)"/>).</para>
    /// <para><b>Every path is made absolute here</b>, against the runner for the files its defaults name (the weights, the legacy references, the reports folder) and against this process for the folders the operator typed, so the file the runner reads names what this application checked and named back.</para>
    /// <para><b>The run never overwrites model.pt.</b> The trained weights stay under the run folder and are copied to <c>&lt;ProjectDirectory&gt;\&lt;RunName&gt;\&lt;RunName&gt;.pt</c>; shipping or holding them is a manual decision from the evaluation table (DiGi-Organization/DiGi.GIS.YOLO.UI#12).</para>
    /// <para>The dataset step authorizes with <b>the runner's own key</b>, read from the <c>GIS_WebAPI_Client.conf</c> beside its executable - a run that ends in <see cref="YearBuiltPredictionExitCode.Authorization"/> is usually that file. <b>Stopping the task kills the process tree</b>, the interpreter included; the dataset manifest lets a Re-train run continue an interrupted build (a Start from yolo26x.pt run does not resume, so a fresh build that was stopped is continued as Re-train with the start weights set back to yolo26x.pt), and a training that was stopped or crashed is <b>offered for resume</b>: the dialog names its epoch and ceiling, the preflight repeats the runner's refusals before launch, and the resume continues <c>weights\last.pt</c> at the next epoch in the same folder. Closing this application or a power cut still stops the run - resume shortens the recovery, it does not prevent the interruption. A training that stalls while its runner is alive is also <b>resumed automatically</b>, up to <see cref="YOLOTrainingRunOptions.AutoResumeCount"/> times (the dialog offers 3) and after <see cref="YOLOTrainingRunOptions.InactivityTimeout"/> without output (15 minutes by default); a run that exhausts them fails with the last reason named, and a stop from the tray is never resumed.</para>
    /// </summary>
    public class UIYOLOTrainingTask : ReportableBackgroundTask<long>, IGISPostgreSQLUIObject
    {
        private readonly GISWebAPIManager GISWebAPIManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIYOLOTrainingTask"/> class.
        /// </summary>
        /// <param name="GISWebAPIManager">The <see cref="WebAPI.Classes.GISWebAPIManager"/> instance the county rows behind the dialog are read with. The run itself authorizes with the runner's own key, not with this one.</param>
        public UIYOLOTrainingTask(GISWebAPIManager GISWebAPIManager)
        {
            this.GISWebAPIManager = GISWebAPIManager;
        }

        /// <summary>
        /// Gets or sets the path of the headless runner. When null it is resolved by <see cref="Query.YearBuiltPredictionConsoleAppPath"/> - the training mode is hosted by the same executable as the Year Built prediction.
        /// </summary>
        public string? ConsoleAppPath { get; set; } = null;

        /// <summary>
        /// Gets or sets the scenario the dialog opens with, and which it writes back to when it is closed with OK.
        /// </summary>
        public YOLOTrainingScenario YOLOTrainingScenario { get; set; } = YOLOTrainingScenario.Retrain;

        /// <summary>
        /// Gets or sets the options the dialog opens with, and which it writes back to - with every path made absolute - when a run is confirmed. When null the defaults of <see cref="YOLOTrainingScenario"/> are used, which name no county and no folder and therefore ask for them.
        /// </summary>
        public YOLOTrainingRunOptions? YOLOTrainingRunOptions { get; set; } = null;

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            string? path_ConsoleApp = Query.YearBuiltPredictionConsoleAppPath(ConsoleAppPath);
            if (string.IsNullOrWhiteSpace(path_ConsoleApp))
            {
                string template = "{FileName} was not found beside this application or in the workspace - the YOLO training run cannot be started";
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, template, Constants.FileName.YearBuiltPredictionConsoleApp);
                throw new BackgroundTaskFailureException(Query.RenderRefusal(template, Constants.FileName.YearBuiltPredictionConsoleApp));
            }

            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                string template = "No WPF application is running - the YOLO training options cannot be asked for";
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, template);
                throw new BackgroundTaskFailureException(template);
            }

            HttpClient? httpClient_AdministrativeAreal2D = GISWebAPIManager.CreateHttpClient<AdministrativeAreal2DController>(nameof(AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync), out string? path_AdministrativeAreal2D);
            if (httpClient_AdministrativeAreal2D is null || string.IsNullOrWhiteSpace(path_AdministrativeAreal2D))
            {
                string template = "County references could not be requested - the YOLO training run cannot be scoped";
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, template);
                throw new BackgroundTaskFailureException(template);
            }

            PostOptions postOptions = new() { RequestResult = true };

            // The endpoint is a HttpGet action and its administrativearealtype parameter is not nullable - omitting it binds to Country, not County.
            string requestUri_AdministrativeAreal2D = new UrlBuilder(path_AdministrativeAreal2D).AddParameter("administrativearealtype", (int)AdministrativeArealType.County).ToString();

            PostResponse<List<AdministrativeAreal2DReference>?> postResponse_AdministrativeAreal2DReferences = await DiGi.WebAPI.Query.GetAsync<List<AdministrativeAreal2DReference>>(httpClient_AdministrativeAreal2D, requestUri_AdministrativeAreal2D, postOptions);
            if (postResponse_AdministrativeAreal2DReferences is null || !postResponse_AdministrativeAreal2DReferences.Succeeded || postResponse_AdministrativeAreal2DReferences.Result is not List<AdministrativeAreal2DReference> administrativeAreal2DReferences || administrativeAreal2DReferences.Count == 0)
            {
                string template = "County references could not be retrieved - the YOLO training run cannot be scoped";
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, template);
                throw new BackgroundTaskFailureException(template);
            }

            YOLOTrainingRunOptions? yOLOTrainingRunOptions = null;
            YOLOTrainingScenario yOLOTrainingScenario = YOLOTrainingScenario;

            application.Dispatcher.Invoke(() =>
            {
                YOLOTrainingOptionsWindow yOLOTrainingOptionsWindow = new(YOLOTrainingScenario, YOLOTrainingRunOptions, administrativeAreal2DReferences, path_ConsoleApp);

                if (yOLOTrainingOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                // Made absolute before the confirmation, so the folders it names back are the ones the runner is
                // handed. Raised here rather than in the window, which keeps the window's OK testable.
                YOLOTrainingRunOptions yOLOTrainingRunOptions_Absolute = AbsolutePaths(yOLOTrainingOptionsWindow.YOLOTrainingRunOptions, path_ConsoleApp!);

                if (!Confirmed(yOLOTrainingOptionsWindow.YOLOTrainingScenario, yOLOTrainingRunOptions_Absolute, administrativeAreal2DReferences))
                {
                    return;
                }

                yOLOTrainingScenario = yOLOTrainingOptionsWindow.YOLOTrainingScenario;
                yOLOTrainingRunOptions = yOLOTrainingRunOptions_Absolute;
            });

            // A cancelled dialog, or a declined confirmation, leaves the options of an earlier run as they were -
            // the window works on a copy - and ends the run here rather than starting one nobody scoped.
            if (yOLOTrainingRunOptions is null)
            {
                Serilog.Modify.Log("YOLO training options were cancelled - nothing was run");
                return true;
            }

            YOLOTrainingScenario = yOLOTrainingScenario;
            YOLOTrainingRunOptions = yOLOTrainingRunOptions;

            Preflight(yOLOTrainingRunOptions, cancellationToken);

            // The preflight may have settled the interpreter; kept, so the dialog opens with it next time.
            YOLOTrainingRunOptions = yOLOTrainingRunOptions;

            string? path_Options = WriteOptions(yOLOTrainingRunOptions, out string? writeOptionsReason);
            if (string.IsNullOrWhiteSpace(path_Options))
            {
                throw new BackgroundTaskFailureException(writeOptionsReason!);
            }

            // Every line the runner prints is logged already; the automatic-resume lines are also kept so a run that
            // fails after exhausting its retries names the last reason on the task row, rather than the generic
            // "reported failure without an exception" text a bare false return produces.
            ConcurrentQueue<string> lines = new();

            YearBuiltPredictionExitCode? yearBuiltPredictionExitCode = await Query.ConsoleAppExitCodeAsync(path_ConsoleApp!, ["--train", path_Options!], "YOLO training", progress, lines.Enqueue, cancellationToken);

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

            throw new BackgroundTaskFailureException(FailureMessage(yearBuiltPredictionExitCode, lines));
        }

        /// <summary>
        /// Builds the message a failed YOLO training run is reported with, so the task row names the last reason instead of the generic "reported failure without an exception" text.
        /// <para>The runner prints one line per automatic resume (<c>... - automatic resume N of M</c>), a closing summary (<c>[NOTE] Resumed automatically N time(s) ...</c>) and, when a stall or a crash ends the run with no resume left, the cause of that last attempt (<c>Training stalled at epoch E ... - no automatic resume left</c>, or <c>- automatic resume is off</c>). That last cause is named first, with the exit code and the summary after it; without it the summary or the last resume line is named with the exit code, and otherwise the exit code alone is.</para>
        /// </summary>
        /// <param name="yearBuiltPredictionExitCode">The exit code the runner ended with.</param>
        /// <param name="lines">The standard output lines the runner printed.</param>
        /// <returns>The failure message for the task row.</returns>
        private static string FailureMessage(YearBuiltPredictionExitCode? yearBuiltPredictionExitCode, ConcurrentQueue<string> lines)
        {
            string? description = yearBuiltPredictionExitCode is YearBuiltPredictionExitCode yearBuiltPredictionExitCode_Value
                ? Core.Query.Description(yearBuiltPredictionExitCode_Value) ?? yearBuiltPredictionExitCode_Value.ToString()
                : null;

            string? summary = null;
            string? final = null;
            List<string> resumes = [];
            foreach (string line in lines)
            {
                if (line.StartsWith("[NOTE] Resumed automatically", StringComparison.Ordinal))
                {
                    summary = line;
                }
                else if (line.Contains("- no automatic resume left", StringComparison.OrdinalIgnoreCase) || line.Contains("- automatic resume is off", StringComparison.OrdinalIgnoreCase))
                {
                    final = line.StartsWith("[NOTE] ", StringComparison.Ordinal) ? line["[NOTE] ".Length..] : line;
                }
                else if (line.Contains("- automatic resume ", StringComparison.OrdinalIgnoreCase))
                {
                    resumes.Add(line);
                }
            }

            string reason = description is null ? "unknown exit code" : description;

            // The attempt that ended the run is named first: it is the reason the run failed, and the earlier
            // resumes, when there were any, follow as the summary.
            if (!string.IsNullOrWhiteSpace(final))
            {
                return string.IsNullOrWhiteSpace(summary)
                    ? string.Format(CultureInfo.InvariantCulture, "The YOLO training run did not finish - {0} - {1}. See the log beside this application.", final, reason)
                    : string.Format(CultureInfo.InvariantCulture, "The YOLO training run did not finish - {0} - {1}. {2}. See the log beside this application.", final, reason, summary);
            }

            if (!string.IsNullOrWhiteSpace(summary))
            {
                return string.Format(CultureInfo.InvariantCulture, "The YOLO training run did not finish - {0} - {1}. See the log beside this application.", summary, reason);
            }

            if (resumes.Count != 0)
            {
                return string.Format(CultureInfo.InvariantCulture, "The YOLO training run did not finish after {0} automatic resume(s): {1} - {2}. See the log beside this application.", resumes.Count, resumes[^1], reason);
            }

            return string.Format(CultureInfo.InvariantCulture, "The YOLO training run did not finish - {0}. See the log beside this application.", reason);
        }

        private static YOLOTrainingRunOptions AbsolutePaths(YOLOTrainingRunOptions yOLOTrainingRunOptions, string path_ConsoleApp)
        {
            YOLOTrainingRunOptions result = new(yOLOTrainingRunOptions);

            // The folders the operator typed are resolved against this process, which is where they were typed. The
            // files the runner's defaults name are resolved against the runner: found where it would find them, or
            // placed where its deployed layout puts them, so a refusal names a real place.
            string? RunnerFilePath(string? path)
            {
                return Query.ConsoleAppFilePath(path_ConsoleApp, path) ?? Query.ConsoleAppDeployedPath(path_ConsoleApp, path);
            }

            result.ProjectDirectory = Query.FullPath(result.ProjectDirectory);
            result.WorkingDirectory = Query.FullPath(result.WorkingDirectory);
            result.StartWeightsPath = RunnerFilePath(result.StartWeightsPath);

            // A bare interpreter name is searched on PATH by the runner; only a path is made absolute.
            if (result.PythonPath is string pythonPath && (pythonPath.Contains(System.IO.Path.DirectorySeparatorChar) || pythonPath.Contains(System.IO.Path.AltDirectorySeparatorChar)))
            {
                result.PythonPath = Query.FullPath(pythonPath);
            }

            if (result.DatasetOptions is YOLOTrainingDatasetOptions yOLOTrainingDatasetOptions)
            {
                yOLOTrainingDatasetOptions.OutputDirectory = Query.FullPath(yOLOTrainingDatasetOptions.OutputDirectory);
                yOLOTrainingDatasetOptions.WorkingDirectory = Query.FullPath(yOLOTrainingDatasetOptions.WorkingDirectory);
                yOLOTrainingDatasetOptions.PythonPath = result.PythonPath;
                yOLOTrainingDatasetOptions.ModelPath = RunnerFilePath(yOLOTrainingDatasetOptions.ModelPath);
                yOLOTrainingDatasetOptions.LegacyReferencesFilePath = RunnerFilePath(yOLOTrainingDatasetOptions.LegacyReferencesFilePath);
                yOLOTrainingDatasetOptions.ReportsDirectory = Query.ConsoleAppDeployedPath(path_ConsoleApp, yOLOTrainingDatasetOptions.ReportsDirectory);

                if (yOLOTrainingDatasetOptions.WeightsPaths is List<string> weightsPaths)
                {
                    List<string> weightsPaths_Absolute = [];
                    foreach (string weightsPath in weightsPaths)
                    {
                        if (RunnerFilePath(weightsPath) is string weightsPath_Absolute)
                        {
                            weightsPaths_Absolute.Add(weightsPath_Absolute);
                        }
                    }

                    yOLOTrainingDatasetOptions.WeightsPaths = weightsPaths_Absolute;
                }
            }

            return result;
        }

        private static bool Confirmed(YOLOTrainingScenario yOLOTrainingScenario, YOLOTrainingRunOptions yOLOTrainingRunOptions, List<AdministrativeAreal2DReference> administrativeAreal2DReferences)
        {
            YOLOTrainingDatasetOptions? yOLOTrainingDatasetOptions = yOLOTrainingRunOptions.DatasetOptions;

            HashSet<int>? countyIds = yOLOTrainingDatasetOptions?.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return false;
            }

            List<string> names = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                if (countyIds.Contains(administrativeAreal2DReference.Id))
                {
                    names.Add(string.Format(CultureInfo.InvariantCulture, "{0} {1} (id {2})", administrativeAreal2DReference.Code, administrativeAreal2DReference.Name, administrativeAreal2DReference.Id));
                }
            }

            const int count_Listed = 10;

            string listed = names.Count <= count_Listed
                ? string.Join(Environment.NewLine, names)
                : string.Join(Environment.NewLine, names.GetRange(0, count_Listed)) + string.Format(CultureInfo.InvariantCulture, "{0}and {1} more", Environment.NewLine, names.Count - count_Listed);

            List<string> steps = [];
            foreach (YOLOTrainingStep yOLOTrainingStep in yOLOTrainingRunOptions.Steps ?? [])
            {
                steps.Add(Core.Query.Description(yOLOTrainingStep) ?? yOLOTrainingStep.ToString());
            }

            string runDirectory = string.IsNullOrWhiteSpace(yOLOTrainingRunOptions.ProjectDirectory) || string.IsNullOrWhiteSpace(yOLOTrainingRunOptions.RunName)
                ? "(no training)"
                : System.IO.Path.Combine(yOLOTrainingRunOptions.ProjectDirectory, yOLOTrainingRunOptions.RunName);

            string message;

            if (yOLOTrainingRunOptions.ResumeTraining)
            {
                // Read here rather than carried from the dialog: the confirmation names what the run folder actually
                // holds, so an edit to the names after the offer was shown is named back with its current numbers.
                string? workingDirectory_Checkpoint = yOLOTrainingRunOptions.WorkingDirectory ?? yOLOTrainingDatasetOptions?.WorkingDirectory ?? yOLOTrainingDatasetOptions?.OutputDirectory;
                DiGi.YOLO.Classes.YOLOCheckpointInformation? yOLOCheckpointInformation_Resume = Query.InterruptedYOLOTrainingRun(yOLOTrainingRunOptions.ProjectDirectory, yOLOTrainingRunOptions.RunName, yOLOTrainingRunOptions.PythonPath ?? yOLOTrainingDatasetOptions?.PythonPath, workingDirectory_Checkpoint);

                message = string.Format(
                    CultureInfo.InvariantCulture,
                    "This is a RESUME of {1}{0}Steps: {2}{0}{0}Counties:{0}{3}{0}{0}Checkpoint: epoch {4} of {5} completed, best fitness {6}{0}Dataset: {7}{0}{0}It continues at {8}; a resumed run is not bit-identical to an uninterrupted one.{0}A power cut or closing this application still stops the run.{0}{0}Start it?",
                    Environment.NewLine,
                    runDirectory,
                    string.Join(", ", steps),
                    listed,
                    yOLOCheckpointInformation_Resume?.Epoch?.ToString(CultureInfo.InvariantCulture) ?? "(unknown)",
                    yOLOCheckpointInformation_Resume?.Epochs?.ToString(CultureInfo.InvariantCulture) ?? "(unknown)",
                    yOLOCheckpointInformation_Resume?.BestFitness?.ToString("0.000", CultureInfo.InvariantCulture) ?? "(unknown)",
                    yOLOCheckpointInformation_Resume?.DataPath ?? yOLOTrainingDatasetOptions?.OutputDirectory ?? "(none)",
                    yOLOCheckpointInformation_Resume?.Epoch is int epoch_Completed ? string.Format(CultureInfo.InvariantCulture, "epoch {0}", epoch_Completed + 1) : "the next epoch");
            }
            else
            {
                message = string.Format(
                    CultureInfo.InvariantCulture,
                    "Scenario: {1}{0}Steps: {2}{0}{0}Counties:{0}{3}{0}{0}Start weights: {4}{0}Dataset: {5} ({6}){0}Run folder: {7}{0}{0}The trained weights are written under the run folder only - model.pt is never overwritten.{0}{0}Start it?",
                    Environment.NewLine,
                    Core.Query.Description(yOLOTrainingScenario) ?? yOLOTrainingScenario.ToString(),
                    string.Join(", ", steps),
                    listed,
                    yOLOTrainingRunOptions.StartWeightsPath ?? "(none)",
                    yOLOTrainingDatasetOptions?.OutputDirectory ?? "(none)",
                    yOLOTrainingDatasetOptions?.Resume == true ? "appended to when it exists" : "built fresh; an existing dataset is refused",
                    runDirectory);
            }

            return System.Windows.MessageBox.Show(message, "Train YOLO detector", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.OK;
        }

        /// <summary>
        /// Logs a pre-launch refusal the operator is refused with and throws it, so the task row shows the reason instead of the generic "reported failure without an exception" text; it never returns normally.
        /// </summary>
        /// <param name="name">The field the refusal is about.</param>
        /// <param name="message">The reason, with a Serilog property placeholder for the value, when there is one.</param>
        /// <param name="value">The value the reason names, when there is one.</param>
        [System.Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void Refuse(string name, string message, string? value = null)
        {
            string template = "YOLO training refused - {Name}: " + message;
            Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, template, name, value ?? "(none)");
            throw new BackgroundTaskFailureException(Query.RenderRefusal(template, name, value ?? "(none)"));
        }

        private static void Preflight(YOLOTrainingRunOptions yOLOTrainingRunOptions, CancellationToken cancellationToken)
        {
            // The runner repeats most of these checks; they are made here as well because this application is where
            // the operator is standing, and a refusal said now is worth more than an exit code later. The label check
            // detector and the gate weights are checked only here: the runner finds them missing after the dataset
            // step, which can take hours.
            //
            // One value is settled rather than checked: an empty interpreter. The runner refuses training and
            // validation without one, while the environment check below searches PATH for it - so the interpreter
            // that passed the check is written into the options, and the run uses exactly what was checked.
            List<YOLOTrainingStep> yOLOTrainingSteps = yOLOTrainingRunOptions.Steps ?? [];
            bool dataset = yOLOTrainingSteps.Count == 0 || yOLOTrainingSteps.Contains(YOLOTrainingStep.Dataset);
            bool labelCheck = yOLOTrainingSteps.Count == 0 || yOLOTrainingSteps.Contains(YOLOTrainingStep.LabelCheck);
            bool train = yOLOTrainingSteps.Count == 0 || yOLOTrainingSteps.Contains(YOLOTrainingStep.Train);
            bool validate = yOLOTrainingSteps.Count == 0 || yOLOTrainingSteps.Contains(YOLOTrainingStep.Validate);
            bool evaluate = yOLOTrainingSteps.Count == 0 || yOLOTrainingSteps.Contains(YOLOTrainingStep.Evaluate);

            // A checkpoint records its dataset path as ultralytics saw it, which may be relative to the working
            // directory the run executed in; the same directory resolves it here.
            static string? ResolveCheckpointDataPath(string? dataPath, string? baseDirectory)
            {
                if (string.IsNullOrWhiteSpace(dataPath))
                {
                    return null;
                }

                if (System.IO.Path.IsPathRooted(dataPath) || string.IsNullOrWhiteSpace(baseDirectory))
                {
                    return dataPath;
                }

                return System.IO.Path.Combine(baseDirectory, dataPath);
            }

            static bool PathsEqual(string? left, string? right)
            {
                if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right))
                {
                    return false;
                }

                try
                {
                    return string.Equals(System.IO.Path.GetFullPath(left), System.IO.Path.GetFullPath(right), StringComparison.OrdinalIgnoreCase);
                }
                catch
                {
                    return false;
                }
            }

            YOLOTrainingDatasetOptions? yOLOTrainingDatasetOptions = yOLOTrainingRunOptions.DatasetOptions;
            if (yOLOTrainingDatasetOptions is null || string.IsNullOrWhiteSpace(yOLOTrainingDatasetOptions.OutputDirectory) || !System.IO.Path.IsPathRooted(yOLOTrainingDatasetOptions.OutputDirectory))
            {
                Refuse(nameof(YOLOTrainingDatasetOptions.OutputDirectory), "the dataset directory has to be an absolute path");
            }

            if (!Query.IsYOLOTrainingStallOptionsValid(yOLOTrainingRunOptions.AutoResumeCount, yOLOTrainingRunOptions.InactivityTimeout, out string? stallReason))
            {
                // The limit is the only value checked here that can be null, so a non-null limit is the field to name
                // when one of the two is out of range.
                string name_Option = yOLOTrainingRunOptions.InactivityTimeout is null
                    ? nameof(YOLOTrainingRunOptions.AutoResumeCount)
                    : nameof(YOLOTrainingRunOptions.InactivityTimeout);

                Refuse(name_Option, stallReason!);
            }

            string? startWeightsPath = yOLOTrainingRunOptions.StartWeightsPath;
            if (train || validate)
            {
                if (string.IsNullOrWhiteSpace(startWeightsPath) || !startWeightsPath!.EndsWith(".pt", StringComparison.OrdinalIgnoreCase) || !File.Exists(startWeightsPath))
                {
                    Refuse(nameof(YOLOTrainingRunOptions.StartWeightsPath), "the start weights {Path} are not an existing .pt file", startWeightsPath);
                }
            }

            if (train)
            {
                string? projectDirectory = yOLOTrainingRunOptions.ProjectDirectory;
                if (string.IsNullOrWhiteSpace(projectDirectory) || !System.IO.Path.IsPathRooted(projectDirectory))
                {
                    Refuse(nameof(YOLOTrainingRunOptions.ProjectDirectory), "the project directory {Path} has to be an absolute path", projectDirectory);
                }

                if (DiGi.YOLO.Query.IsInsideModelsDirectory(projectDirectory))
                {
                    Refuse(nameof(YOLOTrainingRunOptions.ProjectDirectory), "the project directory {Path} is inside a YOLO\\models folder, where the frozen weights live", projectDirectory);
                }

                string? runName = yOLOTrainingRunOptions.RunName;
                if (string.IsNullOrWhiteSpace(runName) || runName!.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0 || string.Equals(runName, System.IO.Path.GetFileNameWithoutExtension(Constants.FileName.Model), StringComparison.OrdinalIgnoreCase))
                {
                    Refuse(nameof(YOLOTrainingRunOptions.RunName), "{RunName} has to be a plain file name other than model", runName);
                }

                string runDirectory = System.IO.Path.Combine(projectDirectory!, runName!);
                string path_Completed = System.IO.Path.Combine(runDirectory, runName + ".pt");

                if (yOLOTrainingRunOptions.ResumeTraining)
                {
                    // The runner repeats these refusals; they are made here too, so the operator is told before the
                    // run is written and launched rather than reading an exit code later.
                    if (dataset)
                    {
                        Refuse(nameof(YOLOTrainingRunOptions.ResumeTraining), "a resume never rebuilds the dataset it was trained on; remove the Dataset step");
                    }

                    if (File.Exists(path_Completed))
                    {
                        Refuse(nameof(YOLOTrainingRunOptions.ResumeTraining), "the run {Path} completed; a completed run is not resumed", path_Completed);
                    }

                    string path_Last = System.IO.Path.Combine(runDirectory, DiGi.GIS.YOLO.UI.Constants.DirectoryName.Weights, DiGi.GIS.YOLO.UI.Constants.FileName.LastWeights);
                    if (!Directory.Exists(runDirectory) || !File.Exists(path_Last))
                    {
                        Refuse(nameof(YOLOTrainingRunOptions.ResumeTraining), "nothing to resume - {Path} has no weights\\last.pt", runDirectory);
                    }

                    string? workingDirectory_Checkpoint = yOLOTrainingRunOptions.WorkingDirectory ?? yOLOTrainingDatasetOptions.WorkingDirectory ?? yOLOTrainingDatasetOptions.OutputDirectory;
                    DiGi.YOLO.Classes.YOLOCheckpointInformation? yOLOCheckpointInformation_Resume = DiGi.YOLO.Query.YOLOCheckpointInformation(path_Last, yOLOTrainingRunOptions.PythonPath ?? yOLOTrainingDatasetOptions.PythonPath, workingDirectory_Checkpoint, cancellationToken);

                    if (yOLOCheckpointInformation_Resume is null)
                    {
                        Refuse(nameof(DiGi.YOLO.Query.YOLOCheckpointInformation), "the checkpoint {Path} could not be read", path_Last);
                    }

                    if (yOLOCheckpointInformation_Resume.Finished)
                    {
                        Refuse(nameof(DiGi.YOLO.Query.YOLOCheckpointInformation), "the checkpoint {Path} is finished; nothing to resume", path_Last);
                    }

                    string? path_Data = ResolveCheckpointDataPath(yOLOCheckpointInformation_Resume.DataPath, workingDirectory_Checkpoint);
                    if (string.IsNullOrWhiteSpace(path_Data) || !File.Exists(path_Data))
                    {
                        Refuse(nameof(DiGi.YOLO.Query.YOLOCheckpointInformation), "the dataset the checkpoint {Path} records is missing or was not named", path_Last);
                    }

                    if (!string.Equals(yOLOCheckpointInformation_Resume.Name, runName, StringComparison.OrdinalIgnoreCase) || !PathsEqual(yOLOCheckpointInformation_Resume.Project, projectDirectory))
                    {
                        Refuse(nameof(DiGi.YOLO.Query.YOLOCheckpointInformation), "the checkpoint {Path} records a different run folder; it was moved or renamed", path_Last);
                    }
                }
                else if (Directory.Exists(runDirectory) || File.Exists(path_Completed))
                {
                    Refuse(nameof(YOLOTrainingRunOptions.RunName), "the run {Path} already exists; choose a new run name", runDirectory);
                }
            }

            if (dataset && (string.IsNullOrWhiteSpace(yOLOTrainingDatasetOptions.LegacyReferencesFilePath) || !File.Exists(yOLOTrainingDatasetOptions.LegacyReferencesFilePath)))
            {
                Refuse(nameof(YOLOTrainingDatasetOptions.LegacyReferencesFilePath), "the legacy references file {Path} was not found beside the runner", yOLOTrainingDatasetOptions.LegacyReferencesFilePath);
            }

            if (labelCheck && (string.IsNullOrWhiteSpace(yOLOTrainingDatasetOptions.ModelPath) || !File.Exists(yOLOTrainingDatasetOptions.ModelPath)))
            {
                Refuse(nameof(YOLOTrainingDatasetOptions.ModelPath), "the label check detector {Path} was not found beside the runner", yOLOTrainingDatasetOptions.ModelPath);
            }

            if (evaluate)
            {
                List<string> weightsPaths = yOLOTrainingDatasetOptions.WeightsPaths ?? [];
                foreach (string weightsPath in weightsPaths)
                {
                    if (!File.Exists(weightsPath))
                    {
                        Refuse(nameof(YOLOTrainingDatasetOptions.WeightsPaths), "the gate weights {Path} were not found", weightsPath);
                    }
                }

                // Without training there is no new weights file to add, so an empty list would evaluate nothing.
                if (!train && weightsPaths.Count == 0)
                {
                    Refuse(nameof(YOLOTrainingDatasetOptions.WeightsPaths), "the evaluation without training needs at least one gate weights file");
                }
            }

            if (train || validate || labelCheck || evaluate)
            {
                // Checked with the start weights when there are any to check, so an ultralytics too old for the
                // checkpoint is refused here too; the label check and the evaluation alone need only the interpreter.
                string? modelPath = train || validate ? startWeightsPath : null;
                string? workingDirectory = yOLOTrainingRunOptions.WorkingDirectory ?? yOLOTrainingDatasetOptions.WorkingDirectory;

                DiGi.YOLO.Classes.YOLOEnvironmentResult yOLOEnvironmentResult = DiGi.YOLO.Query.YOLOEnvironmentResult(yOLOTrainingRunOptions.PythonPath ?? yOLOTrainingDatasetOptions.PythonPath, modelPath, workingDirectory, cancellationToken);
                if (!yOLOEnvironmentResult.Runnable)
                {
                    Refuse(nameof(YOLOTrainingRunOptions.PythonPath), "this machine cannot run the detector - {Messages}", string.Join("; ", yOLOEnvironmentResult.Messages ?? []));
                }

                if (string.IsNullOrWhiteSpace(yOLOTrainingRunOptions.PythonPath) && !string.IsNullOrWhiteSpace(yOLOEnvironmentResult.PythonPath))
                {
                    yOLOTrainingRunOptions.PythonPath = yOLOEnvironmentResult.PythonPath;
                    yOLOTrainingDatasetOptions.PythonPath = yOLOEnvironmentResult.PythonPath;

                    Serilog.Modify.Log("YOLO training uses the interpreter found on PATH - {PythonPath}", yOLOEnvironmentResult.PythonPath!);
                }
            }
        }

        private static string? WriteOptions(YOLOTrainingRunOptions yOLOTrainingRunOptions, out string? reason)
        {
            reason = null;
            string? directory = yOLOTrainingRunOptions.ProjectDirectory ?? yOLOTrainingRunOptions.DatasetOptions?.OutputDirectory;
            if (string.IsNullOrWhiteSpace(directory))
            {
                reason = "No project or dataset directory - the YOLO training options have nowhere to be written";
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, reason);
                return null;
            }

            List<YOLOTrainingStep> yOLOTrainingSteps = yOLOTrainingRunOptions.Steps ?? [];
            bool train = yOLOTrainingSteps.Count == 0 || yOLOTrainingSteps.Contains(YOLOTrainingStep.Train);

            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (yOLOTrainingRunOptions.ToJsonObject() is not JsonObject jsonObject)
                {
                    reason = "The YOLO training options could not be serialized";
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, reason);
                    return null;
                }

                return Create.YOLOTrainingRunOptionsFile(directory, yOLOTrainingRunOptions.RunName, train, jsonObject.ToString(), DateTimeOffset.Now, yOLOTrainingRunOptions.ResumeTraining);
            }
            catch (Exception exception)
            {
                string template = "The YOLO training options could not be written into {Directory}";
                reason = Query.RenderRefusal(template, directory);
                Serilog.Modify.Log(exception, template, directory);
                return null;
            }
        }
    }
}
