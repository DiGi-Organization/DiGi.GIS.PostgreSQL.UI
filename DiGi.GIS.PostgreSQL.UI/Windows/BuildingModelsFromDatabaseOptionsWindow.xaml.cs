using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.UI.Classes;
using DiGi.UI.WPF.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for BuildingModelsFromDatabaseOptionsWindow.xaml
    /// <para>Asks what decides the reach of a BuildingModel regeneration from the database: which county polygon parts are walked, whether the checkpoint of an earlier run is honoured, and where the checkpoint and the failed-county list are written. The request pacing stays with the task - it is tuning nobody changes between runs, and is set where the task is registered.</para>
    /// <para>No county selected means every county, which is what the task has always meant.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#75), so a run repeated after the tray was restarted is not typed again. Importing fills the controls; nothing runs until OK, and OK validates the imported values like typed ones. The report directory is a machine-specific path: it exports as typed and OK keeps checking that it exists on read-back.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class BuildingModelsFromDatabaseOptionsWindow : Window
    {
        private readonly BuildingModelsFromDatabaseOptions buildingModelsFromDatabaseOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingModelsFromDatabaseOptionsWindow"/> class.
        /// </summary>
        /// <param name="buildingModelsFromDatabaseOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        public BuildingModelsFromDatabaseOptionsWindow(BuildingModelsFromDatabaseOptions? buildingModelsFromDatabaseOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.buildingModelsFromDatabaseOptions = buildingModelsFromDatabaseOptions is null ? new BuildingModelsFromDatabaseOptions() : new BuildingModelsFromDatabaseOptions(buildingModelsFromDatabaseOptions);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
            SetControls(this.buildingModelsFromDatabaseOptions);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public BuildingModelsFromDatabaseOptions BuildingModelsFromDatabaseOptions
        {
            get
            {
                return buildingModelsFromDatabaseOptions;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: this window holds no free numbers, so an export never fails on a value - only the
            // file-level refusals apply. No county selected writes no county list (every county is what the
            // task has always meant, and OK accepts it), and the report directory is written as typed - blank
            // as none, a path that does not exist still exported - because it is a machine-specific string
            // that could be written back as typed; OK keeps checking it on read-back.
            if (!TryReadOptions(false, out BuildingModelsFromDatabaseOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export BuildingModels settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "BuildingModels_FromDatabase_Settings.json"
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
                Title = "Import BuildingModels settings",
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

            if (TryReadSettings(json, out string? message_Read) is not BuildingModelsFromDatabaseOptions buildingModelsFromDatabaseOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SetControls(buildingModelsFromDatabaseOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected - the
            // usual sign the settings came from another server or another machine.
            HashSet<int> countyIds = buildingModelsFromDatabaseOptions_Imported.CountyIds ?? [];
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
        /// <para>The options are written only when every control reads and a report directory that is given exists; a refusal leaves them as they were. The empty county scope is legal here, as it always was - every county is what the task means.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out BuildingModelsFromDatabaseOptions? options, out message) || options is null)
            {
                return false;
            }

            buildingModelsFromDatabaseOptions.Resume = options.Resume;
            buildingModelsFromDatabaseOptions.ReportDirectory = options.ReportDirectory;
            buildingModelsFromDatabaseOptions.CountyIds = options.CountyIds;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) applies the rule the window has always applied - a report directory that is given has to exist (blank means the directory the application was launched from, which the task resolves) - and finishes with that message. Export reads the same controls without it, so a half-prepared window can be saved: a report directory that does not exist exports, because it is a string that could be written back as typed, and no county selected writes no county list - never an empty set. The window holds no free numbers, so an export never fails on a value; only the file-level refusals apply.</para>
        /// </summary>
        /// <param name="complete">True for OK: the report directory that is given has to exist. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out BuildingModelsFromDatabaseOptions? options, out string? message)
        {
            options = null;
            message = null;

            // Written as typed, trimmed, blank as none: blank is what the task resolves to the launch
            // directory, and OK's existence rule stays with it.
            string directory_Text = TextBox_ReportDirectory.Text?.Trim() ?? string.Empty;
            string? reportDirectory = directory_Text == string.Empty ? null : directory_Text;
            if (complete && reportDirectory is not null && !Directory.Exists(reportDirectory))
            {
                message = "Report directory does not exist.";
                return false;
            }

            // No selection is "every county" - the task takes null as the whole table. An empty set would
            // mean "none" and process nothing, so it is never handed over - neither by OK nor by an export.
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

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            BuildingModelsFromDatabaseOptions buildingModelsFromDatabaseOptions_Temp = new(buildingModelsFromDatabaseOptions)
            {
                Resume = CheckBox_Resume.IsChecked == true,
                ReportDirectory = reportDirectory,
                CountyIds = countyIds,
            };

            options = buildingModelsFromDatabaseOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before: options with no county list clear
        /// the selection (every county is what an unscoped run means, shown by SetCounties when the window
        /// opens) and options with no report directory empty the box. Internal so a fact can fill a window from
        /// a settings file the way Import does, without a file dialog.
        /// </summary>
        /// <param name="buildingModelsFromDatabaseOptions">The options to show.</param>
        internal void SetControls(BuildingModelsFromDatabaseOptions buildingModelsFromDatabaseOptions)
        {
            CheckBox_Resume.IsChecked = buildingModelsFromDatabaseOptions.Resume;
            TextBox_ReportDirectory.Text = buildingModelsFromDatabaseOptions.ReportDirectory ?? string.Empty;

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the server on every run. SelectItems deselects the rest,
            // so a file with no county list clears whatever was selected before.
            HashSet<int> countyIds = buildingModelsFromDatabaseOptions.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one BuildingModels-from-database options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static BuildingModelsFromDatabaseOptions? TryReadSettings(string? json, out string? message)
        {
            List<BuildingModelsFromDatabaseOptions>? buildingModelsFromDatabaseOptionsList = Core.Convert.ToDiGi<BuildingModelsFromDatabaseOptions>(json);
            if (buildingModelsFromDatabaseOptionsList is null || buildingModelsFromDatabaseOptionsList.Count != 1)
            {
                message = "The file holds no BuildingModels settings - nothing was changed.";
                return null;
            }

            message = null;

            return buildingModelsFromDatabaseOptionsList[0];
        }

        private void Button_ReportDirectory_Click(object sender, RoutedEventArgs e)
        {
            // The window is on the user interface thread, so a folder dialog opens here - the task body
            // cannot use one, because it runs on a thread pool thread.
            OpenFolderDialog openFolderDialog = new();
            if (openFolderDialog.ShowDialog(this) is not true)
            {
                return;
            }

            TextBox_ReportDirectory.Text = openFolderDialog.FolderName;
        }

        private void ListBoxControl_Counties_ItemAdding(object sender, ListBoxItemAddingEventArgs e)
        {
            if (e.Item is not AdministrativeAreal2DReference administrativeAreal2DReference)
            {
                return;
            }

            // The identifier is shown because it is what the run is keyed by, and because two pieces of the same
            // county are told apart by nothing else - they share their code and their name.
            e.Name = $"{administrativeAreal2DReference.Code} {administrativeAreal2DReference.Name} (id {administrativeAreal2DReference.Id})";
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

            HashSet<int>? countyIds = buildingModelsFromDatabaseOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the server on every run.
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }
    }
}
