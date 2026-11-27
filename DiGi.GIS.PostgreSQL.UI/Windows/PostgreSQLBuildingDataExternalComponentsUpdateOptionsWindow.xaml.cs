using DiGi.GIS.PostgreSQL.Classes;
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
    /// Interaction logic for PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow.xaml
    /// <para>Asks for the settings that decide what a run touches and how long it takes - which counties, the command timeout, the batch size, whether finished buildings are skipped, whether party walls are measured, and how many batches of a county run at once. The run reads the stored building models of the counties in scope, so the counties decide its reach and its cost; left unscoped it walks every county part in the country.</para>
    /// <para>Every county is selected by default, so opening the dialog confirms the national scope rather than inviting it; deselecting everything is still refused at OK, because an empty selection is not the same as no selection and neither is a safe thing to leave this window with by accident. For the same reason an empty selection does not export: the file format has no "none" this window could show again, so Export refuses it with the message OK gives.</para>
    /// <para>Party walls decide most of the cost: with them the run also reads the outlines and the stored models of the neighbours of every batch, without them it only classifies the models of the counties in scope and leaves the party wall columns exactly as they were. The box starts from the options, so it is checked unless a run was configured otherwise.</para>
    /// <para>The parallel batches decide how many batches of one county are read, classified and written at once - 0 takes half of the logical processors, 1 is the sequential run. The stored rows and the counters do not depend on it, but the memory of the run grows with that number times the batch size, so it is worth no more than the host can hold.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#68), so a run repeated after the tray was restarted is not typed again. Importing fills the controls: a file with no county list opens with every county selected, the way the window always shows the unscoped run. Nothing runs until OK, and OK validates the imported values like typed ones.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow : Window
    {
        private readonly PostgreSQLBuildingDataExternalComponentsUpdateOptions postgreSQLBuildingDataExternalComponentsUpdateOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLBuildingDataExternalComponentsUpdateOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own. All of them are selected when the options carry no county set.</param>
        public PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow(PostgreSQLBuildingDataExternalComponentsUpdateOptions? postgreSQLBuildingDataExternalComponentsUpdateOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLBuildingDataExternalComponentsUpdateOptions = postgreSQLBuildingDataExternalComponentsUpdateOptions is null ? new PostgreSQLBuildingDataExternalComponentsUpdateOptions() : new PostgreSQLBuildingDataExternalComponentsUpdateOptions(postgreSQLBuildingDataExternalComponentsUpdateOptions);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
            SetControls(this.postgreSQLBuildingDataExternalComponentsUpdateOptions);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLBuildingDataExternalComponentsUpdateOptions PostgreSQLBuildingDataExternalComponentsUpdateOptions
        {
            get
            {
                return postgreSQLBuildingDataExternalComponentsUpdateOptions;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete for the range rules - a timeout below zero or a batch below one still exports, to be
            // refused by OK on read-back - but the county selection has to be there on both paths: this window
            // shows "no county set" as every county selected, so an emptied list could not be written back as
            // typed, and the file would come back as the national scope. A box that does not hold a whole number
            // is refused too, because it could not be written back as typed.
            if (!TryReadOptions(false, out PostgreSQLBuildingDataExternalComponentsUpdateOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export external components settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "ExternalComponents_Update_Settings.json"
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
                Title = "Import external components settings",
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

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLBuildingDataExternalComponentsUpdateOptions postgreSQLBuildingDataExternalComponentsUpdateOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SetControls(postgreSQLBuildingDataExternalComponentsUpdateOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected - the
            // usual sign the settings came from another machine. An import whose ids match nothing leaves the
            // list empty, which OK then refuses.
            HashSet<int> countyIds = postgreSQLBuildingDataExternalComponentsUpdateOptions_Imported.CountyIds ?? [];
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
        /// <para>The options are written only when every control reads, a county is selected and the whole-number rules pass; a refusal leaves them as they were. The member with no control - the party wall distance - keeps the value the window was opened with.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out PostgreSQLBuildingDataExternalComponentsUpdateOptions? options, out message) || options is null)
            {
                return false;
            }

            postgreSQLBuildingDataExternalComponentsUpdateOptions.CountyIds = options.CountyIds;
            postgreSQLBuildingDataExternalComponentsUpdateOptions.CommandTimeout = options.CommandTimeout;
            postgreSQLBuildingDataExternalComponentsUpdateOptions.BatchSize = options.BatchSize;
            postgreSQLBuildingDataExternalComponentsUpdateOptions.SkipCompleted = options.SkipCompleted;
            postgreSQLBuildingDataExternalComponentsUpdateOptions.PartyWalls = options.PartyWalls;
            postgreSQLBuildingDataExternalComponentsUpdateOptions.MaxDegreeOfParallelism = options.MaxDegreeOfParallelism;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>The county selection is required on both paths, unlike every other window's scope: this one shows "no county set" as every county selected, so an emptied list could not be written back as typed and the file would come back as the national scope - Export refuses it with the message OK gives. The range rules (timeout of at least zero, batch size of at least one, parallel batches of at least zero) are OK's only, so a half-configured window still exports; what is refused on both paths is what could not be written back as typed: a box that does not hold a whole number. The candidate is built on a copy of the window's options, so the member with no control - the party wall distance - travels in the file exactly as the copy holds it and is never changed by a read.</para>
        /// </summary>
        /// <param name="complete">True for OK: every rule applies. False for an export: the ranges are left out, the county selection is still required.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out PostgreSQLBuildingDataExternalComponentsUpdateOptions? options, out string? message)
        {
            options = null;
            message = null;

            // Required on both paths: an empty selection would come back as every county - this window's
            // unscoped run - so it is refused with OK's message rather than written as something else.
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

            if (countyIds is null)
            {
                message = "At least one county has to be selected.";
                return false;
            }

            if (!TextBoxControl_CommandTimeout.TryGetValue(out int commandTimeout) || (complete && commandTimeout < 0))
            {
                message = "Command timeout has to be a whole number of seconds, zero or greater.";
                return false;
            }

            if (!TextBoxControl_BatchSize.TryGetValue(out int batchSize) || (complete && batchSize < 1))
            {
                message = "Batch size has to be a whole number, 1 or greater.";
                return false;
            }

            if (!TextBoxControl_MaxDegreeOfParallelism.TryGetValue(out int maxDegreeOfParallelism) || (complete && maxDegreeOfParallelism < 0))
            {
                message = "Parallel batches has to be a whole number, zero or greater.";
                return false;
            }

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            PostgreSQLBuildingDataExternalComponentsUpdateOptions postgreSQLBuildingDataExternalComponentsUpdateOptions_Temp = new(postgreSQLBuildingDataExternalComponentsUpdateOptions)
            {
                CountyIds = countyIds,
                CommandTimeout = commandTimeout,
                BatchSize = batchSize,
                SkipCompleted = CheckBox_SkipCompleted.IsChecked == true,
                PartyWalls = CheckBox_PartyWalls.IsChecked == true,
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
            };

            options = postgreSQLBuildingDataExternalComponentsUpdateOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before. One rule differs from every other
        /// window: options with no county list select every county, because the unscoped run is what this window
        /// always shows for them; options naming counties select exactly those. Internal so a fact can fill a
        /// window from a settings file the way Import does, without a file dialog.
        /// </summary>
        /// <param name="postgreSQLBuildingDataExternalComponentsUpdateOptions">The options to show.</param>
        internal void SetControls(PostgreSQLBuildingDataExternalComponentsUpdateOptions postgreSQLBuildingDataExternalComponentsUpdateOptions)
        {
            // The caption stays in the XAML - the designer does not run this constructor, and a caption set here
            // leaves an empty label over an empty box in the preview. Only the values come from the options.
            TextBoxControl_CommandTimeout.Value = postgreSQLBuildingDataExternalComponentsUpdateOptions.CommandTimeout.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_BatchSize.Value = postgreSQLBuildingDataExternalComponentsUpdateOptions.BatchSize.ToString(CultureInfo.CurrentCulture);
            CheckBox_SkipCompleted.IsChecked = postgreSQLBuildingDataExternalComponentsUpdateOptions.SkipCompleted;
            CheckBox_PartyWalls.IsChecked = postgreSQLBuildingDataExternalComponentsUpdateOptions.PartyWalls;
            TextBoxControl_MaxDegreeOfParallelism.Value = postgreSQLBuildingDataExternalComponentsUpdateOptions.MaxDegreeOfParallelism.ToString(CultureInfo.CurrentCulture);

            HashSet<int> countyIds = postgreSQLBuildingDataExternalComponentsUpdateOptions.CountyIds ?? [];

            // The run reads every stored model of the counties in scope, so the unscoped run is the expensive one
            // and the default: no county list means every county selected, not an empty selection.
            if (countyIds.Count == 0)
            {
                ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => true);
                return;
            }

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run.
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one external components options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLBuildingDataExternalComponentsUpdateOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLBuildingDataExternalComponentsUpdateOptions>? postgreSQLBuildingDataExternalComponentsUpdateOptionsList = Core.Convert.ToDiGi<PostgreSQLBuildingDataExternalComponentsUpdateOptions>(json);
            if (postgreSQLBuildingDataExternalComponentsUpdateOptionsList is null || postgreSQLBuildingDataExternalComponentsUpdateOptionsList.Count != 1)
            {
                message = "The file holds no external components settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLBuildingDataExternalComponentsUpdateOptionsList[0];
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

            HashSet<int>? countyIds = postgreSQLBuildingDataExternalComponentsUpdateOptions.CountyIds;

            // The run reads every stored model of the counties in scope, so the unscoped run is the expensive one
            // and the default: every county is selected, and deselecting everything is what OK still refuses.
            if (countyIds is null || countyIds.Count == 0)
            {
                ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => true);
                return;
            }

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run.
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }
    }
}
