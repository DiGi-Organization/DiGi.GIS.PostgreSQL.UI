using DiGi.GIS.PostgreSQL.Classes;
using DiGi.Geometry.Planar.Classes;
using DiGi.UI.WPF;
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
    /// Interaction logic for OpenBuildingImportOptionsWindow.xaml
    /// <para>Asks what decides a run of the open building import (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#38): which source files the release came in, whether the scope is a county list or a bounding box, and the release metadata the register stores. The dataset itself is not asked - the row the operator started fixes it, so a row named for one release cannot import another.</para>
    /// <para>The SHA-256 is never typed. The field is shown disabled with the caption saying when it is computed: the wrapper hashes the source files after this dialog closes and before the run starts.</para>
    /// <para>Exactly one scope form has to be filled - a county selection or the four coordinates of an EPSG:2180 box - and OK still refuses when either is missing or both are given, because both is ambiguous and neither has no run. What the core task enforces is asked here first through <see cref="Query.OpenBuildingImportOptionsMessage(PostgreSQLOpenBuildingImportTaskOptions)"/>, so the operator is told while the dialog is open rather than from a failed run's log.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#58), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones. A file written for another dataset is refused, because the row fixes the dataset.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class OpenBuildingImportOptionsWindow : Window
    {
        private PostgreSQLOpenBuildingImportTaskOptions postgreSQLOpenBuildingImportTaskOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenBuildingImportOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLOpenBuildingImportTaskOptions">The options the controls are filled from. When null the defaults are used, and the dataset row then reports that no dataset was chosen.</param>
        /// <param name="administrativeAreal2DReferences">The county parts to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own. Nothing is preselected: the scope is required, and a dialog that opens with every county ticked invites a national run as the default.</param>
        public OpenBuildingImportOptionsWindow(PostgreSQLOpenBuildingImportTaskOptions? postgreSQLOpenBuildingImportTaskOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLOpenBuildingImportTaskOptions = postgreSQLOpenBuildingImportTaskOptions is null ? new PostgreSQLOpenBuildingImportTaskOptions() : new PostgreSQLOpenBuildingImportTaskOptions(postgreSQLOpenBuildingImportTaskOptions);

            TextBlock_Dataset.Text = DisplayName(this.postgreSQLOpenBuildingImportTaskOptions.DatasetKey);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);

            SetControls(this.postgreSQLOpenBuildingImportTaskOptions);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLOpenBuildingImportTaskOptions PostgreSQLOpenBuildingImportTaskOptions
        {
            get
            {
                return postgreSQLOpenBuildingImportTaskOptions;
            }
        }

        private void Button_AddFiles_Click(object sender, RoutedEventArgs e)
        {
            // The filter comes from the dataset's file type, with All Files as the second entry (#55).
            OpenFileDialog openFileDialog = new()
            {
                Title = string.Format(CultureInfo.CurrentCulture, "Select {0}", Query.OpenBuildingFileType(postgreSQLOpenBuildingImportTaskOptions.DatasetKey).FileTypeName),
                Filter = Query.OpenBuildingFileFilter(postgreSQLOpenBuildingImportTaskOptions.DatasetKey),
                FilterIndex = 1,
                Multiselect = true
            };

            if (openFileDialog.ShowDialog(this) != true || openFileDialog.FileNames.Length == 0)
            {
                return;
            }

            AddSourcePaths(openFileDialog.FileNames);
        }

        private void Button_AddFolder_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new()
            {
                Title = string.Format(CultureInfo.CurrentCulture, "Select the folder holding {0} files", Query.OpenBuildingFileType(postgreSQLOpenBuildingImportTaskOptions.DatasetKey).FileTypeName)
            };

            if (openFolderDialog.ShowDialog(this) != true)
            {
                return;
            }

            List<string>? paths_Added = Query.OpenBuildingSourcePaths(postgreSQLOpenBuildingImportTaskOptions.DatasetKey, openFolderDialog.FolderName);
            if (paths_Added is null || paths_Added.Count == 0)
            {
                // Said, not ignored: an empty answer looks like a button that did nothing.
                MessageBox.Show(this, "No file of this dataset's type was found below the folder.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            AddSourcePaths(paths_Added);
        }

        /// <summary>
        /// Adds paths to the source list, skipping the ones it already holds, and keeps the list in the ordinal order the options keep.
        /// </summary>
        /// <param name="paths_Added">The paths to add.</param>
        private void AddSourcePaths(IEnumerable<string> paths_Added)
        {
            List<string> paths = ListBoxControl_SourcePaths.GetItems<string>(false) ?? [];
            HashSet<string> paths_HashSet = [.. paths];

            foreach (string path in paths_Added)
            {
                if (paths_HashSet.Add(path))
                {
                    paths.Add(path);
                }
            }

            paths.Sort(StringComparer.Ordinal);
            ListBoxControl_SourcePaths.SetItems(paths);
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
            if (CreateOptions(false) is not PostgreSQLOpenBuildingImportTaskOptions options)
            {
                return;
            }

            // Computed from the files at every run; a stored digest would describe files that may have changed.
            options.Sha256 = null;

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export open building import settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = $"OpenBuilding_Import_Settings_{(options.DatasetKey ?? "dataset").Trim().ToLowerInvariant()}.json"
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
                Title = "Import open building import settings",
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

            List<PostgreSQLOpenBuildingImportTaskOptions>? postgreSQLOpenBuildingImportTaskOptionsList = Core.Convert.ToDiGi<PostgreSQLOpenBuildingImportTaskOptions>(json);
            if (postgreSQLOpenBuildingImportTaskOptionsList is null || postgreSQLOpenBuildingImportTaskOptionsList.Count != 1)
            {
                MessageBox.Show(this, "The file holds no open building import settings - nothing was changed.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PostgreSQLOpenBuildingImportTaskOptions options = postgreSQLOpenBuildingImportTaskOptionsList[0];

            // The row fixes the dataset: settings written for another release would import its files into
            // this one's register row.
            string? datasetKey = postgreSQLOpenBuildingImportTaskOptions.DatasetKey?.Trim();
            string? datasetKey_Imported = options.DatasetKey?.Trim();
            if (!string.IsNullOrEmpty(datasetKey_Imported) && !string.Equals(datasetKey_Imported, datasetKey, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, $"The settings were exported for {DisplayName(datasetKey_Imported)}, and this window imports {DisplayName(datasetKey)} - nothing was changed.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            options.DatasetKey = postgreSQLOpenBuildingImportTaskOptions.DatasetKey;
            options.Sha256 = null;

            SetControls(options);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected, and a
            // missing file is the usual sign the settings came from another machine.
            List<string> notes = [];

            if (options.BoundingBox2D is null && options.CountyIds is HashSet<int> countyIds && countyIds.Count != 0)
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
                    notes.Add(string.Create(CultureInfo.InvariantCulture, $"{countyIds_Missing.Count} county part(s) of the file are not in the list and were not selected: {string.Join(", ", countyIds_Missing)}"));
                }
            }

            int missingFileCount = 0;
            foreach (string path in options.SourcePaths ?? [])
            {
                if (!File.Exists(path))
                {
                    missingFileCount++;
                }
            }

            if (missingFileCount != 0)
            {
                notes.Add(string.Create(CultureInfo.InvariantCulture, $"{missingFileCount} source file(s) of the file do not exist on this machine."));
            }

            if (notes.Count != 0)
            {
                MessageBox.Show(this, string.Join(Environment.NewLine + Environment.NewLine, notes), Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (CreateOptions(true) is not PostgreSQLOpenBuildingImportTaskOptions options)
            {
                return;
            }

            // The last gate before OK returns: the same rules the core task enforces, asked while the
            // dialog can still be corrected instead of after a run has been started and refused.
            if (Query.OpenBuildingImportOptionsMessage(options) is string message)
            {
                MessageBox.Show(this, message, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            postgreSQLOpenBuildingImportTaskOptions = options;

            DialogResult = true;
            Close();
        }

        private void Button_RemoveFiles_Click(object sender, RoutedEventArgs e)
        {
            List<string>? paths_Selected = ListBoxControl_SourcePaths.GetItems<string>();
            if (paths_Selected is null || paths_Selected.Count == 0)
            {
                return;
            }

            HashSet<string> paths_Selected_HashSet = [.. paths_Selected];

            List<string> paths = [];
            foreach (string path in ListBoxControl_SourcePaths.GetItems<string>(false) ?? [])
            {
                if (!paths_Selected_HashSet.Contains(path))
                {
                    paths.Add(path);
                }
            }

            ListBoxControl_SourcePaths.SetItems(paths);
        }

        /// <summary>
        /// Reads the controls into a new copy of the window's options, telling the operator what cannot be read.
        /// </summary>
        /// <param name="complete">True for OK: a scope is required. False for an export: an empty county selection or four blank coordinates are kept as no scope.</param>
        /// <returns>The options the controls describe, or null when a value could not be read; the reason has been shown.</returns>
        private PostgreSQLOpenBuildingImportTaskOptions? CreateOptions(bool complete)
        {
            PostgreSQLOpenBuildingImportTaskOptions options = new(postgreSQLOpenBuildingImportTaskOptions);

            List<string>? paths = ListBoxControl_SourcePaths.GetItems<string>(false);
            options.SourcePaths = paths is null || paths.Count == 0 ? null : [.. paths];

            HashSet<int>? countyIds = null;
            BoundingBox2D? boundingBox2D = null;

            if (RadioButton_BoundingBox.IsChecked == true)
            {
                bool blank = TrimOrNull(TextBoxControl_X1.Value) is null && TrimOrNull(TextBoxControl_Y1.Value) is null && TrimOrNull(TextBoxControl_X2.Value) is null && TrimOrNull(TextBoxControl_Y2.Value) is null;
                if (complete || !blank)
                {
                    if (!TextBoxControl_X1.TryGetValue(out double x1) || !TextBoxControl_Y1.TryGetValue(out double y1) || !TextBoxControl_X2.TryGetValue(out double x2) || !TextBoxControl_Y2.TryGetValue(out double y2))
                    {
                        MessageBox.Show(this, "The bounding box needs four coordinates: x1, y1, x2, y2.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return null;
                    }

                    boundingBox2D = new BoundingBox2D(new Point2D(x1, y1), new Point2D(x2, y2));

                    if (boundingBox2D.Min.X == boundingBox2D.Max.X || boundingBox2D.Min.Y == boundingBox2D.Max.Y)
                    {
                        // A zero-area box resolves to no county part, which would look like a finished run that
                        // imported nothing rather than like the mistake it is.
                        MessageBox.Show(this, "The bounding box has no area - x1 differs from x2, and y1 from y2.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return null;
                    }
                }
            }
            else
            {
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
                    MessageBox.Show(this, "Select the county parts the run covers, or switch to the bounding box scope.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                }
            }

            string? downloadDate_Text = TrimOrNull(TextBoxControl_DownloadDate.Value);
            DateTime? downloadDate = null;
            if (downloadDate_Text is not null)
            {
                if (!DateTime.TryParse(downloadDate_Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime downloadDate_Temp)
                    && !DateTime.TryParse(downloadDate_Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out downloadDate_Temp))
                {
                    MessageBox.Show(this, "The download date has to be a date, e.g. 2026-11-06.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                }

                downloadDate = downloadDate_Temp;
            }

            // Zero is the default and means half the processors; a blank field keeps it.
            int maxDegreeOfParallelism = 0;
            string? maxDegreeOfParallelism_Text = TrimOrNull(TextBoxControl_MaxDegreeOfParallelism.Value);
            if (maxDegreeOfParallelism_Text is not null && (!int.TryParse(maxDegreeOfParallelism_Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out maxDegreeOfParallelism) || maxDegreeOfParallelism < 0))
            {
                MessageBox.Show(this, "The max degree of parallelism has to be a whole number, 0 or more (0 uses half the processors).", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            options.Version = TrimOrNull(TextBoxControl_Version.Value);
            options.DownloadDate = downloadDate;
            options.Licence = TrimOrNull(TextBoxControl_Licence.Value);
            options.SourceUrl = TrimOrNull(TextBoxControl_SourceUrl.Value);
            options.SourceObservationDate = TrimOrNull(TextBoxControl_SourceObservationDate.Value);
            options.FileName = TrimOrNull(TextBoxControl_FileName.Value);
            options.Quadkey = TrimOrNull(TextBoxControl_Quadkey.Value);
            options.SkipSourcesOutOfScope = CheckBox_SkipSourcesOutOfScope.IsChecked == true;
            options.MaxDegreeOfParallelism = maxDegreeOfParallelism;
            options.CountyIds = countyIds;
            options.BoundingBox2D = boundingBox2D;

            return options;
        }

        /// <summary>
        /// Maps a dataset key to the caption shown for it on the window (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#38).
        /// </summary>
        /// <param name="datasetKey">The dataset key the row fixed, or null.</param>
        /// <returns>The display name, including the key itself so the two cannot be read apart.</returns>
        private static string DisplayName(string? datasetKey)
        {
            if (string.IsNullOrWhiteSpace(datasetKey))
            {
                return "(no dataset - this window was opened without one)";
            }

            string key = datasetKey.Trim();

            string name = key.ToUpperInvariant() switch
            {
                "EUBUCCO" => "EUBUCCO",
                "OVERTURE" => "Overture Buildings",
                "OSM" => "OpenStreetMap",
                "DBSM" => "DBSM",
                "MICROSOFT" => "Microsoft Global ML",
                "GHS_OBAT" => "GHS-OBAT",
                _ => "Unknown dataset"
            };

            return $"{name} ({key})";
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

        private void Scope_Changed(object sender, RoutedEventArgs e)
        {
            SetScopeControls();
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave out clears its control rather than keeping what was typed before.
        /// </summary>
        /// <param name="options">The options to show.</param>
        private void SetControls(PostgreSQLOpenBuildingImportTaskOptions options)
        {
            List<string> paths = options.SourcePaths is HashSet<string> sourcePaths ? [.. sourcePaths] : [];
            paths.Sort(StringComparer.Ordinal);
            ListBoxControl_SourcePaths.SetItems(paths);

            TextBoxControl_Version.Value = options.Version ?? string.Empty;
            TextBoxControl_DownloadDate.Value = options.DownloadDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty;
            TextBoxControl_Licence.Value = options.Licence ?? string.Empty;
            TextBoxControl_SourceUrl.Value = options.SourceUrl ?? string.Empty;
            TextBoxControl_SourceObservationDate.Value = options.SourceObservationDate ?? string.Empty;
            TextBoxControl_FileName.Value = options.FileName ?? string.Empty;
            TextBoxControl_Quadkey.Value = options.Quadkey ?? string.Empty;
            CheckBox_SkipSourcesOutOfScope.IsChecked = options.SkipSourcesOutOfScope;
            TextBoxControl_MaxDegreeOfParallelism.Value = options.MaxDegreeOfParallelism.ToString(CultureInfo.InvariantCulture);

            // The scope radios carry no IsChecked in the markup: setting it there raises Checked while the
            // window is still being parsed, before the panels the handler switches exist. The state is set
            // here instead, once every control is in place.
            if (options.BoundingBox2D is BoundingBox2D boundingBox2D)
            {
                RadioButton_BoundingBox.IsChecked = true;

                TextBoxControl_X1.Value = boundingBox2D.Min.X.ToString(CultureInfo.InvariantCulture);
                TextBoxControl_Y1.Value = boundingBox2D.Min.Y.ToString(CultureInfo.InvariantCulture);
                TextBoxControl_X2.Value = boundingBox2D.Max.X.ToString(CultureInfo.InvariantCulture);
                TextBoxControl_Y2.Value = boundingBox2D.Max.Y.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                RadioButton_CountyParts.IsChecked = true;

                TextBoxControl_X1.Value = string.Empty;
                TextBoxControl_Y1.Value = string.Empty;
                TextBoxControl_X2.Value = string.Empty;
                TextBoxControl_Y2.Value = string.Empty;
            }

            // Matched on the identifier rather than on the references themselves: the options carry
            // identifiers, and the references are read afresh from the database on every run. Nothing is
            // preselected when the options name no county: the scope is required, and an empty selection
            // is what OK refuses.
            HashSet<int> countyIds = options.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));

            SetScopeControls();
        }

        /// <summary>
        /// Shows the scope form that is checked and hides the other one, so the window never offers a county selection and a box to fill at the same time.
        /// </summary>
        private void SetScopeControls()
        {
            bool county = RadioButton_CountyParts.IsChecked == true;

            Grid_CountyScope.Visibility = county ? Visibility.Visible : Visibility.Collapsed;
            StackPanel_BoundingBox.Visibility = county ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Fills the county list, sorted by code and then by identifier. The selection is set by <see cref="SetControls(PostgreSQLOpenBuildingImportTaskOptions)"/>.
        /// </summary>
        /// <param name="administrativeAreal2DReferences">The county parts to list.</param>
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
        }

        /// <summary>
        /// Reads a control's value as trimmed text, mapping blank to null so an empty optional field is stored as absent rather than as whitespace.
        /// </summary>
        /// <param name="value">The raw control value.</param>
        /// <returns>The trimmed text, or null when there was none.</returns>
        private static string? TrimOrNull(string? value)
        {
            string? trimmed = value?.Trim();

            return string.IsNullOrEmpty(trimmed) ? null : trimmed;
        }
    }
}
