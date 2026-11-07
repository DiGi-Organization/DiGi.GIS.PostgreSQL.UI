using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.UI.Enums;
using DiGi.GIS.YOLO.UI.Classes;
using DiGi.GIS.YOLO.UI.Enums;
using DiGi.UI.WPF.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

            YOLOTrainingDatasetOptions? yOLOTrainingDatasetOptions = this.yOLOTrainingRunOptions.DatasetOptions;

            // Selected before the handler is attached, so opening the dialog is not taken for a switch.
            ComboBox_Scenario.SelectedIndex = yOLOTrainingScenario == YOLOTrainingScenario.Fresh ? 1 : 0;
            ComboBox_Scenario.SelectionChanged += ComboBox_Scenario_SelectionChanged;

            TextBoxControl_DatasetDirectory.Value = yOLOTrainingDatasetOptions?.OutputDirectory;
            TextBoxControl_StartWeightsPath.Value = this.yOLOTrainingRunOptions.StartWeightsPath;
            TextBoxControl_Epochs.Value = this.yOLOTrainingRunOptions.Epochs.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_Patience.Value = this.yOLOTrainingRunOptions.Patience.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_ImageSize.Value = this.yOLOTrainingRunOptions.ImageSize.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_Batch.Value = this.yOLOTrainingRunOptions.Batch.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_Seed.Value = this.yOLOTrainingRunOptions.Seed.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_Device.Value = this.yOLOTrainingRunOptions.Device;
            TextBoxControl_AutoResumeCount.Value = this.yOLOTrainingRunOptions.AutoResumeCount.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_InactivityTimeout.Value = this.yOLOTrainingRunOptions.InactivityTimeout is TimeSpan inactivityTimeout ? ((int)inactivityTimeout.TotalMinutes).ToString(CultureInfo.InvariantCulture) : null;
            TextBoxControl_RunName.Value = this.yOLOTrainingRunOptions.RunName;
            TextBoxControl_ProjectDirectory.Value = this.yOLOTrainingRunOptions.ProjectDirectory;
            TextBoxControl_PythonPath.Value = this.yOLOTrainingRunOptions.PythonPath ?? yOLOTrainingDatasetOptions?.PythonPath;
            TextBoxControl_WorkingDirectory.Value = this.yOLOTrainingRunOptions.WorkingDirectory ?? yOLOTrainingDatasetOptions?.WorkingDirectory;
            TextBoxControl_WeightsPaths.Value = yOLOTrainingDatasetOptions?.WeightsPaths is List<string> weightsPaths ? string.Join("; ", weightsPaths) : null;

            // The exclusion of the pilot test areas (DiGi.GIS.PostgreSQL.UI#39), shown as stored so reopening the
            // dialog shows what the next build will exclude.
            TextBoxControl_ExclusionBuffer.Value = (yOLOTrainingDatasetOptions ?? new YOLOTrainingDatasetOptions()).ExclusionBuffer.ToString("R", CultureInfo.InvariantCulture);
            TextBox_ExclusionBoundingBoxes.Text = yOLOTrainingDatasetOptions?.ExclusionBoundingBoxes.ToSystem_String() ?? string.Empty;

            // The runner reads an empty list as every step, and so does the dialog.
            List<YOLOTrainingStep>? yOLOTrainingSteps = this.yOLOTrainingRunOptions.Steps;
            bool Checked(YOLOTrainingStep yOLOTrainingStep)
            {
                return yOLOTrainingSteps is null || yOLOTrainingSteps.Count == 0 || yOLOTrainingSteps.Contains(yOLOTrainingStep);
            }

            CheckBox_Dataset.IsChecked = Checked(YOLOTrainingStep.Dataset);
            CheckBox_LabelCheck.IsChecked = Checked(YOLOTrainingStep.LabelCheck);
            CheckBox_Train.IsChecked = Checked(YOLOTrainingStep.Train);
            CheckBox_Validate.IsChecked = Checked(YOLOTrainingStep.Validate);
            CheckBox_Evaluate.IsChecked = Checked(YOLOTrainingStep.Evaluate);

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

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            string title = Title ?? string.Empty;

            void Warn(string message)
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();

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

            // The runner reads an empty list as every step, so no step ticked would start the whole run.
            if (yOLOTrainingSteps.Count == 0)
            {
                Warn("At least one step has to be selected.");
                return;
            }

            // The counties are what the dataset step builds from; without it they scope nothing, but they are still
            // what the confirmation names back, so a run with none is refused rather than guessed at.
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                Warn("At least one county has to be selected.");
                return;
            }

            if (!TextBoxControl_Epochs.TryGetValue(out int epochs) || epochs < 1)
            {
                Warn("Epochs has to be a whole number of at least one.");
                return;
            }

            if (!TextBoxControl_Patience.TryGetValue(out int patience) || patience < 0)
            {
                Warn("Patience has to be a whole number of at least zero.");
                return;
            }

            if (!TextBoxControl_ImageSize.TryGetValue(out int imageSize) || imageSize < 32)
            {
                Warn("Image size has to be a whole number of at least 32.");
                return;
            }

            if (!TextBoxControl_Batch.TryGetValue(out int batch) || batch < 1)
            {
                Warn("Batch has to be a whole number of at least one.");
                return;
            }

            if (!TextBoxControl_Seed.TryGetValue(out int seed) || seed < 0)
            {
                Warn("Seed has to be a whole number of at least zero.");
                return;
            }

            if (!TextBoxControl_AutoResumeCount.TryGetValue(out int autoResumeCount))
            {
                Warn("Automatic resumes has to be a whole number from 0 to 10.");
                return;
            }

            TimeSpan? inactivityTimeout = null;
            if (TextBoxControl_InactivityTimeout.TryGetValue(out int inactivityTimeoutMinutes))
            {
                inactivityTimeout = TimeSpan.FromMinutes(inactivityTimeoutMinutes);
            }
            else if (!string.IsNullOrWhiteSpace(TextBoxControl_InactivityTimeout.Value))
            {
                Warn("The stall limit has to be a whole number of at least one minute, or empty to use the default.");
                return;
            }

            if (!Query.IsYOLOTrainingStallOptionsValid(autoResumeCount, inactivityTimeout, out string? stallReason))
            {
                Warn(stallReason!);
                return;
            }

            string? datasetDirectory = Value(TextBoxControl_DatasetDirectory.Value);
            if (datasetDirectory is null)
            {
                // Every step reads or writes the dataset folder, so it is asked for whichever steps are ticked.
                Warn("A dataset directory has to be given.");
                return;
            }

            bool train = yOLOTrainingSteps.Contains(YOLOTrainingStep.Train);

            bool resume = yOLOCheckpointInformation is not null && CheckBox_ResumeInterruptedRun.IsChecked == true;
            if (resume && !train)
            {
                Warn("A resume needs the Training step - the training is the step it continues.");
                return;
            }

            if (resume && yOLOTrainingSteps.Contains(YOLOTrainingStep.Dataset))
            {
                Warn("A resume never rebuilds the dataset it was trained on.");
                return;
            }

            string? startWeightsPath = Value(TextBoxControl_StartWeightsPath.Value);
            if (startWeightsPath is null && (train || yOLOTrainingSteps.Contains(YOLOTrainingStep.Validate)))
            {
                Warn("Start weights have to be given for training or validation.");
                return;
            }

            string? runName = Value(TextBoxControl_RunName.Value);
            string? projectDirectory = Value(TextBoxControl_ProjectDirectory.Value);
            if (train && (runName is null || projectDirectory is null))
            {
                Warn("A run name and a project directory have to be given for training.");
                return;
            }

            // A relative directory would otherwise be resolved against this process's current directory, where the
            // runner would not look for it, so it is refused here rather than silently mis-scoped.
            if (DirectoryRefusal(datasetDirectory, projectDirectory, train) is string directoryRefusal)
            {
                Warn(directoryRefusal);
                return;
            }

            if (!Query.TryParseYOLOExclusionBuffer(TextBoxControl_ExclusionBuffer.Value, out double exclusionBuffer, out string? exclusionBufferReason))
            {
                Warn(exclusionBufferReason!);
                return;
            }

            if (!Query.TryParseYOLOExclusionBoundingBoxes(TextBox_ExclusionBoundingBoxes.Text, out List<YOLOExclusionBoundingBox>? yOLOExclusionBoundingBoxes, out string? exclusionBoundingBoxesReason))
            {
                Warn(exclusionBoundingBoxesReason!);
                return;
            }

            HashSet<int> countyIds = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                countyIds.Add(administrativeAreal2DReference.Id);
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
            // to - and the controls then overwrite the defaults it set, so what was typed is what runs.
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

            yOLOTrainingRunOptions = yOLOTrainingRunOptions_Result;
            yOLOTrainingScenario = yOLOTrainingScenario_Selected;

            DialogResult = true;
            Close();
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
