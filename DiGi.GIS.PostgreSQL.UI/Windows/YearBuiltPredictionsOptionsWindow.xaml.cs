using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.YOLO.UI.Classes;
using DiGi.UI.WPF.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for YearBuiltPredictionsOptionsWindow.xaml
    /// <para>Asks for the scope of one Year Built prediction run and nothing else: the counties, where its imagery goes, which interpreter runs the detector, and how hard the export leans on the server.</para>
    /// <para><b>A tray run has one shape - the full six step flow - and it writes.</b> The steps are not offered because they are not a choice here: DiGi-Organization/DiGi.GIS.YOLO.UI#8 made export, detector, detection write, score, history write and column write a single run per county precisely so that no step could be left out of sequence, and it decided that the granular flags stay on the options class and the console app for hand-driven diagnostics while the tray driven flow collapses them. Eight checkboxes offered two hundred and fifty six combinations, of which three were real - and the rest failed late, after the half hour of export and the hour and a half of inference had already been paid for. The OK handler writes all six on, so the run the operator gets is the run the standing recipe describes.</para>
    /// <para><b>What settles the model is deliberately not here either.</b> The weights, the confidence threshold, the year range and the radiuses all decide what the regressor is handed, and a value that disagrees with what it was trained on is, for the year range and the radiuses, checked against the model's stated trained contract before the run reads a county - narrower is refused, wider is warned (DiGi-Organization/DiGi.GIS.ML#6) - while the weights and the confidence threshold still score without failing. They belong to the deployment and to the options file rather than to a dialog opened before every run. The working directory and the two batch sizes are out for a different reason: none of the three is a choice - see the comment on the candidate in <c>TryReadOptions</c>.</para>
    /// <para>The scratch cleanup is the one flag that survives, because its reason is about this run rather than about the sequence: a cancelled county is cleaned, so a run that is meant to be interrupted has to be able to say so beforehand.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#74), so a run repeated after the tray was restarted is not typed again. Importing fills the controls; the step flags and the resume switch OK always writes keep whatever the file carried until OK runs. Nothing runs until OK, and OK validates the imported values like typed ones.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were, and every member the window has no control for carries through untouched.</para>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "The Closed handler cancels and disposes the token source.")]
    public partial class YearBuiltPredictionsOptionsWindow : Window
    {
        // What a blank scratch directory means: the dialog opens with it and OK substitutes it for an emptied box. A relative
        // path is the ordinary case - the task makes it absolute before the run is launched.
        private const string DefaultScratchDirectory = "scratch";

        private readonly YearBuiltPredictionPipelineOptions yearBuiltPredictionPipelineOptions;
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private Dictionary<int, double>? coverageFactors;

        /// <summary>
        /// Initializes a new instance of the <see cref="YearBuiltPredictionsOptionsWindow"/> class.
        /// </summary>
        /// <param name="yearBuiltPredictionPipelineOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own - a run names every part of a county so that each written row is filed under the part its reference belongs to.</param>
        /// <param name="coverageFactorsAsync">Reads the predicted year built coverage of each county part, keyed by part id, for the markers. Started here and never awaited before the window opens: the county list is shown at once and its markers are filled in when the read completes, whenever that is, with a progress bar in the bottom-left corner running until it does. Closing the window cancels it. When null no coverage is read, nothing is marked and the progress bar is never shown.</param>
        public YearBuiltPredictionsOptionsWindow(YearBuiltPredictionPipelineOptions? yearBuiltPredictionPipelineOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences, Func<CancellationToken, Task<Dictionary<int, double>?>>? coverageFactorsAsync = null)
        {
            InitializeComponent();

            this.yearBuiltPredictionPipelineOptions = yearBuiltPredictionPipelineOptions is null ? new YearBuiltPredictionPipelineOptions() : new YearBuiltPredictionPipelineOptions(yearBuiltPredictionPipelineOptions);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
            SetControls(this.yearBuiltPredictionPipelineOptions);

            // A read still running when the window closes is no longer wanted by anyone; cancelling it also stops the query
            // behind it on the server.
            // Every way out raises Closed - OK included, because once the counties are chosen nobody needs the markers.
            // Disposed straight after: the read took its token when it started, and Cancel has already reached it.
            Closed += (sender, e) =>
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            };

            if (coverageFactorsAsync is not null)
            {
                Label_Coverage.Content = "Reading prediction coverage...";
                ProgressBar_Coverage.Visibility = Visibility.Visible;
                _ = ReadCoverageFactorsAsync(coverageFactorsAsync);
            }
        }

        /// <summary>
        /// Sets the predicted year built coverage the county markers are drawn from, and redraws the county list in place.
        /// <para>The selection and the scroll position are kept: the coverage normally arrives after the window is open, possibly while counties are being picked. Null means the coverage could not be read, which marks nothing and says so - never a false "not run".</para>
        /// <para>Either way the coverage is settled, so the progress bar in the bottom-left corner is hidden.</para>
        /// </summary>
        /// <param name="coverageFactors">The share of each county part's buildings carrying a predicted year built, keyed by part id, or null when it could not be read.</param>
        public void SetCoverageFactors(IReadOnlyDictionary<int, double>? coverageFactors)
        {
            this.coverageFactors = coverageFactors is null ? null : new Dictionary<int, double>(coverageFactors);

            ProgressBar_Coverage.Visibility = Visibility.Collapsed;

            Label_Coverage.Content = coverageFactors is null
                ? "Prediction coverage could not be read - no county is marked"
                : string.Format(CultureInfo.CurrentCulture, "{0} - more than {1} % of the county's buildings already carry a predicted year built", Query.PredictedYearBuiltCoverageMarker(double.PositiveInfinity), Constants.YearBuiltPrediction.CoverageMarkerThreshold * 100);

            ListBoxControl_Counties.RefreshItems();
        }

        private async Task ReadCoverageFactorsAsync(Func<CancellationToken, Task<Dictionary<int, double>?>> coverageFactorsAsync)
        {
            try
            {
                Dictionary<int, double>? coverageFactors_Read;
                try
                {
                    coverageFactors_Read = await coverageFactorsAsync(cancellationTokenSource.Token);
                }
                catch (OperationCanceledException) when (cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception exception)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Prediction coverage could not be read: {Message}", exception.Message);
                    coverageFactors_Read = null;
                }

                // Resumed on the user interface thread, the context the read was started from. A window closed meanwhile is left alone.
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }

                SetCoverageFactors(coverageFactors_Read);
            }
            finally
            {
                // However the read ended - markers, a failure, or a cancellation that returns before SetCoverageFactors - nothing is
                // being read any more.
                ProgressBar_Coverage.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public YearBuiltPredictionPipelineOptions YearBuiltPredictionPipelineOptions
        {
            get
            {
                return yearBuiltPredictionPipelineOptions;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: a half-prepared window can be saved and finished after the next import. The
            // concurrency floor and the county scope are OK's gate, not the file's; a blank scratch box is
            // written as no directory (the window shows the default in it, and OK substitutes the default on
            // read-back, exactly as when typed); the step flags and the resume switch are written as the copy
            // holds them - OK is what forces them on. What is refused is what could not be written back as
            // typed: a concurrency box that does not hold a whole number.
            if (!TryReadOptions(false, out YearBuiltPredictionPipelineOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export year built settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "YearBuilt_Prediction_Settings.json"
            };

            if (saveFileDialog.ShowDialog(this) != true)
            {
                return;
            }

            string? json = Core.Convert.ToSystem_String(options);
            if (json is null)
            {
                MessageBox.Show(this, "The settings could not be converted to JSON - nothing was written.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                File.WriteAllText(saveFileDialog.FileName, json, Encoding.UTF8);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                MessageBox.Show(this, $"The settings could not be written to {saveFileDialog.FileName}: {exception.Message}", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_ImportSettings_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "Import year built settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1
            };

            if (openFileDialog.ShowDialog(this) != true)
            {
                return;
            }

            string json;
            try
            {
                json = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                MessageBox.Show(this, $"The settings could not be read from {openFileDialog.FileName}: {exception.Message}", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (TryReadSettings(json, out string? message_Read) is not YearBuiltPredictionPipelineOptions yearBuiltPredictionPipelineOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Only the controls are filled: the step flags and the resume switch keep what the file carried
            // until OK runs, and OK writes what it always writes. The coverage markers are a display-only
            // window field and never reach the file.
            SetControls(yearBuiltPredictionPipelineOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected - the
            // usual sign the settings came from another machine.
            HashSet<int> countyIds = yearBuiltPredictionPipelineOptions_Imported.CountyIds ?? [];
            if (countyIds.Count != 0)
            {
                HashSet<int> countyIds_Listed = [];
                foreach (AdministrativeAreal2DReference administrativeAreal2DReference in ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>(false) ?? [])
                {
                    countyIds_Listed.Add(administrativeAreal2DReference.Id);
                }

                List<int> countyIds_Missing = [];
                foreach (int countyId in countyIds)
                {
                    if (!countyIds_Listed.Contains(countyId))
                    {
                        countyIds_Missing.Add(countyId);
                    }
                }

                countyIds_Missing.Sort();
                if (countyIds_Missing.Count != 0)
                {
                    MessageBox.Show(this, string.Create(CultureInfo.CurrentCulture, $"{countyIds_Missing.Count} county part(s) of the file are not in the list and were not selected: {string.Join(", ", countyIds_Missing)}"), Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (TryGetOptions(out string? message))
            {
                DialogResult = true;
                Close();
                return;
            }

            MessageBox.Show(message ?? "The options cannot start a run.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Reads every control into the window's options and checks them, without closing the window or showing a message - the part of OK a fact can drive, because a message box would hang it.
        /// <para>The options are written only when every control reads and passes the rules OK has always applied - a concurrency of at least one, at least one county - and OK's run shape comes with them: the six step flags and the resume switch are written on, whatever the file or the previous run carried. A refusal leaves the options as they were. The members the window has no control for keep the values it was opened with.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out YearBuiltPredictionPipelineOptions? options, out message) || options is null)
            {
                return false;
            }

            yearBuiltPredictionPipelineOptions.ScratchDirectory = options.ScratchDirectory;
            yearBuiltPredictionPipelineOptions.CountyIds = options.CountyIds;
            yearBuiltPredictionPipelineOptions.MaxConcurrentRequests = options.MaxConcurrentRequests;
            yearBuiltPredictionPipelineOptions.PythonPath = options.PythonPath;
            yearBuiltPredictionPipelineOptions.CleanScratchDirectory = options.CleanScratchDirectory;

            // The full six step flow, written rather than left to the defaults: the three write flags default
            // to false, and the window is handed the previous run's options, so a tray run that did not set
            // them here would read a county, score it and store nothing while reporting that it had run.
            // Resume is fixed on rather than offered - see the class docs.
            yearBuiltPredictionPipelineOptions.ExportImages = options.ExportImages;
            yearBuiltPredictionPipelineOptions.RunPrediction = options.RunPrediction;
            yearBuiltPredictionPipelineOptions.Score = options.Score;
            yearBuiltPredictionPipelineOptions.UpdateDetections = options.UpdateDetections;
            yearBuiltPredictionPipelineOptions.UpdateYearBuiltData = options.UpdateYearBuiltData;
            yearBuiltPredictionPipelineOptions.UpdatePredictedYearBuilt = options.UpdatePredictedYearBuilt;
            yearBuiltPredictionPipelineOptions.Resume = options.Resume;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) applies the rules the window has always applied - a concurrency of at least one, at least one county - finishes with those messages, substitutes the default scratch directory for a blank box, and forces the six step flags and the resume switch on: that is the run shape of a tray run. Export reads the same controls without those requirements, so a half-prepared window can be saved: a blank scratch box writes no directory (the window shows the default in it, and OK substitutes it on read-back), no county writes no county list, and the step flags travel as the copy holds them. What is refused on both paths is what could not be written back as typed: a concurrency box that does not hold a whole number. The members with no control - the year range, the radiuses, the confidence, the weights, the batch sizes - carry through the candidate untouched.</para>
        /// </summary>
        /// <param name="complete">True for OK: every rule applies and the run shape is forced. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out YearBuiltPredictionPipelineOptions? options, out string? message)
        {
            options = null;
            message = null;

            // A blank box is "the default" on OK, not refused - the runner still refuses an empty directory,
            // which is the backstop for a hand-written options file. An export writes the blank box as no
            // directory rather than as the default: what is shown is the default, what is typed is blank.
            string? scratchDirectory = TextBoxControl_ScratchDirectory.Value;
            if (string.IsNullOrWhiteSpace(scratchDirectory))
            {
                scratchDirectory = complete ? DefaultScratchDirectory : null;
            }

            if (!TextBoxControl_MaxConcurrentRequests.TryGetValue(out int maxConcurrentRequests) || (complete && maxConcurrentRequests < 1))
            {
                message = "Max concurrent requests has to be a whole number of at least one.";
                return false;
            }

            // No selection is written as no county list (null); an empty set would mean "no county was named"
            // and the run would stop, so it is never handed over - neither by OK nor by an export.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();

            HashSet<int>? countyIds = null;
            if (administrativeAreal2DReferences is not null && administrativeAreal2DReferences.Count != 0)
            {
                countyIds = [];
                foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
                {
                    countyIds.Add(administrativeAreal2DReference.Id);
                }
            }

            if (complete && countyIds is null)
            {
                message = "At least one county has to be selected.";
                return false;
            }

            // An empty path is not a path: null is what makes the runner search PATH for an interpreter,
            // while an empty string is a path that resolves to nothing. Trimmed on both paths, as OK does.
            string? pythonPath = TextBoxControl_PythonPath.Value;
            pythonPath = string.IsNullOrWhiteSpace(pythonPath) ? null : pythonPath.Trim();

            // Eight members have no control here on purpose, and unlike the step flags forced on the complete
            // path above they are not set either - they carry through the copy untouched, as does anything
            // else this window is not responsible for.
            //
            // Years, Radiuses, Confidence and ModelPath all decide what the regressor is handed. The year range
            // and the radiuses decide which columns the feature projection asks for; the confidence threshold is
            // passed to the detector as --conf, and every detection it lets through is written into the ninety
            // "Prediction Confidence <year>" columns the model reads back - so a run at anything other than the
            // value the weights were trained against hands the model a feature distribution it has never seen.
            // It scores without failing, and a tray run always writes those columns, so the mistake outlives the
            // run that made it. The feature coverage guard cannot catch it either: it refuses only a group that
            // is wholly absent. Naming different weights is the same failure through the same path,
            // and a ModelPath typed here would be resolved in the runner's directory rather than in this
            // application's, so the box could not even say whether the file exists
            // (DiGi-Organization/DiGi.GIS.ML#6). The year range and the radiuses are the two of the four the run itself
            // checks: the orchestrator compares them against the model's stated trained contract before it reads a
            // county, and refuses the run when they narrow it (DiGi-Organization/DiGi.GIS.ML#6). A control for any of
            // them still invites the mistake; the check exists for the two the model can state.
            //
            // WorkingDirectory is not a choice: DiGi.YOLO.Modify.Predict writes predict.py and utils.py into it,
            // so there is nothing for the operator to point at, and left null it resolves to the county's own
            // scratch folder, which is already correct and is cleaned up with the rest of it.
            //
            // BatchSize and ReferenceBatchSize are not choices either. BatchSize sizes the write requests - it
            // never reaches the detector, whose image batch is its own - and five thousand is what a county of
            // buildings against ninety odd detection columns was tuned to. ReferenceBatchSize has one sensible
            // value: above the endpoint's cap is refused server side, below it is the same work in more
            // requests. Max concurrent requests is the one knob that answers a server which has started
            // refusing, and it is the one that is offered.
            //
            // ReferencesFilePath narrows a run to the buildings of a training dataset manifest, which only the
            // labelled-building detection rewrite before a regressor retrain wants (DiGi-Organization/DiGi.GIS.YOLO.UI#22).
            // The tray's scoring runs cover whole counties, so it is left as the copy carries it.

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            YearBuiltPredictionPipelineOptions yearBuiltPredictionPipelineOptions_Temp = new(yearBuiltPredictionPipelineOptions)
            {
                ScratchDirectory = scratchDirectory,
                CountyIds = countyIds,
                MaxConcurrentRequests = maxConcurrentRequests,
                PythonPath = pythonPath,
                CleanScratchDirectory = CheckBox_CleanScratchDirectory.IsChecked == true,
            };

            if (complete)
            {
                // The full six step flow, written rather than left to the defaults, and Resume fixed on with
                // them - the run shape of a tray run, whatever the copy carried.
                yearBuiltPredictionPipelineOptions_Temp.ExportImages = true;
                yearBuiltPredictionPipelineOptions_Temp.RunPrediction = true;
                yearBuiltPredictionPipelineOptions_Temp.Score = true;
                yearBuiltPredictionPipelineOptions_Temp.UpdateDetections = true;
                yearBuiltPredictionPipelineOptions_Temp.UpdateYearBuiltData = true;
                yearBuiltPredictionPipelineOptions_Temp.UpdatePredictedYearBuilt = true;
                yearBuiltPredictionPipelineOptions_Temp.Resume = true;
            }

            options = yearBuiltPredictionPipelineOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before: options with no scratch directory
        /// show the default (as the constructor always showed them), options with no county list clear the
        /// selection. The step flags and the eight members with no control are left alone - OK writes the run
        /// shape. Internal so a fact can fill a window from a settings file the way Import does, without a file
        /// dialog.
        /// </summary>
        /// <param name="yearBuiltPredictionPipelineOptions">The options to show.</param>
        internal void SetControls(YearBuiltPredictionPipelineOptions yearBuiltPredictionPipelineOptions)
        {
            // The captions stay in the XAML - the designer does not run this constructor, and a caption set here
            // leaves an empty label over an empty box in the preview. Only the values come from the options.
            TextBoxControl_ScratchDirectory.Value = string.IsNullOrWhiteSpace(yearBuiltPredictionPipelineOptions.ScratchDirectory) ? DefaultScratchDirectory : yearBuiltPredictionPipelineOptions.ScratchDirectory;
            TextBoxControl_PythonPath.Value = yearBuiltPredictionPipelineOptions.PythonPath;
            TextBoxControl_MaxConcurrentRequests.Value = yearBuiltPredictionPipelineOptions.MaxConcurrentRequests.ToString(CultureInfo.CurrentCulture);

            CheckBox_CleanScratchDirectory.IsChecked = yearBuiltPredictionPipelineOptions.CleanScratchDirectory;

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh on every run. SelectItems deselects the rest, so a file with no
            // county list clears whatever was selected before.
            HashSet<int> countyIds = yearBuiltPredictionPipelineOptions.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one Year Built pipeline options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static YearBuiltPredictionPipelineOptions? TryReadSettings(string? json, out string? message)
        {
            List<YearBuiltPredictionPipelineOptions>? yearBuiltPredictionPipelineOptionsList = Core.Convert.ToDiGi<YearBuiltPredictionPipelineOptions>(json);
            if (yearBuiltPredictionPipelineOptionsList is null || yearBuiltPredictionPipelineOptionsList.Count != 1)
            {
                message = "The file holds no Year Built settings - nothing was changed.";
                return null;
            }

            message = null;

            return yearBuiltPredictionPipelineOptionsList[0];
        }

        private void ListBoxControl_Counties_ItemAdding(object sender, ListBoxItemAddingEventArgs e)
        {
            if (e.Item is not AdministrativeAreal2DReference administrativeAreal2DReference)
            {
                return;
            }

            // The identifier is shown because it is what the run is keyed by, and because two pieces of the same
            // county are told apart by nothing else - they share their code and their name.
            string name = $"{administrativeAreal2DReference.Code} {administrativeAreal2DReference.Name} (id {administrativeAreal2DReference.Id})";

            // Per part id, never per code: each piece of a multi-part county has its own coverage.
            string? marker = coverageFactors is not null && coverageFactors.TryGetValue(administrativeAreal2DReference.Id, out double coverageFactor) ? Query.PredictedYearBuiltCoverageMarker(coverageFactor) : null;

            e.Name = marker is null ? name : $"{name} {marker}";
        }

        private void SetCounties(IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            if (administrativeAreal2DReferences is null)
            {
                ListBoxControl_Counties.ClearItems();
                return;
            }

            List<AdministrativeAreal2DReference> administrativeAreal2DReferences_Sorted = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                if (administrativeAreal2DReference is null)
                {
                    continue;
                }

                administrativeAreal2DReferences_Sorted.Add(administrativeAreal2DReference);
            }

            // By code, so that the counties of one voivodeship sit together; by identifier within a code,
            // because the pieces of a multi-part county share their code and their name and the identifier is
            // all that separates them.
            administrativeAreal2DReferences_Sorted.Sort((x, y) =>
            {
                int result = string.CompareOrdinal(x.Code ?? string.Empty, y.Code ?? string.Empty);

                return result != 0 ? result : x.Id.CompareTo(y.Id);
            });

            ListBoxControl_Counties.SetItems(administrativeAreal2DReferences_Sorted);

            HashSet<int>? countyIds = yearBuiltPredictionPipelineOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run.
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }
    }
}
