using DiGi.GIS.PostgreSQL.Classes;
using DiGi.Geometry.Planar.Classes;
using DiGi.UI.WPF;
using DiGi.UI.WPF.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for OpenBuildingImportOptionsWindow.xaml
    /// <para>Asks what decides a run of the open building import (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#38): which source files the release came in, whether the scope is a county list or a bounding box, and the release metadata the register stores. The dataset itself is not asked - the row the operator started fixes it, so a row named for one release cannot import another.</para>
    /// <para>The SHA-256 is never typed. The field is shown disabled with the caption saying when it is computed: the wrapper hashes the source files after this dialog closes and before the run starts.</para>
    /// <para>Exactly one scope form has to be filled - a county selection or the four coordinates of an EPSG:2180 box - and OK still refuses when either is missing or both are given, because both is ambiguous and neither has no run. What the core task enforces is asked here first through <see cref="Query.OpenBuildingImportOptionsMessage(PostgreSQLOpenBuildingImportTaskOptions)"/>, so the operator is told while the dialog is open rather than from a failed run's log.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class OpenBuildingImportOptionsWindow : Window
    {
        private readonly PostgreSQLOpenBuildingImportTaskOptions postgreSQLOpenBuildingImportTaskOptions;

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

            if (this.postgreSQLOpenBuildingImportTaskOptions.SourcePaths is HashSet<string> sourcePaths)
            {
                List<string> paths = [.. sourcePaths];
                paths.Sort(StringComparer.Ordinal);
                ListBoxControl_SourcePaths.SetItems(paths);
            }

            TextBoxControl_Version.Value = this.postgreSQLOpenBuildingImportTaskOptions.Version ?? string.Empty;
            TextBoxControl_DownloadDate.Value = this.postgreSQLOpenBuildingImportTaskOptions.DownloadDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty;
            TextBoxControl_Licence.Value = this.postgreSQLOpenBuildingImportTaskOptions.Licence ?? string.Empty;
            TextBoxControl_SourceUrl.Value = this.postgreSQLOpenBuildingImportTaskOptions.SourceUrl ?? string.Empty;
            TextBoxControl_SourceObservationDate.Value = this.postgreSQLOpenBuildingImportTaskOptions.SourceObservationDate ?? string.Empty;
            TextBoxControl_FileName.Value = this.postgreSQLOpenBuildingImportTaskOptions.FileName ?? string.Empty;
            TextBoxControl_Quadkey.Value = this.postgreSQLOpenBuildingImportTaskOptions.Quadkey ?? string.Empty;

            // The scope radios carry no IsChecked in the markup: setting it there raises Checked while the
            // window is still being parsed, before the panels the handler switches exist. The state is set
            // here instead, once every control is in place.
            if (this.postgreSQLOpenBuildingImportTaskOptions.BoundingBox2D is BoundingBox2D boundingBox2D)
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
            }

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);

            SetScopeControls();
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
            (string fileTypeName, string fileExtension) = FileFilter(postgreSQLOpenBuildingImportTaskOptions.DatasetKey);

            string[]? paths_Added = this.Paths(fileTypeName, fileExtension);
            if (paths_Added is null || paths_Added.Length == 0)
            {
                return;
            }

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

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            PostgreSQLOpenBuildingImportTaskOptions options = postgreSQLOpenBuildingImportTaskOptions;

            List<string>? paths = ListBoxControl_SourcePaths.GetItems<string>(false);
            options.SourcePaths = paths is null || paths.Count == 0 ? null : [.. paths];

            HashSet<int>? countyIds = null;
            BoundingBox2D? boundingBox2D = null;

            if (RadioButton_BoundingBox.IsChecked == true)
            {
                if (!TextBoxControl_X1.TryGetValue(out double x1) || !TextBoxControl_Y1.TryGetValue(out double y1) || !TextBoxControl_X2.TryGetValue(out double x2) || !TextBoxControl_Y2.TryGetValue(out double y2))
                {
                    MessageBox.Show("The bounding box needs four coordinates: x1, y1, x2, y2.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                boundingBox2D = new BoundingBox2D(new Point2D(x1, y1), new Point2D(x2, y2));

                if (boundingBox2D.Min.X == boundingBox2D.Max.X || boundingBox2D.Min.Y == boundingBox2D.Max.Y)
                {
                    // A zero-area box resolves to no county part, which would look like a finished run that
                    // imported nothing rather than like the mistake it is.
                    MessageBox.Show("The bounding box has no area - x1 differs from x2, and y1 from y2.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();
                if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
                {
                    MessageBox.Show("Select the county parts the run covers, or switch to the bounding box scope.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                countyIds = [];
                foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
                {
                    countyIds.Add(administrativeAreal2DReference.Id);
                }
            }

            string? downloadDate_Text = TrimOrNull(TextBoxControl_DownloadDate.Value);
            DateTime? downloadDate = null;
            if (downloadDate_Text is not null)
            {
                if (!DateTime.TryParse(downloadDate_Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime downloadDate_Temp)
                    && !DateTime.TryParse(downloadDate_Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out downloadDate_Temp))
                {
                    MessageBox.Show("The download date has to be a date, e.g. 2026-11-06.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                downloadDate = downloadDate_Temp;
            }

            options.Version = TrimOrNull(TextBoxControl_Version.Value);
            options.DownloadDate = downloadDate;
            options.Licence = TrimOrNull(TextBoxControl_Licence.Value);
            options.SourceUrl = TrimOrNull(TextBoxControl_SourceUrl.Value);
            options.SourceObservationDate = TrimOrNull(TextBoxControl_SourceObservationDate.Value);
            options.FileName = TrimOrNull(TextBoxControl_FileName.Value);
            options.Quadkey = TrimOrNull(TextBoxControl_Quadkey.Value);
            options.CountyIds = countyIds;
            options.BoundingBox2D = boundingBox2D;

            // The last gate before OK returns: the same rules the core task enforces, asked while the
            // dialog can still be corrected instead of after a run has been started and refused.
            if (Query.OpenBuildingImportOptionsMessage(options) is string message)
            {
                MessageBox.Show(message, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
        /// Maps a dataset key to the file type the add-files dialog is filtered by (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#38).
        /// <para>The dialog always offers All Files as well, so the datasets released as two formats - GHS-OBAT comes as CSV with GeoPackage centroids - can still be picked in one pass.</para>
        /// </summary>
        /// <param name="datasetKey">The dataset key the row fixed, or null.</param>
        /// <returns>The dialog's file type name and extension filter.</returns>
        private static (string FileTypeName, string FileExtension) FileFilter(string? datasetKey)
        {
            return datasetKey?.Trim().ToUpperInvariant() switch
            {
                "EUBUCCO" => ("EUBUCCO GeoPackage", "gpkg"),
                "OVERTURE" => ("Overture GeoParquet", "parquet"),
                "OSM" => ("OpenStreetMap PBF", "pbf"),
                "DBSM" => ("DBSM GeoPackage", "gpkg"),
                "MICROSOFT" => ("Microsoft GeoJSON sequence", "geojsonl"),
                "GHS_OBAT" => ("GHS-OBAT CSV", "csv"),
                _ => ("Source file", "*")
            };
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
        /// Shows the scope form that is checked and hides the other one, so the window never offers a county selection and a box to fill at the same time.
        /// </summary>
        private void SetScopeControls()
        {
            bool county = RadioButton_CountyParts.IsChecked == true;

            Grid_CountyScope.Visibility = county ? Visibility.Visible : Visibility.Collapsed;
            StackPanel_BoundingBox.Visibility = county ? Visibility.Collapsed : Visibility.Visible;
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

            HashSet<int>? countyIds = postgreSQLOpenBuildingImportTaskOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                // Nothing preselected: the scope is required, and an empty selection is what OK refuses.
                return;
            }

            // Matched on the identifier rather than on the references themselves: the options carry
            // identifiers, and the references are read afresh from the database on every run.
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
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
