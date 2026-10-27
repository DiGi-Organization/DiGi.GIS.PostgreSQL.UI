using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.YOLO.UI.Classes;
using DiGi.UI.WPF.Classes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for YearBuiltPredictionsOptionsWindow.xaml
    /// <para>Asks for the scope of one Year Built prediction run and nothing else: the counties, where its imagery goes, which interpreter runs the detector, and how hard the export leans on the server.</para>
    /// <para><b>A tray run has one shape - the full six step flow - and it writes.</b> The steps are not offered because they are not a choice here: DiGi-Organization/DiGi.GIS.YOLO.UI#8 made export, detector, detection write, score, history write and column write a single run per county precisely so that no step could be left out of sequence, and it decided that the granular flags stay on the options class and the console app for hand-driven diagnostics while the tray driven flow collapses them. Eight checkboxes offered two hundred and fifty six combinations, of which three were real - and the rest failed late, after the half hour of export and the hour and a half of inference had already been paid for. The OK handler writes all six on, so the run the operator gets is the run the standing recipe describes.</para>
    /// <para><b>What settles the model is deliberately not here either.</b> The weights, the confidence threshold, the year range and the radiuses all decide what the regressor is handed, and a value that disagrees with what it was trained on is, for the year range and the radiuses, checked against the model's stated trained contract before the run reads a county - narrower is refused, wider is warned (DiGi-Organization/DiGi.GIS.ML#6) - while the weights and the confidence threshold still score without failing. They belong to the deployment and to the options file rather than to a dialog opened before every run. The working directory and the two batch sizes are out for a different reason: none of the three is a choice - see the comment in the OK handler.</para>
    /// <para>The scratch cleanup is the one flag that survives, because its reason is about this run rather than about the sequence: a cancelled county is cleaned, so a run that is meant to be interrupted has to be able to say so beforehand.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were, and every member the window has no control for carries through untouched.</para>
    /// </summary>
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
        /// <param name="coverageFactorsAsync">Reads the predicted year built coverage of each county part, keyed by part id, for the markers. Started here and never awaited before the window opens: the county list is shown at once and its markers are filled in when the read completes, whenever that is. Closing the window cancels it. When null no coverage is read and nothing is marked.</param>
        public YearBuiltPredictionsOptionsWindow(YearBuiltPredictionPipelineOptions? yearBuiltPredictionPipelineOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences, Func<CancellationToken, Task<Dictionary<int, double>?>>? coverageFactorsAsync = null)
        {
            InitializeComponent();

            this.yearBuiltPredictionPipelineOptions = yearBuiltPredictionPipelineOptions is null ? new YearBuiltPredictionPipelineOptions() : new YearBuiltPredictionPipelineOptions(yearBuiltPredictionPipelineOptions);

            // The captions stay in the XAML - the designer does not run this constructor, and a caption set here
            // leaves an empty label over an empty box in the preview. Only the values come from the options.
            TextBoxControl_ScratchDirectory.Value = string.IsNullOrWhiteSpace(this.yearBuiltPredictionPipelineOptions.ScratchDirectory) ? DefaultScratchDirectory : this.yearBuiltPredictionPipelineOptions.ScratchDirectory;
            TextBoxControl_PythonPath.Value = this.yearBuiltPredictionPipelineOptions.PythonPath;
            TextBoxControl_MaxConcurrentRequests.Value = this.yearBuiltPredictionPipelineOptions.MaxConcurrentRequests.ToString();

            CheckBox_CleanScratchDirectory.IsChecked = this.yearBuiltPredictionPipelineOptions.CleanScratchDirectory;

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);

            // A read still running when the window closes is no longer wanted by anyone; cancelling it also stops the query
            // behind it on the server.
            Closed += (sender, e) => cancellationTokenSource.Cancel();

            if (coverageFactorsAsync is not null)
            {
                Label_Coverage.Content = "Reading prediction coverage...";
                _ = ReadCoverageFactorsAsync(coverageFactorsAsync);
            }
        }

        /// <summary>
        /// Sets the predicted year built coverage the county markers are drawn from, and redraws the county list in place.
        /// <para>The selection and the scroll position are kept: the coverage normally arrives after the window is open, possibly while counties are being picked. Null means the coverage could not be read, which marks nothing and says so - never a false "not run".</para>
        /// </summary>
        /// <param name="coverageFactors">The share of each county part's buildings carrying a predicted year built, keyed by part id, or null when it could not be read.</param>
        public void SetCoverageFactors(IReadOnlyDictionary<int, double>? coverageFactors)
        {
            this.coverageFactors = coverageFactors is null ? null : new Dictionary<int, double>(coverageFactors);

            Label_Coverage.Content = coverageFactors is null
                ? "Prediction coverage could not be read - no county is marked"
                : string.Format("{0} - more than {1} % of the county's buildings already carry a predicted year built", Query.PredictedYearBuiltCoverageMarker(double.PositiveInfinity), Constants.YearBuiltPrediction.CoverageMarkerThreshold * 100);

            ListBoxControl_Counties.RefreshItems();
        }

        private async Task ReadCoverageFactorsAsync(Func<CancellationToken, Task<Dictionary<int, double>?>> coverageFactorsAsync)
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

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            string? scratchDirectory = TextBoxControl_ScratchDirectory.Value;
            if (string.IsNullOrWhiteSpace(scratchDirectory))
            {
                // A blank box is taken as "the default", not refused. The runner still refuses an empty directory,
                // which is the backstop for a hand-written options file.
                scratchDirectory = DefaultScratchDirectory;
            }

            if (!TextBoxControl_MaxConcurrentRequests.TryGetValue(out int maxConcurrentRequests) || maxConcurrentRequests < 1)
            {
                MessageBox.Show("Max concurrent requests has to be a whole number of at least one.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                // An empty set is not the same as no set: the run takes it as "no county was named" and stops, so
                // it must never leave this window.
                MessageBox.Show("At least one county has to be selected.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            HashSet<int> countyIds = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                countyIds.Add(administrativeAreal2DReference.Id);
            }

            yearBuiltPredictionPipelineOptions.ScratchDirectory = scratchDirectory;
            yearBuiltPredictionPipelineOptions.CountyIds = countyIds;
            yearBuiltPredictionPipelineOptions.MaxConcurrentRequests = maxConcurrentRequests;

            // An empty path is not a path: null is what makes the runner search PATH for an interpreter, while an
            // empty string is a path that resolves to nothing.
            yearBuiltPredictionPipelineOptions.PythonPath = Value(TextBoxControl_PythonPath.Value);

            // The full six step flow, written rather than left to the defaults: the three write flags default to
            // false, and the window is handed the previous run's options, so a tray run that did not set them
            // here would read a county, score it and store nothing while reporting that it had run.
            yearBuiltPredictionPipelineOptions.ExportImages = true;
            yearBuiltPredictionPipelineOptions.RunPrediction = true;
            yearBuiltPredictionPipelineOptions.Score = true;
            yearBuiltPredictionPipelineOptions.UpdateDetections = true;
            yearBuiltPredictionPipelineOptions.UpdateYearBuiltData = true;
            yearBuiltPredictionPipelineOptions.UpdatePredictedYearBuilt = true;

            // Resume is fixed on rather than offered. With the cleanup on it only helps inside a single run and
            // on the retry of a county that failed - which keeps its scratch folder for exactly that reason - and
            // there is no run for which off is the better answer.
            yearBuiltPredictionPipelineOptions.Resume = true;

            // The one flag that is a choice: a cancelled county is cleaned, so a run that is meant to be
            // interrupted has to be able to keep what it exported.
            yearBuiltPredictionPipelineOptions.CleanScratchDirectory = CheckBox_CleanScratchDirectory.IsChecked == true;

            // Eight members have no control here on purpose, and unlike the step flags above they are not set
            // either - they carry through the copy untouched, as does anything else this window is not
            // responsible for.
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

            DialogResult = true;
            Close();

            static string? Value(string? text)
            {
                return string.IsNullOrWhiteSpace(text) ? null : text!.Trim();
            }
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
