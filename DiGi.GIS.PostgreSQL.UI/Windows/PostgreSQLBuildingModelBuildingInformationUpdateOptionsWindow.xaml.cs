using DiGi.Analytical.Building.Enums;
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
    /// Interaction logic for PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow.xaml
    /// <para>Asks what decides the reach of a BuildingInformation stamping run: which county polygon parts are walked, which detail level, the page size, the statement timeout, where the checkpoint and the reports are written, and whether a run that has already been started resumes or restarts.</para>
    /// <para>Every county is selected by default, so opening the dialog confirms the national scope rather than inviting it; deselecting everything is still refused at OK, because an empty selection is not the same as no selection and neither is a safe thing to leave this window with by accident.</para>
    /// <para>The dry run checkbox is checked by default: the run reports by default and writes nothing until it is turned off, and the window is where the operator makes that decision.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#73), so a run repeated after the tray was restarted is not typed again. Importing fills the controls; a file with no county list leaves the selection empty, which OK then refuses exactly as a typed empty selection. Nothing runs until OK.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow : Window
    {
        private readonly PostgreSQLBuildingModelBuildingInformationUpdateOptions postgreSQLBuildingModelBuildingInformationUpdateOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLBuildingModelBuildingInformationUpdateOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own. All of them are selected when the options carry no county set.</param>
        public PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow(PostgreSQLBuildingModelBuildingInformationUpdateOptions? postgreSQLBuildingModelBuildingInformationUpdateOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLBuildingModelBuildingInformationUpdateOptions = postgreSQLBuildingModelBuildingInformationUpdateOptions is null ? new PostgreSQLBuildingModelBuildingInformationUpdateOptions() : new PostgreSQLBuildingModelBuildingInformationUpdateOptions(postgreSQLBuildingModelBuildingInformationUpdateOptions);

            // The non-county controls first: the county list does not exist yet, so SetControls' selection
            // pass is a no-op here and SetCounties - which fills the list - decides the opening selection:
            // every county for an unscoped run, exactly the ids the options name otherwise. The order also
            // makes an import behave the other way round: the list exists by then, and a file with no county
            // list clears the selection, which OK refuses.
            SetControls(this.postgreSQLBuildingModelBuildingInformationUpdateOptions);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLBuildingModelBuildingInformationUpdateOptions PostgreSQLBuildingModelBuildingInformationUpdateOptions
        {
            get
            {
                return postgreSQLBuildingModelBuildingInformationUpdateOptions;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: a half-prepared window can be saved and finished after the next import - no county
            // selected writes no county list, the range rules are OK's gate, and the report directory is
            // written as typed (blank as none): a path that does not exist exports and OK refuses it on
            // read-back. What is refused on both paths is what could not be written back as typed: a batch
            // size or timeout that is not a whole number.
            if (!TryReadOptions(false, out PostgreSQLBuildingModelBuildingInformationUpdateOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export BuildingInformation settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "BuildingInformation_Update_Settings.json"
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
                Title = "Import BuildingInformation settings",
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

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLBuildingModelBuildingInformationUpdateOptions postgreSQLBuildingModelBuildingInformationUpdateOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Only the controls are filled: a file with no county list clears the selection (the list exists
            // by now), which OK refuses exactly as a typed empty selection; the select-all opening default is
            // SetCounties' rule and applies only when the window opens.
            SetControls(postgreSQLBuildingModelBuildingInformationUpdateOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected - the
            // usual sign the settings came from another machine.
            HashSet<int> countyIds = postgreSQLBuildingModelBuildingInformationUpdateOptions_Imported.CountyIds ?? [];
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
        /// <para>The options are written only when every control reads and passes the full set of rules OK has always applied - at least one county, a batch size of at least one, a timeout of at least zero, a report directory that exists when one is given; a refusal leaves them as they were.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out PostgreSQLBuildingModelBuildingInformationUpdateOptions? options, out message) || options is null)
            {
                return false;
            }

            postgreSQLBuildingModelBuildingInformationUpdateOptions.CountyIds = options.CountyIds;
            postgreSQLBuildingModelBuildingInformationUpdateOptions.BuildingModelDetailLevel = options.BuildingModelDetailLevel;
            postgreSQLBuildingModelBuildingInformationUpdateOptions.BatchSize = options.BatchSize;
            postgreSQLBuildingModelBuildingInformationUpdateOptions.CommandTimeout = options.CommandTimeout;
            postgreSQLBuildingModelBuildingInformationUpdateOptions.ReportDirectory = options.ReportDirectory;
            postgreSQLBuildingModelBuildingInformationUpdateOptions.Resume = options.Resume;
            postgreSQLBuildingModelBuildingInformationUpdateOptions.DryRun = options.DryRun;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) applies every rule the window has always applied, in the order it has always applied them, and finishes with those messages. Export reads the same controls without those requirements, so a half-prepared window can be saved: no county selected is written as no county list, and a report directory that does not exist still exports - it is a string that could be written back as typed - while OK keeps refusing it on read-back. What is refused on both paths is what could not be written back as typed: a batch size or timeout that is not a whole number. Every member of this window's options has a control, so the round trip is complete.</para>
        /// </summary>
        /// <param name="complete">True for OK: every rule applies. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out PostgreSQLBuildingModelBuildingInformationUpdateOptions? options, out string? message)
        {
            options = null;
            message = null;

            // No selection is written as no county list (null); an empty set would mean "none" and is never
            // handed over - neither by OK nor by an export.
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

            if (!TextBoxControl_BatchSize.TryGetValue(out int batchSize) || (complete && batchSize < 1))
            {
                message = "Batch size has to be a whole number, 1 or greater.";
                return false;
            }

            if (!TextBoxControl_CommandTimeout.TryGetValue(out int commandTimeout) || (complete && commandTimeout < 0))
            {
                message = "Command timeout has to be a whole number of seconds, zero or greater.";
                return false;
            }

            // Written as typed, trimmed, blank as none: OK's existence rule stays with it, so a path that does
            // not exist exports and is refused on read-back.
            string reportDirectory = (TextBoxControl_ReportDirectory.Value ?? string.Empty).Trim();
            if (complete && reportDirectory != string.Empty && !Directory.Exists(reportDirectory))
            {
                message = $"The report directory does not exist: {reportDirectory}";
                return false;
            }

            // "All" is index zero, which is also the default the combo opens with when no level is set.
            BuildingModelDetailLevel? buildingModelDetailLevel = ComboBox_DetailLevel.SelectedIndex switch
            {
                1 => BuildingModelDetailLevel.Component,
                2 => BuildingModelDetailLevel.Envelope,
                _ => null
            };

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            PostgreSQLBuildingModelBuildingInformationUpdateOptions postgreSQLBuildingModelBuildingInformationUpdateOptions_Temp = new(postgreSQLBuildingModelBuildingInformationUpdateOptions)
            {
                CountyIds = countyIds,
                BuildingModelDetailLevel = buildingModelDetailLevel,
                BatchSize = batchSize,
                CommandTimeout = commandTimeout,
                ReportDirectory = reportDirectory == string.Empty ? null : reportDirectory,
                Resume = CheckBox_Resume.IsChecked == true,
                DryRun = CheckBox_DryRun.IsChecked == true,
            };

            options = postgreSQLBuildingModelBuildingInformationUpdateOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening (before the county list exists, so SetCounties
        /// still decides the opening selection) and again on an import, where the list is there and a value the
        /// file leaves out clears its control: options with no county list clear the selection, which OK then
        /// refuses. Internal so a fact can fill a window from a settings file the way Import does, without a
        /// file dialog.
        /// </summary>
        /// <param name="postgreSQLBuildingModelBuildingInformationUpdateOptions">The options to show.</param>
        internal void SetControls(PostgreSQLBuildingModelBuildingInformationUpdateOptions postgreSQLBuildingModelBuildingInformationUpdateOptions)
        {
            // The caption stays in the XAML - the designer does not run this constructor, and a caption set here
            // leaves an empty label over an empty box in the preview. Only the values come from the options.
            TextBoxControl_BatchSize.Value = postgreSQLBuildingModelBuildingInformationUpdateOptions.BatchSize.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_CommandTimeout.Value = postgreSQLBuildingModelBuildingInformationUpdateOptions.CommandTimeout.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_ReportDirectory.Value = postgreSQLBuildingModelBuildingInformationUpdateOptions.ReportDirectory ?? string.Empty;
            CheckBox_Resume.IsChecked = postgreSQLBuildingModelBuildingInformationUpdateOptions.Resume;
            CheckBox_DryRun.IsChecked = postgreSQLBuildingModelBuildingInformationUpdateOptions.DryRun;

            // "All" is index zero, which is also the default the combo opens with when no level is set.
            ComboBox_DetailLevel.SelectedIndex = postgreSQLBuildingModelBuildingInformationUpdateOptions.BuildingModelDetailLevel switch
            {
                BuildingModelDetailLevel.Component => 1,
                BuildingModelDetailLevel.Envelope => 2,
                _ => 0
            };

            // Matched on the identifier rather than on the references themselves: the options carry
            // identifiers, and the references are read afresh on every run. SelectItems deselects the rest; on
            // the opening call the list does not exist yet and this is a no-op, leaving the choice to
            // SetCounties, whose rule for an unscoped run is every county selected.
            HashSet<int> countyIds = postgreSQLBuildingModelBuildingInformationUpdateOptions.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one BuildingInformation update options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLBuildingModelBuildingInformationUpdateOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLBuildingModelBuildingInformationUpdateOptions>? postgreSQLBuildingModelBuildingInformationUpdateOptionsList = Core.Convert.ToDiGi<PostgreSQLBuildingModelBuildingInformationUpdateOptions>(json);
            if (postgreSQLBuildingModelBuildingInformationUpdateOptionsList is null || postgreSQLBuildingModelBuildingInformationUpdateOptionsList.Count != 1)
            {
                message = "The file holds no BuildingInformation settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLBuildingModelBuildingInformationUpdateOptionsList[0];
        }

        private void ListBoxControl_Counties_ItemAdding(object sender, ListBoxItemAddingEventArgs e)
        {
            if (e.Item is not AdministrativeAreal2DReference administrativeAreal2DReference)
            {
                return;
            }

            // The identifier is shown because it is what the run is keyed by, and because two pieces of the
            // same county are told apart by nothing else - they share their code and their name.
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
            // because the pieces of a multi-part county share their code and their name and the identifier
            // is all that separates them.
            administrativeAreal2DReferences_Sorted.Sort((x, y) =>
            {
                int result = string.CompareOrdinal(x.Code ?? string.Empty, y.Code ?? string.Empty);

                return result != 0 ? result : x.Id.CompareTo(y.Id);
            });

            ListBoxControl_Counties.SetItems(administrativeAreal2DReferences_Sorted);

            HashSet<int>? countyIds = postgreSQLBuildingModelBuildingInformationUpdateOptions.CountyIds;

            // The unscoped run is the national pass, so every county is selected by default, and
            // deselecting everything is what OK still refuses.
            if (countyIds is null || countyIds.Count == 0)
            {
                ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => true);
                return;
            }

            // Matched on the identifier rather than on the references themselves: the options carry
            // identifiers, and the references are read afresh from the database on every run.
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }
    }
}
