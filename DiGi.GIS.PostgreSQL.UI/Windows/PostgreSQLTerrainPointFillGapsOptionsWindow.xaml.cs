using DiGi.GIS.PostgreSQL.Classes;
using DiGi.UI.WPF.Classes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLTerrainPointFillGapsOptionsWindow.xaml
    /// <para>Asks for the two settings that decide what a repair covers - the spacing the counties were sampled at, and the counties to measure. Every other option of the instance it was given is carried over untouched: the origin of the lattice and the tile size are what let this agree with the run it is repairing, and they are not settings to change from a dialog.</para>
    /// <para>The spacing is the one that has to be right. Set finer than a county actually holds, every node in between reads as a gap and the repair turns into a densification of the whole country, so the equivalent spacing measured for each county is shown beside it.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLTerrainPointFillGapsOptionsWindow : Window
    {
        private readonly PostgreSQLTerrainPointFillGapsOptions postgreSQLTerrainPointFillGapsOptions;
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private Dictionary<int, TerrainPointDensityResult>? terrainPointDensityResults_ByCountyId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLTerrainPointFillGapsOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLTerrainPointFillGapsOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        /// <param name="terrainPointDensityResultsAsync">Reads the density measurements of the county partitions, keyed by part id, for the point count, density and equivalent spacing shown beside each county. Started here and never awaited before the window opens: the county list is shown at once, with a progress bar in the bottom-left corner, and the figures are filled in when the read completes (DiGi.GIS.PostgreSQL.UI#41). Closing the window cancels it. When null nothing is read, no county is annotated and the progress bar is never shown.</param>
        public PostgreSQLTerrainPointFillGapsOptionsWindow(PostgreSQLTerrainPointFillGapsOptions? postgreSQLTerrainPointFillGapsOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences, Func<CancellationToken, Task<Dictionary<int, TerrainPointDensityResult>?>>? terrainPointDensityResultsAsync = null)
        {
            InitializeComponent();

            this.postgreSQLTerrainPointFillGapsOptions = postgreSQLTerrainPointFillGapsOptions is null ? new PostgreSQLTerrainPointFillGapsOptions() : new PostgreSQLTerrainPointFillGapsOptions(postgreSQLTerrainPointFillGapsOptions);

            // The caption stays in the XAML - the designer does not run this constructor, and a caption set here
            // leaves an empty label over an empty box in the preview. Only the values come from the options.
            TextBoxControl_GridSize.Value = this.postgreSQLTerrainPointFillGapsOptions.GridSize.ToString();

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);

            // A read still running when the window closes is no longer wanted by anyone; cancelling it also stops the
            // queries behind it on the server.
            // Every way out raises Closed - OK included, because once the counties are chosen nobody needs the densities.
            // Disposed straight after: the read took its token when it started, and Cancel has already reached it.
            Closed += (sender, e) =>
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            };

            if (terrainPointDensityResultsAsync is not null)
            {
                Label_Density.Content = "Reading terrain point densities...";
                ProgressBar_Density.Visibility = Visibility.Visible;
                _ = ReadTerrainPointDensityResultsAsync(terrainPointDensityResultsAsync);
            }
        }

        /// <summary>
        /// Sets the terrain point densities shown beside the counties, and redraws the county list in place.
        /// <para>The selection and the scroll position are kept: the densities normally arrive after the window is open, possibly while counties are being picked. Null means the densities could not be read, which annotates nothing and says so - a bare county is never a measured one. Either way the read is settled, so the progress bar is hidden.</para>
        /// </summary>
        /// <param name="terrainPointDensityResults">The densities keyed by county part id, or null when they could not be read.</param>
        public void SetTerrainPointDensityResults(IReadOnlyDictionary<int, TerrainPointDensityResult>? terrainPointDensityResults)
        {
            terrainPointDensityResults_ByCountyId = terrainPointDensityResults is null ? null : new Dictionary<int, TerrainPointDensityResult>(terrainPointDensityResults);

            ProgressBar_Density.Visibility = Visibility.Collapsed;

            Label_Density.Content = terrainPointDensityResults is null ? "Terrain point densities could not be read - no county is annotated" : "Point count, density and equivalent spacing stored for each county";

            ListBoxControl_Counties.RefreshItems();
        }

        private async Task ReadTerrainPointDensityResultsAsync(Func<CancellationToken, Task<Dictionary<int, TerrainPointDensityResult>?>> terrainPointDensityResultsAsync)
        {
            try
            {
                Dictionary<int, TerrainPointDensityResult>? terrainPointDensityResults;
                try
                {
                    // No ConfigureAwait(false): the continuation touches the controls, so it has to resume on the user interface thread.
                    terrainPointDensityResults = await terrainPointDensityResultsAsync(cancellationTokenSource.Token);
                }
                catch (OperationCanceledException) when (cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception exception)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Terrain point densities could not be read: {Message}", exception.Message);
                    terrainPointDensityResults = null;
                }

                // A window closed meanwhile is left alone.
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }

                SetTerrainPointDensityResults(terrainPointDensityResults);
            }
            finally
            {
                // However the read ended - figures, a failure, or a cancellation that returns before the set - nothing is being read any more.
                ProgressBar_Density.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLTerrainPointFillGapsOptions PostgreSQLTerrainPointFillGapsOptions
        {
            get
            {
                return postgreSQLTerrainPointFillGapsOptions;
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
            // saying nothing. Refusing it here is the only place the reason can be given.
            if (!TextBoxControl_GridSize.TryGetValue(out double gridSize) || double.IsNaN(gridSize) || double.IsInfinity(gridSize) || gridSize <= 0)
            {
                MessageBox.Show("Grid size has to be a number greater than zero.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                // An empty set is not the same as no set: the task takes it as "nothing to repair" and reports
                // success having done nothing, so it must never leave this window.
                MessageBox.Show("At least one county has to be selected.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            HashSet<int> countyIds = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                countyIds.Add(administrativeAreal2DReference.Id);
            }

            postgreSQLTerrainPointFillGapsOptions.GridSize = gridSize;
            postgreSQLTerrainPointFillGapsOptions.CountyIds = countyIds;

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

            HashSet<int>? countyIds = postgreSQLTerrainPointFillGapsOptions.CountyIds;
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
