using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Solar.Classes;
using DiGi.UI.WPF.Classes;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLBuildingSolarRadiationSolveOptionsWindow.xaml
    /// <para>Asks for everything that decides what a batch solar radiation solve costs and stores: the county parts, the neighbour radii every building is solved for, whether results already stored are stepped over, and the batch, tile, ceiling and timeout settings (DiGi.GIS.PostgreSQL#136).</para>
    /// <para>An optional limit of buildings per county part runs a pilot: empty solves every building (DiGi.GIS.PostgreSQL.UI#45).</para>
    /// <para>The number of solves run at once: 0 is automatic, half the logical processors; 1 is one at a time. It changes the run time only, never the stored results (DiGi.GIS.PostgreSQL.UI#46, DiGi.GIS.PostgreSQL#139).</para>
    /// <para>The counties matter most. A solve costs seconds per building and radius, so an unscoped run is the whole country several times over - the window never leaves with no county chosen.</para>
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

            // The captions stay in the XAML - the designer does not run this constructor. Only the values come from
            // the options.
            CheckBox_SkipExisting.IsChecked = this.postgreSQLBuildingSolarRadiationSolveOptions.SkipExisting;
            TextBoxControl_BatchSize.Value = this.postgreSQLBuildingSolarRadiationSolveOptions.BatchSize.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_TileSize.Value = this.postgreSQLBuildingSolarRadiationSolveOptions.TileSize.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_ReceiverCountMax.Value = this.postgreSQLBuildingSolarRadiationSolveOptions.ReceiverCountMax.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_CasterTriangleCountMax.Value = this.postgreSQLBuildingSolarRadiationSolveOptions.CasterTriangleCountMax.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_CommandTimeout.Value = this.postgreSQLBuildingSolarRadiationSolveOptions.CommandTimeout.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_MaxDegreeOfParallelism.Value = this.postgreSQLBuildingSolarRadiationSolveOptions.MaxDegreeOfParallelism.ToString(CultureInfo.CurrentCulture);

            // Empty is "every building", not zero - the option is null then.
            int? buildingCountMax = this.postgreSQLBuildingSolarRadiationSolveOptions.BuildingCountMax;
            TextBoxControl_BuildingCountMax.Value = buildingCountMax is int buildingCountMax_Value ? buildingCountMax_Value.ToString(CultureInfo.CurrentCulture) : string.Empty;

            // Subscribed before the lists are filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;
            ListBoxControl_Radii.ItemAdding += ListBoxControl_Radii_ItemAdding;

            foreach (double radius in this.postgreSQLBuildingSolarRadiationSolveOptions.Radii ?? [])
            {
                if (double.IsFinite(radius) && radius >= 0 && !radii.Contains(radius))
                {
                    radii.Add(radius);
                }
            }

            SetRadii();
            SetCounties(administrativeAreal2DReferences);
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

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                // The task takes null as "every county" and an empty set as "none", so neither is a safe thing to
                // leave this window with by accident.
                MessageBox.Show("At least one county has to be selected.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (radii.Count == 0)
            {
                MessageBox.Show("At least one radius has to be given.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_BatchSize.TryGetValue(out int batchSize) || batchSize < 1)
            {
                MessageBox.Show("Batch size has to be a whole number, 1 or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_TileSize.TryGetValue(out double tileSize) || !double.IsFinite(tileSize) || tileSize < 1)
            {
                MessageBox.Show("Tile size has to be a number of metres, 1 or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_ReceiverCountMax.TryGetValue(out int receiverCountMax) || receiverCountMax < 1)
            {
                MessageBox.Show("The receiving surface ceiling has to be a whole number, 1 or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_CasterTriangleCountMax.TryGetValue(out int casterTriangleCountMax) || casterTriangleCountMax < 1)
            {
                MessageBox.Show("The shading triangle ceiling has to be a whole number, 1 or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_CommandTimeout.TryGetValue(out int commandTimeout) || commandTimeout < 0)
            {
                MessageBox.Show("Command timeout has to be a whole number of seconds, zero or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_MaxDegreeOfParallelism.TryGetValue(out int maxDegreeOfParallelism) || maxDegreeOfParallelism < 0)
            {
                MessageBox.Show("Parallel solves has to be a whole number, zero (automatic) or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int? buildingCountMax = null;
            if (!string.IsNullOrWhiteSpace(TextBoxControl_BuildingCountMax.Value))
            {
                if (!TextBoxControl_BuildingCountMax.TryGetValue(out int buildingCountMax_Value) || buildingCountMax_Value < 1)
                {
                    MessageBox.Show("Buildings per part has to be empty or a whole number, 1 or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                buildingCountMax = buildingCountMax_Value;
            }

            HashSet<int> countyIds = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                countyIds.Add(administrativeAreal2DReference.Id);
            }

            postgreSQLBuildingSolarRadiationSolveOptions.CountyIds = countyIds;
            postgreSQLBuildingSolarRadiationSolveOptions.Radii = [.. radii];
            postgreSQLBuildingSolarRadiationSolveOptions.SkipExisting = CheckBox_SkipExisting.IsChecked == true;
            postgreSQLBuildingSolarRadiationSolveOptions.BatchSize = batchSize;
            postgreSQLBuildingSolarRadiationSolveOptions.TileSize = tileSize;
            postgreSQLBuildingSolarRadiationSolveOptions.ReceiverCountMax = receiverCountMax;
            postgreSQLBuildingSolarRadiationSolveOptions.CasterTriangleCountMax = casterTriangleCountMax;
            postgreSQLBuildingSolarRadiationSolveOptions.CommandTimeout = commandTimeout;
            postgreSQLBuildingSolarRadiationSolveOptions.BuildingCountMax = buildingCountMax;
            postgreSQLBuildingSolarRadiationSolveOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism;

            DialogResult = true;
            Close();
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

            HashSet<int>? countyIds = postgreSQLBuildingSolarRadiationSolveOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            // Matched on the identifier: the options carry identifiers, and the references are read afresh on every
            // run.
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        private void SetRadii()
        {
            radii.Sort();

            ListBoxControl_Radii.SetItems(radii);
        }
    }
}
