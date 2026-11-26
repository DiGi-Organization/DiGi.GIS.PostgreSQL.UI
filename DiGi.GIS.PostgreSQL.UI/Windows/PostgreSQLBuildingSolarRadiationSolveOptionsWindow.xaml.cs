using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Solar.Classes;
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
    /// Interaction logic for PostgreSQLBuildingSolarRadiationSolveOptionsWindow.xaml
    /// <para>Asks for everything that decides what a batch solar radiation solve costs and stores: the county parts, the neighbour radii every building is solved for, whether results already stored are stepped over, and the batch, tile, ceiling and timeout settings (DiGi.GIS.PostgreSQL#136).</para>
    /// <para>An optional limit of buildings per county part runs a pilot: empty solves every building (DiGi.GIS.PostgreSQL.UI#45).</para>
    /// <para>The number of solves run at once: 0 is automatic, half the logical processors; 1 is one at a time. It changes the run time only, never the stored results (DiGi.GIS.PostgreSQL.UI#46, DiGi.GIS.PostgreSQL#139).</para>
    /// <para>The counties matter most. A solve costs seconds per building and radius, so an unscoped run is the whole country several times over - the window never leaves with no county chosen.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#59), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLBuildingSolarRadiationSolveOptionsWindow : Window
    {
        private readonly PostgreSQLBuildingSolarRadiationSolveOptions postgreSQLBuildingSolarRadiationSolveOptions;

        private readonly List<double> radii = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLBuildingSolarRadiationSolveOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLBuildingSolarRadiationSolveOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        public PostgreSQLBuildingSolarRadiationSolveOptionsWindow(PostgreSQLBuildingSolarRadiationSolveOptions? postgreSQLBuildingSolarRadiationSolveOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLBuildingSolarRadiationSolveOptions = postgreSQLBuildingSolarRadiationSolveOptions is null ? new PostgreSQLBuildingSolarRadiationSolveOptions() : new PostgreSQLBuildingSolarRadiationSolveOptions(postgreSQLBuildingSolarRadiationSolveOptions);

            // Subscribed before the lists are filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;
            ListBoxControl_Radii.ItemAdding += ListBoxControl_Radii_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
            SetControls(this.postgreSQLBuildingSolarRadiationSolveOptions);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLBuildingSolarRadiationSolveOptions PostgreSQLBuildingSolarRadiationSolveOptions
        {
            get
            {
                return postgreSQLBuildingSolarRadiationSolveOptions;
            }
        }

        private void Button_AddRadius_Click(object sender, RoutedEventArgs e)
        {
            if (!TextBoxControl_Radius.TryGetValue(out double radius) || !double.IsFinite(radius) || radius < 0)
            {
                MessageBox.Show("A radius has to be a number of metres, zero or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!radii.Contains(radius))
            {
                radii.Add(radius);
                SetRadii();
            }

            TextBoxControl_Radius.Value = string.Empty;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: a half-prepared window can be saved and finished after the next import. A value that
            // is there but malformed is still refused, because it could not be written back as typed.
            if (CreateOptions(false, out string? message) is not PostgreSQLBuildingSolarRadiationSolveOptions options)
            {
                MessageBox.Show(message ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export solve building solar radiation settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "SolarRadiation_Solve_Settings.json"
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
                Title = "Import solve building solar radiation settings",
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

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLBuildingSolarRadiationSolveOptions postgreSQLBuildingSolarRadiationSolveOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SetControls(postgreSQLBuildingSolarRadiationSolveOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected, and a
            // missing one is the usual sign the settings came from another machine.
            HashSet<int> countyIds = postgreSQLBuildingSolarRadiationSolveOptions_Imported.CountyIds ?? [];
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
            // The message box and the DialogResult stay here: a fact must be able to accept or refuse the dialog
            // without ShowDialog (a DialogResult needs it) and without a modal box it could hang on.
            if (!TryAccept(out string? message))
            {
                MessageBox.Show(message ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Reads the controls into a new copy of the window's options, telling what cannot be read. The refusal is
        /// returned rather than shown: <see cref="TryAccept(out string)"/> hands it to OK to display, and Export
        /// shows it itself, so a value is never reported twice. Internal so a fact can read a half-filled window the
        /// way Export does, without a file dialog.
        /// </summary>
        /// <param name="complete">True for OK: a county and a radius are required. False for an export: an empty county selection or an empty radii list is kept as no scope, so a half-prepared window can still be saved.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>The options the controls describe, or null when a value could not be read.</returns>
        internal PostgreSQLBuildingSolarRadiationSolveOptions? CreateOptions(bool complete, out string? message)
        {
            message = null;

            PostgreSQLBuildingSolarRadiationSolveOptions options = new(postgreSQLBuildingSolarRadiationSolveOptions);

            HashSet<int>? countyIds = null;
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();
            if (administrativeAreal2DReferences is not null && administrativeAreal2DReferences.Count != 0)
            {
                countyIds = [];
                foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
                {
                    countyIds.Add(administrativeAreal2DReference.Id);
                }
            }
            else if (complete)
            {
                // The task takes null as "every county" and an empty set as "none", so neither is a safe thing to
                // leave this window with by accident.
                message = "At least one county has to be selected.";
                return null;
            }

            // Empty on an export is saved as no radius rather than refused: OK refuses the file it answers back the
            // same way it refuses a window whose radii list was cleared by hand.
            if (radii.Count == 0 && complete)
            {
                message = "At least one radius has to be given.";
                return null;
            }

            if (!TextBoxControl_BatchSize.TryGetValue(out int batchSize) || batchSize < 1)
            {
                message = "Batch size has to be a whole number, 1 or greater.";
                return null;
            }

            if (!TextBoxControl_TileSize.TryGetValue(out double tileSize) || !double.IsFinite(tileSize) || tileSize < 1)
            {
                message = "Tile size has to be a number of metres, 1 or greater.";
                return null;
            }

            if (!TextBoxControl_ReceiverCountMax.TryGetValue(out int receiverCountMax) || receiverCountMax < 1)
            {
                message = "The receiving surface ceiling has to be a whole number, 1 or greater.";
                return null;
            }

            if (!TextBoxControl_CasterTriangleCountMax.TryGetValue(out int casterTriangleCountMax) || casterTriangleCountMax < 1)
            {
                message = "The shading triangle ceiling has to be a whole number, 1 or greater.";
                return null;
            }

            if (!TextBoxControl_CommandTimeout.TryGetValue(out int commandTimeout) || commandTimeout < 0)
            {
                message = "Command timeout has to be a whole number of seconds, zero or greater.";
                return null;
            }

            if (!TextBoxControl_MaxDegreeOfParallelism.TryGetValue(out int maxDegreeOfParallelism) || maxDegreeOfParallelism < 0)
            {
                message = "Parallel solves has to be a whole number, zero (automatic) or greater.";
                return null;
            }

            int? buildingCountMax = null;
            if (!string.IsNullOrWhiteSpace(TextBoxControl_BuildingCountMax.Value))
            {
                if (!TextBoxControl_BuildingCountMax.TryGetValue(out int buildingCountMax_Value) || buildingCountMax_Value < 1)
                {
                    message = "Buildings per part has to be empty or a whole number, 1 or greater.";
                    return null;
                }

                buildingCountMax = buildingCountMax_Value;
            }

            options.CountyIds = countyIds;
            options.Radii = [.. radii];
            options.SkipExisting = CheckBox_SkipExisting.IsChecked == true;
            options.BatchSize = batchSize;
            options.TileSize = tileSize;
            options.ReceiverCountMax = receiverCountMax;
            options.CasterTriangleCountMax = casterTriangleCountMax;
            options.CommandTimeout = commandTimeout;
            options.BuildingCountMax = buildingCountMax;
            options.MaxDegreeOfParallelism = maxDegreeOfParallelism;

            return options;
        }

        /// <summary>
        /// Validates the controls and, when they hold together, carries them into the options the window holds: the
        /// selected counties, the radii list as edited, and the batch, tile, ceiling, timeout, pilot-limit and
        /// parallelism settings. On refusal nothing is written and the reason is returned for the caller to show
        /// (DiGi.GIS.PostgreSQL#158).
        /// </summary>
        /// <param name="message">The reason the dialog cannot be accepted, or null when it can.</param>
        /// <returns>True when the options were written; false when a control was refused, leaving them untouched.</returns>
        internal bool TryAccept(out string? message)
        {
            if (CreateOptions(true, out message) is not PostgreSQLBuildingSolarRadiationSolveOptions options)
            {
                return false;
            }

            postgreSQLBuildingSolarRadiationSolveOptions.CountyIds = options.CountyIds;
            postgreSQLBuildingSolarRadiationSolveOptions.Radii = options.Radii;
            postgreSQLBuildingSolarRadiationSolveOptions.SkipExisting = options.SkipExisting;
            postgreSQLBuildingSolarRadiationSolveOptions.BatchSize = options.BatchSize;
            postgreSQLBuildingSolarRadiationSolveOptions.TileSize = options.TileSize;
            postgreSQLBuildingSolarRadiationSolveOptions.ReceiverCountMax = options.ReceiverCountMax;
            postgreSQLBuildingSolarRadiationSolveOptions.CasterTriangleCountMax = options.CasterTriangleCountMax;
            postgreSQLBuildingSolarRadiationSolveOptions.CommandTimeout = options.CommandTimeout;
            postgreSQLBuildingSolarRadiationSolveOptions.BuildingCountMax = options.BuildingCountMax;
            postgreSQLBuildingSolarRadiationSolveOptions.MaxDegreeOfParallelism = options.MaxDegreeOfParallelism;

            return true;
        }

        private void Button_RemoveRadii_Click(object sender, RoutedEventArgs e)
        {
            List<double>? radii_Selected = ListBoxControl_Radii.GetItems<double>();
            if (radii_Selected is null || radii_Selected.Count == 0)
            {
                return;
            }

            radii.RemoveAll(radii_Selected.Contains);
            SetRadii();
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

        private void ListBoxControl_Radii_ItemAdding(object sender, ListBoxItemAddingEventArgs e)
        {
            if (e.Item is not double radius)
            {
                return;
            }

            // 0 is not "no radius": it is the building alone, the reference the urban-context effect is measured
            // against (DiGi.GIS.PostgreSQL#107), and it is easy to drop by mistake if it is not named.
            e.Name = radius == 0 ? "0 m - isolated building (no neighbours)" : string.Format(CultureInfo.CurrentCulture, "{0} m", radius);
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before. The captions stay in the XAML: the
        /// designer does not run this code, only the values do. Internal so a fact can fill a window from a settings
        /// file the way Import does, without a file dialog.
        /// </summary>
        /// <param name="options">The options to show.</param>
        internal void SetControls(PostgreSQLBuildingSolarRadiationSolveOptions options)
        {
            CheckBox_SkipExisting.IsChecked = options.SkipExisting;
            TextBoxControl_BatchSize.Value = options.BatchSize.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_TileSize.Value = options.TileSize.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_ReceiverCountMax.Value = options.ReceiverCountMax.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_CasterTriangleCountMax.Value = options.CasterTriangleCountMax.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_CommandTimeout.Value = options.CommandTimeout.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_MaxDegreeOfParallelism.Value = options.MaxDegreeOfParallelism.ToString(CultureInfo.CurrentCulture);

            // Empty is "every building", not zero - the option is null then.
            int? buildingCountMax = options.BuildingCountMax;
            TextBoxControl_BuildingCountMax.Value = buildingCountMax is int buildingCountMax_Value ? buildingCountMax_Value.ToString(CultureInfo.CurrentCulture) : string.Empty;

            // Stripped of what the solve would ignore anyway - a radius that is not a finite, non-negative number,
            // and a duplicate - then ordered by SetRadii. A stripped value shows as much in the list.
            radii.Clear();
            foreach (double radius in options.Radii ?? [])
            {
                if (double.IsFinite(radius) && radius >= 0 && !radii.Contains(radius))
                {
                    radii.Add(radius);
                }
            }

            SetRadii();

            // Matched on the identifier rather than on the references themselves: the options carry identifiers, and
            // the references are read afresh on every run. Nothing is preselected when the options name no county:
            // the scope is required, and an empty selection is what OK refuses.
            HashSet<int> countyIds = options.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
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

            // By code, so that the counties of one voivodeship sit together; by identifier within a code, because
            // the pieces of a multi-part county share their code and their name.
            administrativeAreal2DReferences_Sorted.Sort((x, y) =>
            {
                int result = string.CompareOrdinal(x.Code ?? string.Empty, y.Code ?? string.Empty);

                return result != 0 ? result : x.Id.CompareTo(y.Id);
            });

            ListBoxControl_Counties.SetItems(administrativeAreal2DReferences_Sorted);
        }

        private void SetRadii()
        {
            radii.Sort();

            ListBoxControl_Radii.SetItems(radii);
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one solve options object is accepted: a file of another
        /// window's settings answers a different type name and cannot be read as this one, and a file holding several
        /// objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLBuildingSolarRadiationSolveOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLBuildingSolarRadiationSolveOptions>? postgreSQLBuildingSolarRadiationSolveOptionsList = Core.Convert.ToDiGi<PostgreSQLBuildingSolarRadiationSolveOptions>(json);
            if (postgreSQLBuildingSolarRadiationSolveOptionsList is null || postgreSQLBuildingSolarRadiationSolveOptionsList.Count != 1)
            {
                message = "The file holds no solve building solar radiation settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLBuildingSolarRadiationSolveOptionsList[0];
        }
    }
}
