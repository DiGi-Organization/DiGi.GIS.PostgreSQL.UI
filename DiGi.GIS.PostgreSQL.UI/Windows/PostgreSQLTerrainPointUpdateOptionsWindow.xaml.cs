using DiGi.GIS.PostgreSQL.Classes;
using DiGi.UI.WPF.Classes;
using System.Collections.Generic;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLTerrainPointUpdateOptionsWindow.xaml
    /// <para>Asks for the three settings that decide what a terrain point run costs and covers - the spacing of the sampling grid, whether points already stored are sampled again, and the counties to sample. Every other option of the instance it was given is carried over untouched, which is deliberate: the origin of the grid and the tile size are what let separate runs share their points, and they are not settings to change from a dialog.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLTerrainPointUpdateOptionsWindow : Window
    {
        private readonly PostgreSQLTerrainPointUpdateOptions postgreSQLTerrainPointUpdateOptions;
        private readonly Dictionary<int, TerrainPointDensityResult>? terrainPointDensityResults_ByCountyId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLTerrainPointUpdateOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLTerrainPointUpdateOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        /// <param name="terrainPointDensityResults">The density measurements of the county partitions. When provided, point count, density and equivalent spacing are shown for each county.</param>
        public PostgreSQLTerrainPointUpdateOptionsWindow(PostgreSQLTerrainPointUpdateOptions? postgreSQLTerrainPointUpdateOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences, IEnumerable<TerrainPointDensityResult>? terrainPointDensityResults = null)
        {
            InitializeComponent();

            this.postgreSQLTerrainPointUpdateOptions = postgreSQLTerrainPointUpdateOptions is null ? new PostgreSQLTerrainPointUpdateOptions() : new PostgreSQLTerrainPointUpdateOptions(postgreSQLTerrainPointUpdateOptions);

            if (terrainPointDensityResults is not null)
            {
                terrainPointDensityResults_ByCountyId = [];
                foreach (TerrainPointDensityResult terrainPointDensityResult in terrainPointDensityResults)
                {
                    if (terrainPointDensityResult is not null)
                    {
                        terrainPointDensityResults_ByCountyId[terrainPointDensityResult.CountyId] = terrainPointDensityResult;
                    }
                }
            }

            // The caption stays in the XAML - the designer does not run this constructor, and a caption set here
            // leaves an empty label over an empty box in the preview. Only the values come from the options.
            TextBoxControl_GridSize.Value = this.postgreSQLTerrainPointUpdateOptions.GridSize.ToString();

            CheckBox_OverrideExisting.IsChecked = this.postgreSQLTerrainPointUpdateOptions.OverrideExisting;

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLTerrainPointUpdateOptions PostgreSQLTerrainPointUpdateOptions
        {
            get
            {
                return postgreSQLTerrainPointUpdateOptions;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            // The task itself rejects a grid size that is not a positive finite number by returning false and
            // saying nothing, hours of work later at the earliest. Refusing it here is the only place the
            // reason can be given.
            if (!TextBoxControl_GridSize.TryGetValue(out double gridSize) || double.IsNaN(gridSize) || double.IsInfinity(gridSize) || gridSize <= 0)
            {
                MessageBox.Show("Grid size has to be a number greater than zero.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                // An empty set is not the same as no set: the task takes it as "nothing to sample" and reports
                // success having done nothing, so it must never leave this window.
                MessageBox.Show("At least one county has to be selected.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            HashSet<int> countyIds = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                countyIds.Add(administrativeAreal2DReference.Id);
            }

            postgreSQLTerrainPointUpdateOptions.GridSize = gridSize;
            postgreSQLTerrainPointUpdateOptions.OverrideExisting = CheckBox_OverrideExisting.IsChecked == true;
            postgreSQLTerrainPointUpdateOptions.CountyIds = countyIds;

            DialogResult = true;
            Close();
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

            if (terrainPointDensityResults_ByCountyId is not null && terrainPointDensityResults_ByCountyId.TryGetValue(administrativeAreal2DReference.Id, out TerrainPointDensityResult? terrainPointDensityResult) && terrainPointDensityResult is not null)
            {
                if (terrainPointDensityResult.Count > 0)
                {
                    string densityText = terrainPointDensityResult.Density.HasValue ? $"{terrainPointDensityResult.Density.Value.ToString("0.#####", System.Globalization.CultureInfo.InvariantCulture)} pts/m²" : "0 pts/m²";
                    string spacingText = terrainPointDensityResult.SpacingEquivalent.HasValue ? $", ~{terrainPointDensityResult.SpacingEquivalent.Value.ToString("0.#", System.Globalization.CultureInfo.InvariantCulture)} m" : string.Empty;

                    name = $"{name} - {terrainPointDensityResult.Count.ToString("N0", System.Globalization.CultureInfo.InvariantCulture)} pts ({densityText}{spacingText})";
                }
                else
                {
                    name = $"{name} - 0 pts (0 pts/m²)";
                }
            }

            e.Name = name;
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

            HashSet<int>? countyIds = postgreSQLTerrainPointUpdateOptions.CountyIds;
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
