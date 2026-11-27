using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.UI.Enums;
using DiGi.GIS.YOLO.UI.Classes;
using DiGi.GIS.YOLO.UI.Enums;
using DiGi.UI.WPF.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for YOLOTrainingOptionsWindow.xaml
    /// <para>Asks for the inputs of one YOLO detector training run: the scenario, the counties, the dataset folder, the start weights, the training hyperparameters, where the run is written, the interpreter, the steps and the weights the new detector is gated against.</para>
    /// <para><b>The scenario sets defaults, not rules.</b> Switching it resets the start weights and the epoch ceiling to that scenario's defaults (<see cref="Create.YOLOTrainingRunOptions(YOLOTrainingScenario, string?, YOLOTrainingRunOptions?)"/>) and leaves every other control as it is; on OK it also decides whether the dataset folder is appended to (Re-train) or must be new (Start from yolo26x.pt).</para>
    /// <para><b>An interrupted run is offered for resume.</b> When the run name and project directory name a run whose folder holds an unfinished <c>weights\last.pt</c> and no completed <c>&lt;RunName&gt;.pt</c>, the dialog shows the checkpoint's epoch and ceiling and offers to resume it. Ticking <see cref="YOLOTrainingStep.Train"/> is still required; the box locks what a resume cannot change - the hyperparameters, the start weights and the dataset build - because the checkpoint restores them, and the epoch ceiling is fixed by the checkpoint.</para>
    /// <para><b>Stalls are handled by the runner.</b> <c>Automatic resumes</c> and <c>Stall limit</c> set <see cref="YOLOTrainingRunOptions.AutoResumeCount"/> and <see cref="YOLOTrainingRunOptions.InactivityTimeout"/>: a training that stalls or crashes is continued from its own <c>last.pt</c> that many times, and a stall is what a silent training becomes after the limit. Both are enabled only with the Training step, and an empty stall limit keeps the runner's default of 15 minutes.</para>
    /// <para><b>What is not offered is the runner's to decide.</b> The dataset's confidence threshold, split, label check sizes, legacy cut-off and request sizes carry through the copy untouched at the values its README describes; they shape what the detector is trained on, and a control opened before every run invites changing them between runs that are then compared.</para>
    /// <para><b>Test areas are kept out of training.</b> The exclusion buffer and the exclusion boxes (DiGi.GIS.PostgreSQL.UI#39) edit <see cref="YOLOTrainingDatasetOptions.ExclusionBuffer"/> and <see cref="YOLOTrainingDatasetOptions.ExclusionBoundingBoxes"/>, parsed by <see cref="Query.TryParseYOLOExclusionBuffer(string?, out double, out string?)"/> and <see cref="Query.TryParseYOLOExclusionBoundingBoxes(string?, out List{YOLOExclusionBoundingBox}?, out string?)"/>: an empty buffer is the class default, an empty box list excludes nothing, and a malformed line refuses OK with its line number.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#76), so a run repeated after the tray was restarted is not typed again. The scenario is not part of the file - it stays as chosen, and the buttons' tooltips say so - and Import only fills the controls and re-runs the interrupted-run evaluation for the imported run name and project directory. Nothing runs until OK, and OK validates the imported values like typed ones; the resume stashes, the checkpoint and the runner path are window state and never reach the file.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were, and every member the window has no control for carries through untouched.</para>
    /// </summary>
    public partial class YOLOTrainingOptionsWindow : Window
    {
        private readonly string? consoleAppPath;
        private YOLOTrainingRunOptions yOLOTrainingRunOptions;
        private YOLOTrainingScenario yOLOTrainingScenario;

        // The interrupted run the name boxes currently point at, and the hyperparameter values the resume replaces
        // while its box is ticked - kept so unticking the box puts the operator's own values back.
        private DiGi.YOLO.Classes.YOLOCheckpointInformation? yOLOCheckpointInformation;
        private string? path_InterruptedRun;
        private bool resumeState;
        private string? startWeightsPath_Resume;
        private string? epochs_Resume;
        private string? patience_Resume;
        private string? imageSize_Resume;
        private string? batch_Resume;
        private string? seed_Resume;
        private bool dataset_Resume;

        /// <summary>
        /// Initializes a new instance of the <see cref="YOLOTrainingOptionsWindow"/> class.
        /// </summary>
        /// <param name="yOLOTrainingScenario">The scenario the dialog opens with.</param>
        /// <param name="yOLOTrainingRunOptions">The options the controls are filled from - the previous run's, which are copied rather than changed. When null the defaults of <paramref name="yOLOTrainingScenario"/> are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        /// <param name="consoleAppPath">The full path of the runner, which the default weights are named relative to.</param>
        public YOLOTrainingOptionsWindow(YOLOTrainingScenario yOLOTrainingScenario, YOLOTrainingRunOptions? yOLOTrainingRunOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences, string? consoleAppPath)
        {
            InitializeComponent();

            this.consoleAppPath = consoleAppPath;
            this.yOLOTrainingScenario = yOLOTrainingScenario;

            // An earlier run's options are copied as they are - applying the scenario's defaults to them here would
            // throw away the start weights and epochs the operator chose last time. Only a first run is defaulted.
            this.yOLOTrainingRunOptions = yOLOTrainingRunOptions is null ? Create.YOLOTrainingRunOptions(yOLOTrainingScenario, consoleAppPath) : new YOLOTrainingRunOptions(yOLOTrainingRunOptions);

            // Selected before the handler is attached, so opening the dialog is not taken for a switch. The
            // scenario is the one control the settings file never fills - it stays as chosen.
            ComboBox_Scenario.SelectedIndex = yOLOTrainingScenario == YOLOTrainingScenario.Fresh ? 1 : 0;
            ComboBox_Scenario.SelectionChanged += ComboBox_Scenario_SelectionChanged;

            // The boxes, the step checks and the county selection all come from here - the same call an import
            // makes. The county pass is a no-op before the list exists; SetCounties below fills the list and
            // applies the options' own selection, exactly as it always did.
            SetControls(this.yOLOTrainingRunOptions);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);

            // A run that is already interrupted is offered as soon as the dialog opens with its name; a change to
            // either name re-evaluates it, so a name typed by hand is offered before OK is pressed.
            TextBoxControl_RunName.LostFocus += TextBoxControl_RunNameOrProjectDirectory_LostFocus;
            TextBoxControl_ProjectDirectory.LostFocus += TextBoxControl_RunNameOrProjectDirectory_LostFocus;

            EvaluateInterruptedRun();

            SetStallInputsEnabled();
        }

        /// <summary>
        /// Gets the scenario the window holds. It is the one chosen only once the dialog has been closed with OK; until then, and after a cancellation, it is the one it was opened with.
        /// </summary>
        public YOLOTrainingScenario YOLOTrainingScenario
        {
            get
            {
                return yOLOTrainingScenario;
            }
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public YOLOTrainingRunOptions YOLOTrainingRunOptions
        {
            get
            {
                return yOLOTrainingRunOptions;
            }
        }

        private YOLOTrainingScenario SelectedScenario()
        {
            return ComboBox_Scenario.SelectedIndex == 1 ? YOLOTrainingScenario.Fresh : YOLOTrainingScenario.Retrain;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: a half-prepared window can be saved and finished after the next import - the
            // ranges, the required directories, the step/county floors and the resume pairing are OK's gate,
            // not the file's. What is refused is what could not be written back as typed: a box that does not
            // hold its number, or exclusion text that does not parse. The scenario is not written: only the
            // YOLOTrainingRunOptions leave through this button, and the scenario stays as chosen.
            if (!TryReadOptions(false, out YOLOTrainingRunOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export YOLO training settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "YOLO_Training_Settings.json"
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
                Title = "Import YOLO training settings",
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

            if (TryReadSettings(json, out string? message_Read) is not YOLOTrainingRunOptions yOLOTrainingRunOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Only the controls are filled - the scenario combo deliberately among them not - and then the
            // interrupted-run evaluation runs again for the imported run name and project directory, so an
            // imported pair offers resuming exactly as a typed one does; the resume tick follows the file when
            // an offer is on. Nothing runs until OK, and a cancellation still leaves the options untouched.
            SetControls(yOLOTrainingRunOptions_Imported);
            EvaluateInterruptedRun();
            SetStallInputsEnabled();
            CheckBox_ResumeInterruptedRun.IsChecked = yOLOTrainingRunOptions_Imported.ResumeTraining;

            // Said rather than left for OK to find: a county the list does not hold cannot be selected - the
            // usual sign the settings came from another machine.
            HashSet<int> countyIds = yOLOTrainingRunOptions_Imported.DatasetOptions?.CountyIds ?? [];
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
        /// Reads every control into a candidate set of options and checks it, without closing the window or showing a message - the part of OK a fact can drive, because a message box would hang it.
        /// <para>The options are written only when every control reads and the whole gate OK has always applied passes - at least one step, at least one county, the hyperparameter ranges, the stall pairing, the required directories, the resume pairing and the exclusion text - and the result is built exactly as OK builds it: the scenario of the current selection applied to a copy of the window's options first, then every control over it. A refusal leaves the window's options as they were.</para>
        /// <para>Export reads the same controls without the gates, so a half-prepared window can be saved: no county and no dataset directory write nothing, and relative directories, missing start weights and out-of-range numbers still export, to be refused by OK on read-back. What is refused on both paths is what could not be written back as typed - a box that does not hold its number, a stall limit that is not a number of minutes, exclusion text that does not parse - and an empty step list, which the runner reads as every step and so could not round-trip. The scenario is never read from the file: it is not in it.</para>
        /// </summary>
        /// <param name="complete">True for OK: every gate applies. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out YOLOTrainingRunOptions? options, out string? message)
        {
            options = null;
            message = null;

            // The runner reads an empty list as every step, so a selection of none could not be written back as
            // typed: the file would come back as every step ticked. Required on both paths, like the external
            // components window's county selection.
            List<YOLOTrainingStep> yOLOTrainingSteps = [];
            if (CheckBox_Dataset.IsChecked == true)
            {
                yOLOTrainingSteps.Add(YOLOTrainingStep.Dataset);
            }

            if (CheckBox_LabelCheck.IsChecked == true)
            {
                yOLOTrainingSteps.Add(YOLOTrainingStep.LabelCheck);
            }

            if (CheckBox_Train.IsChecked == true)
            {
                yOLOTrainingSteps.Add(YOLOTrainingStep.Train);
            }

            if (CheckBox_Validate.IsChecked == true)
            {
                yOLOTrainingSteps.Add(YOLOTrainingStep.Validate);
            }

            if (CheckBox_Evaluate.IsChecked == true)
            {
                yOLOTrainingSteps.Add(YOLOTrainingStep.Evaluate);
            }

            if (yOLOTrainingSteps.Count == 0)
            {
                message = "At least one step has to be selected.";
                return false;
            }

            // The counties are what the dataset step builds from; without it they scope nothing, but they are
            // still what the confirmation names back, so a run with none is refused rather than guessed at.
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

            if (!TextBoxControl_Epochs.TryGetValue(out int epochs) || (complete && epochs < 1))
            {
                message = "Epochs has to be a whole number of at least one.";
                return false;
            }

            if (!TextBoxControl_Patience.TryGetValue(out int patience) || (complete && patience < 0))
            {
                message = "Patience has to be a whole number of at least zero.";
                return false;
            }

            if (!TextBoxControl_ImageSize.TryGetValue(out int imageSize) || (complete && imageSize < 32))
            {
                message = "Image size has to be a whole number of at least 32.";
                return false;
            }

            if (!TextBoxControl_Batch.TryGetValue(out int batch) || (complete && batch < 1))
            {
                message = "Batch has to be a whole number of at least one.";
                return false;
            }

            if (!TextBoxControl_Seed.TryGetValue(out int seed) || (complete && seed < 0))
            {
                message = "Seed has to be a whole number of at least zero.";
                return false;
            }

            if (!TextBoxControl_AutoResumeCount.TryGetValue(out int autoResumeCount))
            {
                message = "Automatic resumes has to be a whole number from 0 to 10.";
                return false;
            }

            // Empty is the runner's default of fifteen minutes; a box holding something that is not a number
            // of minutes cannot be written back as typed, on either path.
            TimeSpan? inactivityTimeout = null;
            if (TextBoxControl_InactivityTimeout.TryGetValue(out int inactivityTimeoutMinutes))
            {
                inactivityTimeout = TimeSpan.FromMinutes(inactivityTimeoutMinutes);
            }
            else if (!string.IsNullOrWhiteSpace(TextBoxControl_InactivityTimeout.Value))
            {
                message = "The stall limit has to be a whole number of at least one minute, or empty to use the default.";
                return false;
            }

            if (complete && !Query.IsYOLOTrainingStallOptionsValid(autoResumeCount, inactivityTimeout, out string? stallReason))
            {
                message = stallReason!;
                return false;
            }

            string? datasetDirectory = Value(TextBoxControl_DatasetDirectory.Value);
            if (complete && datasetDirectory is null)
            {
                // Every step reads or writes the dataset folder, so it is asked for whichever steps are ticked.
                message = "A dataset directory has to be given.";
                return false;
            }

            bool train = yOLOTrainingSteps.Contains(YOLOTrainingStep.Train);

            bool resume = yOLOCheckpointInformation is not null && CheckBox_ResumeInterruptedRun.IsChecked == true;
            if (complete && resume && !train)
            {
                message = "A resume needs the Training step - the training is the step it continues.";
                return false;
            }

            if (complete && resume && yOLOTrainingSteps.Contains(YOLOTrainingStep.Dataset))
            {
                message = "A resume never rebuilds the dataset it was trained on.";
                return false;
            }

            string? startWeightsPath = Value(TextBoxControl_StartWeightsPath.Value);
            if (complete && startWeightsPath is null && (train || yOLOTrainingSteps.Contains(YOLOTrainingStep.Validate)))
            {
                message = "Start weights have to be given for training or validation.";
                return false;
            }

            string? runName = Value(TextBoxControl_RunName.Value);
            string? projectDirectory = Value(TextBoxControl_ProjectDirectory.Value);
            if (complete && train && (runName is null || projectDirectory is null))
            {
                message = "A run name and a project directory have to be given for training.";
                return false;
            }

            // A relative directory would otherwise be resolved against this process's current directory, where the
            // runner would not look for it, so it is refused here rather than silently mis-scoped. A relative path
            // is a string that could be written back as typed, so only OK refuses it.
            if (complete && DirectoryRefusal(datasetDirectory, projectDirectory, train) is string directoryRefusal)
            {
                message = directoryRefusal;
                return false;
            }

            if (!Query.TryParseYOLOExclusionBuffer(TextBoxControl_ExclusionBuffer.Value, out double exclusionBuffer, out string? exclusionBufferReason))
            {
                message = exclusionBufferReason!;
                return false;
            }

            if (!Query.TryParseYOLOExclusionBoundingBoxes(TextBox_ExclusionBoundingBoxes.Text, out List<YOLOExclusionBoundingBox>? yOLOExclusionBoundingBoxes, out string? exclusionBoundingBoxesReason))
            {
                message = exclusionBoundingBoxesReason!;
                return false;
            }

            List<string> weightsPaths = [];
            foreach (string weightsPath in (TextBoxControl_WeightsPaths.Value ?? string.Empty).Split(';'))
            {
                if (Value(weightsPath) is string weightsPath_Trimmed)
                {
                    weightsPaths.Add(weightsPath_Trimmed);
                }
            }

            YOLOTrainingScenario yOLOTrainingScenario_Selected = SelectedScenario();

            // The scenario is applied to the copy first - it is what decides whether the dataset folder is appended
            // to - and the controls then overwrite the defaults it set, so what was typed is what runs. The same
            // construction on the export path makes the file what OK would write, minus the gates.
            YOLOTrainingRunOptions yOLOTrainingRunOptions_Result = Create.YOLOTrainingRunOptions(yOLOTrainingScenario_Selected, consoleAppPath, yOLOTrainingRunOptions);
            YOLOTrainingDatasetOptions yOLOTrainingDatasetOptions = yOLOTrainingRunOptions_Result.DatasetOptions ?? new YOLOTrainingDatasetOptions();
            yOLOTrainingRunOptions_Result.DatasetOptions = yOLOTrainingDatasetOptions;

            string? pythonPath = Value(TextBoxControl_PythonPath.Value);
            string? workingDirectory = Value(TextBoxControl_WorkingDirectory.Value);

            yOLOTrainingRunOptions_Result.StartWeightsPath = startWeightsPath;
            yOLOTrainingRunOptions_Result.Epochs = epochs;
            yOLOTrainingRunOptions_Result.Patience = patience;
            yOLOTrainingRunOptions_Result.ImageSize = imageSize;
            yOLOTrainingRunOptions_Result.Batch = batch;
            yOLOTrainingRunOptions_Result.Seed = seed;
            yOLOTrainingRunOptions_Result.AutoResumeCount = autoResumeCount;
            yOLOTrainingRunOptions_Result.InactivityTimeout = inactivityTimeout;
            yOLOTrainingRunOptions_Result.Device = Value(TextBoxControl_Device.Value);
            yOLOTrainingRunOptions_Result.RunName = runName;
            yOLOTrainingRunOptions_Result.ProjectDirectory = projectDirectory;
            yOLOTrainingRunOptions_Result.ResumeTraining = resume;
            yOLOTrainingRunOptions_Result.Steps = yOLOTrainingSteps;

            // Written at both levels: the run-level value overrides for training, and the dataset-level one is
            // what the label check and the evaluation are handed, so one box means one interpreter everywhere.
            yOLOTrainingRunOptions_Result.PythonPath = pythonPath;
            yOLOTrainingRunOptions_Result.WorkingDirectory = workingDirectory;
            yOLOTrainingDatasetOptions.PythonPath = pythonPath;
            yOLOTrainingDatasetOptions.WorkingDirectory = workingDirectory;

            yOLOTrainingDatasetOptions.OutputDirectory = datasetDirectory;
            yOLOTrainingDatasetOptions.CountyIds = countyIds;
            yOLOTrainingDatasetOptions.WeightsPaths = weightsPaths.Count == 0 ? null : weightsPaths;
            yOLOTrainingDatasetOptions.ExclusionBuffer = exclusionBuffer;
            yOLOTrainingDatasetOptions.ExclusionBoundingBoxes = yOLOExclusionBoundingBoxes ?? [];

            options = yOLOTrainingRunOptions_Result;

            return true;
        }

        /// <summary>
        /// Reads every control into the window's options and checks them - the fact-driven part of OK, which writes the candidate over the window's options and takes the scenario from the selection box. The scenario is never read from a file: it is not in one.
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and the scenario its selection, and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out YOLOTrainingRunOptions? options, out message) || options is null)
            {
                return false;
            }

            yOLOTrainingRunOptions = options;
            yOLOTrainingScenario = SelectedScenario();

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before: an empty step list reads as every
        /// step, exactly as the runner reads it. The scenario combo is deliberately not touched - the scenario is
        /// not part of the settings file and stays as chosen; the resume offer is the interrupted-run evaluation's
        /// to make, so the caller runs it after this returns. Internal so a fact can fill a window from a settings
        /// file the way Import does, without a file dialog.
        /// </summary>
        /// <param name="yOLOTrainingRunOptions">The options to show.</param>
        internal void SetControls(YOLOTrainingRunOptions yOLOTrainingRunOptions)
        {
            YOLOTrainingDatasetOptions? yOLOTrainingDatasetOptions = yOLOTrainingRunOptions.DatasetOptions;

            TextBoxControl_DatasetDirectory.Value = yOLOTrainingDatasetOptions?.OutputDirectory;
            TextBoxControl_StartWeightsPath.Value = yOLOTrainingRunOptions.StartWeightsPath;
            TextBoxControl_Epochs.Value = yOLOTrainingRunOptions.Epochs.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_Patience.Value = yOLOTrainingRunOptions.Patience.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_ImageSize.Value = yOLOTrainingRunOptions.ImageSize.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_Batch.Value = yOLOTrainingRunOptions.Batch.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_Seed.Value = yOLOTrainingRunOptions.Seed.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_Device.Value = yOLOTrainingRunOptions.Device;
            TextBoxControl_AutoResumeCount.Value = yOLOTrainingRunOptions.AutoResumeCount.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_InactivityTimeout.Value = yOLOTrainingRunOptions.InactivityTimeout is TimeSpan inactivityTimeout ? ((int)inactivityTimeout.TotalMinutes).ToString(CultureInfo.InvariantCulture) : null;
            TextBoxControl_RunName.Value = yOLOTrainingRunOptions.RunName;
            TextBoxControl_ProjectDirectory.Value = yOLOTrainingRunOptions.ProjectDirectory;
            TextBoxControl_PythonPath.Value = yOLOTrainingRunOptions.PythonPath ?? yOLOTrainingDatasetOptions?.PythonPath;
            TextBoxControl_WorkingDirectory.Value = yOLOTrainingRunOptions.WorkingDirectory ?? yOLOTrainingDatasetOptions?.WorkingDirectory;
            TextBoxControl_WeightsPaths.Value = yOLOTrainingDatasetOptions?.WeightsPaths is List<string> weightsPaths ? string.Join("; ", weightsPaths) : null;

            // The exclusion of the pilot test areas (DiGi.GIS.PostgreSQL.UI#39), shown as stored so reopening the
            // dialog shows what the next build will exclude.
            TextBoxControl_ExclusionBuffer.Value = (yOLOTrainingDatasetOptions ?? new YOLOTrainingDatasetOptions()).ExclusionBuffer.ToString("R", CultureInfo.InvariantCulture);
            TextBox_ExclusionBoundingBoxes.Text = yOLOTrainingDatasetOptions?.ExclusionBoundingBoxes.ToSystem_String() ?? string.Empty;

            // The runner reads an empty list as every step, and so does the dialog.
            List<YOLOTrainingStep>? yOLOTrainingSteps = yOLOTrainingRunOptions.Steps;
            CheckBox_Dataset.IsChecked = yOLOTrainingSteps is null || yOLOTrainingSteps.Count == 0 || yOLOTrainingSteps.Contains(YOLOTrainingStep.Dataset);
            CheckBox_LabelCheck.IsChecked = yOLOTrainingSteps is null || yOLOTrainingSteps.Count == 0 || yOLOTrainingSteps.Contains(YOLOTrainingStep.LabelCheck);
            CheckBox_Train.IsChecked = yOLOTrainingSteps is null || yOLOTrainingSteps.Count == 0 || yOLOTrainingSteps.Contains(YOLOTrainingStep.Train);
            CheckBox_Validate.IsChecked = yOLOTrainingSteps is null || yOLOTrainingSteps.Count == 0 || yOLOTrainingSteps.Contains(YOLOTrainingStep.Validate);
            CheckBox_Evaluate.IsChecked = yOLOTrainingSteps is null || yOLOTrainingSteps.Count == 0 || yOLOTrainingSteps.Contains(YOLOTrainingStep.Evaluate);

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh on every run. SelectItems deselects the rest, so a file with no
            // county list clears whatever was selected before.
            HashSet<int> countyIds = yOLOTrainingDatasetOptions?.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one YOLO training options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's. The scenario
        /// is not part of any of them - it is not written and not read.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static YOLOTrainingRunOptions? TryReadSettings(string? json, out string? message)
        {
            List<YOLOTrainingRunOptions>? yOLOTrainingRunOptionsList = Core.Convert.ToDiGi<YOLOTrainingRunOptions>(json);
            if (yOLOTrainingRunOptionsList is null || yOLOTrainingRunOptionsList.Count != 1)
            {
                message = "The file holds no YOLO training settings - nothing was changed.";
                return null;
            }

            message = null;

            return yOLOTrainingRunOptionsList[0];
        }

        private static string? Value(string? text)
        {
            return string.IsNullOrWhiteSpace(text) ? null : text!.Trim();
        }

        /// <summary>
        /// Returns the warning a relative directory is refused with, or null when every directory it names is absolute: the dataset directory always, and the project directory when training is on.
        /// </summary>
        /// <param name="datasetDirectory">The dataset directory, when given.</param>
        /// <param name="projectDirectory">The project directory, when training is on.</param>
        /// <param name="train">Whether the run trains, so the project directory is in play.</param>
        /// <returns>The warning to show, or null when the directories are absolute.</returns>
        internal static string? DirectoryRefusal(string? datasetDirectory, string? projectDirectory, bool train)
        {
            if (!string.IsNullOrWhiteSpace(datasetDirectory) && !System.IO.Path.IsPathRooted(datasetDirectory))
            {
                return "The dataset directory has to be an absolute path.";
            }

            if (train && !string.IsNullOrWhiteSpace(projectDirectory) && !System.IO.Path.IsPathRooted(projectDirectory))
            {
                return "The project directory has to be an absolute path.";
            }

            return null;
        }

        private static string? JsonInt(JsonObject? jsonObject, string name)
        {
            if (jsonObject is not null && jsonObject[name] is JsonValue jsonValue && jsonValue.TryGetValue(out int value))
            {
                return value.ToString(CultureInfo.InvariantCulture);
            }

            return null;
        }

        private void TextBoxControl_RunNameOrProjectDirectory_LostFocus(object sender, RoutedEventArgs e)
        {
            EvaluateInterruptedRun();
        }

        private void CheckBox_ResumeInterruptedRun_Changed(object sender, RoutedEventArgs e)
        {
            ApplyResumeMode();
        }

        private void CheckBox_Train_Changed(object sender, RoutedEventArgs e)
        {
            SetStallInputsEnabled();
        }

        /// <summary>
        /// Enables the automatic-resume count and the stall limit only when the training step is ticked: without training there is nothing that can stall or be resumed, and the runner would ignore both.
        /// </summary>
        private void SetStallInputsEnabled()
        {
            bool train = CheckBox_Train.IsChecked == true;
            TextBoxControl_AutoResumeCount.IsEnabled = train;
            TextBoxControl_InactivityTimeout.IsEnabled = train;
        }

        /// <summary>
        /// Re-reads whether the run named by the run name and project directory boxes is an interrupted one, and shows or hides the resume offer accordingly. The checkpoint is read through <see cref="Query.InterruptedYOLOTrainingRun(string?, string?, string?, string?, System.Threading.CancellationToken)"/>, so the offer names the epoch and ceiling the checkpoint holds.
        /// </summary>
        private void EvaluateInterruptedRun()
        {
            string? projectDirectory = Value(TextBoxControl_ProjectDirectory.Value);
            string? runName = Value(TextBoxControl_RunName.Value);
            string? pythonPath = Value(TextBoxControl_PythonPath.Value);
            string? workingDirectory = Value(TextBoxControl_WorkingDirectory.Value);

            DiGi.YOLO.Classes.YOLOCheckpointInformation? yOLOCheckpointInformation_Evaluated = null;
            if (projectDirectory is not null && runName is not null)
            {
                yOLOCheckpointInformation_Evaluated = Query.InterruptedYOLOTrainingRun(projectDirectory, runName, pythonPath, workingDirectory);
            }

            yOLOCheckpointInformation = yOLOCheckpointInformation_Evaluated;
            path_InterruptedRun = yOLOCheckpointInformation_Evaluated is null || projectDirectory is null || runName is null
                ? null
                : Path.Combine(projectDirectory, runName, DiGi.GIS.YOLO.UI.Constants.DirectoryName.Weights, DiGi.GIS.YOLO.UI.Constants.FileName.LastWeights);

            if (yOLOCheckpointInformation_Evaluated is null)
            {
                TextBlock_InterruptedRun.Text = string.Empty;
                CheckBox_ResumeInterruptedRun.Visibility = Visibility.Collapsed;
                CheckBox_ResumeInterruptedRun.IsChecked = false;
            }
            else
            {
                // Epoch counts the completed epochs; the resume enters the next one, the epoch the runner logs as "Resuming ... from epoch N of M".
                int epoch_Completed = yOLOCheckpointInformation_Evaluated.Epoch ?? 0;
                TextBlock_InterruptedRun.Text = string.Format(CultureInfo.InvariantCulture, "Interrupted after epoch {0} of {1} - resume at epoch {2}?", epoch_Completed, yOLOCheckpointInformation_Evaluated.Epochs ?? 0, epoch_Completed + 1);
                CheckBox_ResumeInterruptedRun.Visibility = Visibility.Visible;

                // Reopened after a resume that was not started yet: the choice is kept rather than made again.
                CheckBox_ResumeInterruptedRun.IsChecked = yOLOTrainingRunOptions.ResumeTraining;
            }

            ApplyResumeMode();
        }

        /// <summary>
        /// Locks what a resume cannot change - the hyperparameters, the start weights and the dataset build - and shows the checkpoint's values while the resume box is ticked; unticking it puts the operator's own values back. Device, interpreter, working directory, gate weights and the steps after training stay editable.
        /// </summary>
        private void ApplyResumeMode()
        {
            bool resume = yOLOCheckpointInformation is not null && CheckBox_ResumeInterruptedRun.IsChecked == true;

            if (resume && !resumeState)
            {
                startWeightsPath_Resume = TextBoxControl_StartWeightsPath.Value;
                epochs_Resume = TextBoxControl_Epochs.Value;
                patience_Resume = TextBoxControl_Patience.Value;
                imageSize_Resume = TextBoxControl_ImageSize.Value;
                batch_Resume = TextBoxControl_Batch.Value;
                seed_Resume = TextBoxControl_Seed.Value;
                dataset_Resume = CheckBox_Dataset.IsChecked == true;
                resumeState = true;
            }
            else if (!resume && resumeState)
            {
                TextBoxControl_StartWeightsPath.Value = startWeightsPath_Resume;
                TextBoxControl_Epochs.Value = epochs_Resume;
                TextBoxControl_Patience.Value = patience_Resume;
                TextBoxControl_ImageSize.Value = imageSize_Resume;
                TextBoxControl_Batch.Value = batch_Resume;
                TextBoxControl_Seed.Value = seed_Resume;
                CheckBox_Dataset.IsChecked = dataset_Resume;
                resumeState = false;
            }

            TextBoxControl_StartWeightsPath.IsEnabled = !resume;
            TextBoxControl_Epochs.IsEnabled = !resume;
            TextBoxControl_Patience.IsEnabled = !resume;
            TextBoxControl_ImageSize.IsEnabled = !resume;
            TextBoxControl_Batch.IsEnabled = !resume;
            TextBoxControl_Seed.IsEnabled = !resume;
            CheckBox_Dataset.IsEnabled = !resume;

            if (!resume || yOLOCheckpointInformation is null)
            {
                return;
            }

            TextBoxControl_StartWeightsPath.Value = path_InterruptedRun;
            TextBoxControl_Epochs.Value = (yOLOCheckpointInformation.Epochs ?? 0).ToString(CultureInfo.InvariantCulture);

            JsonObject? trainArguments = yOLOCheckpointInformation.TrainArguments;
            TextBoxControl_Patience.Value = JsonInt(trainArguments, "patience") ?? TextBoxControl_Patience.Value;
            TextBoxControl_ImageSize.Value = JsonInt(trainArguments, "imgsz") ?? TextBoxControl_ImageSize.Value;
            TextBoxControl_Batch.Value = JsonInt(trainArguments, "batch") ?? TextBoxControl_Batch.Value;
            TextBoxControl_Seed.Value = JsonInt(trainArguments, "seed") ?? TextBoxControl_Seed.Value;

            // A resume never rebuilds the dataset the checkpoint was trained on.
            CheckBox_Dataset.IsChecked = false;
        }

        private void ComboBox_Scenario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Only the two controls the scenario decides are reset; the counties, the folders and everything else
            // the operator typed stay. The dataset folder's resume flag follows the scenario on OK.
            YOLOTrainingRunOptions yOLOTrainingRunOptions_Default = Create.YOLOTrainingRunOptions(SelectedScenario(), consoleAppPath);

            TextBoxControl_StartWeightsPath.Value = yOLOTrainingRunOptions_Default.StartWeightsPath;
            TextBoxControl_Epochs.Value = yOLOTrainingRunOptions_Default.Epochs.ToString(CultureInfo.InvariantCulture);
        }

        private void ListBoxControl_Counties_ItemAdding(object sender, ListBoxItemAddingEventArgs e)
        {
            if (e.Item is not AdministrativeAreal2DReference administrativeAreal2DReference)
            {
                return;
            }

            // The identifier is shown because it is what the dataset is keyed by, and because two pieces of the
            // same county are told apart by nothing else - they share their code and their name.
            e.Name = string.Format(CultureInfo.InvariantCulture, "{0} {1} (id {2})", administrativeAreal2DReference.Code, administrativeAreal2DReference.Name, administrativeAreal2DReference.Id);
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

            // By code, so the counties of one voivodeship sit together; by identifier within a code, because the
            // pieces of a multi-part county share their code and their name.
            administrativeAreal2DReferences_Sorted.Sort((x, y) =>
            {
                int result = string.CompareOrdinal(x.Code ?? string.Empty, y.Code ?? string.Empty);

                return result != 0 ? result : x.Id.CompareTo(y.Id);
            });

            ListBoxControl_Counties.SetItems(administrativeAreal2DReferences_Sorted);

            HashSet<int>? countyIds = yOLOTrainingRunOptions.DatasetOptions?.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }
    }
}
