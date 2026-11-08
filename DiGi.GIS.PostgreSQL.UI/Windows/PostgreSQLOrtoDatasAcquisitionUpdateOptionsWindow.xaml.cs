using DiGi.GIS.PostgreSQL.Classes;
using DiGi.UI.WPF.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow.xaml
    /// <para>Asks for the county parts an acquisition-date backfill of <c>orto_datas</c> walks, the last year the stored series were fetched up to, and the paging of the walk. Unlike most county-scoped tasks, no selection means nothing rather than every county: the backfill reads every stored series it reaches, about 0.5 MB each, so the scope is always named.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow : Window
    {
        private readonly PostgreSQLOrtoDatasAcquisitionUpdateOptions postgreSQLOrtoDatasAcquisitionUpdateOptions;
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private Dictionary<int, OrtoDataAcquisitionCoverageResult>? ortoDataAcquisitionCoverageResults_ByCountyId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLOrtoDatasAcquisitionUpdateOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The county parts to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier.</param>
        /// <param name="ortoDataAcquisitionCoverageResultsAsync">Reads how far the backfill has reached in each county part, keyed by part id, for the series, the backlog and the dated images shown beside each part (DiGi.GIS.PostgreSQL#134). Started here and never awaited before the window opens: the county list is shown at once, with a progress bar in the bottom-left corner, and the figures are filled in when the read completes. Closing the window cancels it. When null nothing is read, no part is annotated and the progress bar is never shown.</param>
        public PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow(PostgreSQLOrtoDatasAcquisitionUpdateOptions? postgreSQLOrtoDatasAcquisitionUpdateOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences, Func<CancellationToken, Task<Dictionary<int, OrtoDataAcquisitionCoverageResult>?>>? ortoDataAcquisitionCoverageResultsAsync = null)
        {
            InitializeComponent();

            this.postgreSQLOrtoDatasAcquisitionUpdateOptions = postgreSQLOrtoDatasAcquisitionUpdateOptions is null ? new PostgreSQLOrtoDatasAcquisitionUpdateOptions() : new PostgreSQLOrtoDatasAcquisitionUpdateOptions(postgreSQLOrtoDatasAcquisitionUpdateOptions);

            TextBoxControl_LastYear.Value = this.postgreSQLOrtoDatasAcquisitionUpdateOptions.LastYear.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_PageSize.Value = this.postgreSQLOrtoDatasAcquisitionUpdateOptions.PageSize.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_CommandTimeout.Value = this.postgreSQLOrtoDatasAcquisitionUpdateOptions.CommandTimeout.ToString(CultureInfo.InvariantCulture);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);

            // Every way out raises Closed - OK included, because once the parts are chosen nobody needs the coverage.
            // Disposed straight after: the read took its token when it started, and Cancel has already reached it.
            Closed += (sender, e) =>
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            };

            if (ortoDataAcquisitionCoverageResultsAsync is not null)
            {
                Label_Coverage.Content = "Reading acquisition-date coverage...";
                ProgressBar_Coverage.Visibility = Visibility.Visible;
                _ = ReadOrtoDataAcquisitionCoverageResultsAsync(ortoDataAcquisitionCoverageResultsAsync);
            }
        }

        /// <summary>
        /// Sets the acquisition-date coverage shown beside the county parts, and redraws the county list in place.
        /// <para>The selection and the scroll position are kept: the coverage normally arrives after the window is open, possibly while parts are being picked. Null means the coverage could not be read, which annotates nothing and says so - a bare part is never a measured one. A part absent from a dictionary that was read holds no OrtoDatas series, and says so. Either way the read is settled, so the progress bar is hidden.</para>
        /// </summary>
        /// <param name="ortoDataAcquisitionCoverageResults">The coverage keyed by county part id, or null when it could not be read.</param>
        public void SetOrtoDataAcquisitionCoverageResults(IReadOnlyDictionary<int, OrtoDataAcquisitionCoverageResult>? ortoDataAcquisitionCoverageResults)
        {
            ortoDataAcquisitionCoverageResults_ByCountyId = ortoDataAcquisitionCoverageResults is null ? null : new Dictionary<int, OrtoDataAcquisitionCoverageResult>(ortoDataAcquisitionCoverageResults);

            ProgressBar_Coverage.Visibility = Visibility.Collapsed;

            Label_Coverage.Content = ortoDataAcquisitionCoverageResults is null ? "Acquisition-date coverage could not be read - no county part is annotated" : "OrtoDatas series per part, how many are not backfilled yet, and how many of their images are dated";

            ListBoxControl_Counties.RefreshItems();
        }

        private async Task ReadOrtoDataAcquisitionCoverageResultsAsync(Func<CancellationToken, Task<Dictionary<int, OrtoDataAcquisitionCoverageResult>?>> ortoDataAcquisitionCoverageResultsAsync)
        {
            try
            {
                Dictionary<int, OrtoDataAcquisitionCoverageResult>? ortoDataAcquisitionCoverageResults;
                try
                {
                    // No ConfigureAwait(false): the continuation touches the controls, so it has to resume on the user interface thread.
                    ortoDataAcquisitionCoverageResults = await ortoDataAcquisitionCoverageResultsAsync(cancellationTokenSource.Token);
                }
                catch (OperationCanceledException) when (cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception exception)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OrtoDatas acquisition coverage could not be read: {Message}", exception.Message);
                    ortoDataAcquisitionCoverageResults = null;
                }

                // A window closed meanwhile is left alone.
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }

                SetOrtoDataAcquisitionCoverageResults(ortoDataAcquisitionCoverageResults);
            }
            finally
            {
                // However the read ended - figures, a failure, or a cancellation that returns before the set - nothing is being read any more.
                ProgressBar_Coverage.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLOrtoDatasAcquisitionUpdateOptions PostgreSQLOrtoDatasAcquisitionUpdateOptions
        {
            get
            {
                return postgreSQLOrtoDatasAcquisitionUpdateOptions;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (!TextBoxControl_LastYear.TryGetValue(out int lastYear) || lastYear < 1900)
            {
                MessageBox.Show("The last year has to be a whole number from 1900 on.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_PageSize.TryGetValue(out int pageSize) || pageSize < 1)
            {
                MessageBox.Show("Page size has to be a whole number of rows, one or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_CommandTimeout.TryGetValue(out int commandTimeout) || commandTimeout < 0)
            {
                MessageBox.Show("Command timeout has to be a whole number of seconds, zero (no timeout) or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                MessageBox.Show("Select at least one county part.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            HashSet<int> countyIds = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                countyIds.Add(administrativeAreal2DReference.Id);
            }

            postgreSQLOrtoDatasAcquisitionUpdateOptions.LastYear = lastYear;
            postgreSQLOrtoDatasAcquisitionUpdateOptions.PageSize = pageSize;
            postgreSQLOrtoDatasAcquisitionUpdateOptions.CommandTimeout = commandTimeout;
            postgreSQLOrtoDatasAcquisitionUpdateOptions.CountyIds = countyIds;

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

            // Unread or unreadable coverage leaves the part bare; the status label says which.
            if (ortoDataAcquisitionCoverageResults_ByCountyId is not null)
            {
                if (ortoDataAcquisitionCoverageResults_ByCountyId.TryGetValue(administrativeAreal2DReference.Id, out OrtoDataAcquisitionCoverageResult? ortoDataAcquisitionCoverageResult) && ortoDataAcquisitionCoverageResult is not null && ortoDataAcquisitionCoverageResult.SeriesCount > 0)
                {
                    long datedCount = ortoDataAcquisitionCoverageResult.ImageCount - ortoDataAcquisitionCoverageResult.UndefinedCount;

                    name = $"{name} - {ortoDataAcquisitionCoverageResult.SeriesCount.ToString("N0", CultureInfo.InvariantCulture)} series, {ortoDataAcquisitionCoverageResult.WithoutAcquisitionSeriesCount.ToString("N0", CultureInfo.InvariantCulture)} not backfilled, {datedCount.ToString("N0", CultureInfo.InvariantCulture)} of {ortoDataAcquisitionCoverageResult.ImageCount.ToString("N0", CultureInfo.InvariantCulture)} images dated";
                }
                else
                {
                    // Read, and absent: the part stores no series, so there is nothing to backfill.
                    name = $"{name} - no OrtoDatas";
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

            administrativeAreal2DReferences_Sorted.Sort((x, y) =>
            {
                int result = string.CompareOrdinal(x.Code ?? string.Empty, y.Code ?? string.Empty);

                return result != 0 ? result : x.Id.CompareTo(y.Id);
            });

            ListBoxControl_Counties.SetItems(administrativeAreal2DReferences_Sorted);

            HashSet<int>? countyIds = postgreSQLOrtoDatasAcquisitionUpdateOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }
    }
}
