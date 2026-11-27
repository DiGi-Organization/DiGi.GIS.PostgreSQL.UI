using DiGi.GIS.PostgreSQL.Classes;
using DiGi.UI.WPF.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLTerrainPointFillGapsOptionsWindow.xaml
    /// <para>Asks for the two settings that decide what a repair covers - the spacing the counties were sampled at, and the counties to measure. Every other option of the instance it was given is carried over untouched: the origin of the lattice and the tile size are what let this agree with the run it is repairing, and they are not settings to change from a dialog.</para>
    /// <para>The spacing is the one that has to be right. Set finer than a county actually holds, every node in between reads as a gap and the repair turns into a densification of the whole country, so the equivalent spacing measured for each county is shown beside it.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#64), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "The Closed handler cancels and disposes the token source.")]
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

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
            SetControls(this.postgreSQLTerrainPointFillGapsOptions);

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

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: a half-prepared window can be saved and finished after the next import - a window
            // with no county selected writes no county list, and the grid size floor and the at-least-one-county
            // rule are OK's gate, not the file's. A box that does not hold a number is still refused, because it
            // could not be written back as typed.
            if (!TryReadOptions(false, out PostgreSQLTerrainPointFillGapsOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export terrain point gap settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "TerrainPoint_FillGaps_Settings.json"
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
                Title = "Import terrain point gap settings",
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

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLTerrainPointFillGapsOptions postgreSQLTerrainPointFillGapsOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Only the controls are filled - the members this window has no control for (the lattice origin,
            // the tile size) stay as the window was opened with them, exactly as a typed value would.
            SetControls(postgreSQLTerrainPointFillGapsOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected - the
            // usual sign the settings came from another machine.
            HashSet<int> countyIds = postgreSQLTerrainPointFillGapsOptions_Imported.CountyIds ?? [];
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
        /// <para>The options are written only when every control reads, the grid size is greater than zero and at least one county is selected; a refusal leaves them as they were. The members the window has no control for keep the values it was opened with.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out PostgreSQLTerrainPointFillGapsOptions? options, out message) || options is null)
            {
                return false;
            }

            postgreSQLTerrainPointFillGapsOptions.GridSize = options.GridSize;
            postgreSQLTerrainPointFillGapsOptions.CountyIds = options.CountyIds;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) requires a grid size greater than zero and at least one county, and finishes with the messages OK has always given. Export reads the same controls without those requirements - no county selected is written as no county list, never as an empty set - so a half-prepared window can be saved; what is still refused is what could not be written back as typed: a grid size box that does not hold a finite number. The candidate is built on a copy of the window's options, so the members with no control (the lattice origin, the tile size, the batch and retry settings) travel in the file exactly as the copy holds them and are never changed by a read.</para>
        /// </summary>
        /// <param name="complete">True for OK: the grid size range and a county are required. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out PostgreSQLTerrainPointFillGapsOptions? options, out string? message)
        {
            options = null;
            message = null;

            bool gridSize_Read = TextBoxControl_GridSize.TryGetValue(out double gridSize) && double.IsFinite(gridSize);

            if (!gridSize_Read || (complete && gridSize <= 0))
            {
                message = "Grid size has to be a number greater than zero.";
                return false;
            }

            // No selection is written as no county list (null); an empty set would mean "nothing to repair" and
            // is never handed over - neither by OK nor by an export.
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

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            PostgreSQLTerrainPointFillGapsOptions postgreSQLTerrainPointFillGapsOptions_Temp = new(postgreSQLTerrainPointFillGapsOptions)
            {
                GridSize = gridSize,
                CountyIds = countyIds,
            };

            options = postgreSQLTerrainPointFillGapsOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before: options with no county list clear the
        /// selection. The members this window has no control for are left alone. Internal so a fact can fill a
        /// window from a settings file the way Import does, without a file dialog.
        /// </summary>
        /// <param name="postgreSQLTerrainPointFillGapsOptions">The options to show.</param>
        internal void SetControls(PostgreSQLTerrainPointFillGapsOptions postgreSQLTerrainPointFillGapsOptions)
        {
            // The caption stays in the XAML - the designer does not run this constructor, and a caption set here
            // leaves an empty label over an empty box in the preview. Only the values come from the options.
            TextBoxControl_GridSize.Value = postgreSQLTerrainPointFillGapsOptions.GridSize.ToString(CultureInfo.CurrentCulture);

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run. SelectItems deselects the rest,
            // so a file with no county list clears whatever was selected before.
            HashSet<int> countyIds = postgreSQLTerrainPointFillGapsOptions.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one terrain point gap options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLTerrainPointFillGapsOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLTerrainPointFillGapsOptions>? postgreSQLTerrainPointFillGapsOptionsList = Core.Convert.ToDiGi<PostgreSQLTerrainPointFillGapsOptions>(json);
            if (postgreSQLTerrainPointFillGapsOptionsList is null || postgreSQLTerrainPointFillGapsOptionsList.Count != 1)
            {
                message = "The file holds no terrain point gap settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLTerrainPointFillGapsOptionsList[0];
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
