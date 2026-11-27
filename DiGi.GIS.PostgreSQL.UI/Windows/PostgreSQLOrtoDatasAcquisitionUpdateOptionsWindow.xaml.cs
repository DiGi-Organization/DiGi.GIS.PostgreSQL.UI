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
    /// Interaction logic for PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow.xaml
    /// <para>Asks for the county parts an acquisition-date backfill of <c>orto_datas</c> walks, the last year the stored series were fetched up to, and the paging of the walk. Unlike most county-scoped tasks, no selection means nothing rather than every county: the backfill reads every stored series it reaches, about 0.5 MB each, so the scope is always named.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#70), so a run repeated after the tray was restarted is not typed again. Importing fills the controls; the coverage figures shown beside the parts are a display-only window field and never reach the file. Nothing runs until OK, and OK validates the imported values like typed ones.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "The Closed handler cancels and disposes the token source.")]
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

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
            SetControls(this.postgreSQLOrtoDatasAcquisitionUpdateOptions);

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

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: a half-prepared window can be saved and finished after the next import - no county
            // part selected writes no county list, and the range rules (1900 on, page size at least one, timeout
            // at least zero) are OK's gate, not the file's. A box that does not hold its number is still refused,
            // because it could not be written back as typed.
            if (!TryReadOptions(false, out PostgreSQLOrtoDatasAcquisitionUpdateOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export acquisition backfill settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "OrtoDatas_Acquisition_Settings.json"
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
                Title = "Import acquisition backfill settings",
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

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLOrtoDatasAcquisitionUpdateOptions postgreSQLOrtoDatasAcquisitionUpdateOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SetControls(postgreSQLOrtoDatasAcquisitionUpdateOptions_Imported);

            // Said rather than left for OK to find: a county part the list does not hold cannot be selected -
            // the usual sign the settings came from another machine.
            HashSet<int> countyIds = postgreSQLOrtoDatasAcquisitionUpdateOptions_Imported.CountyIds ?? [];
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
        /// <para>The options are written only when every control reads and passes the full set of rules OK has always applied - the last year from 1900 on, a page size of at least one row, a timeout of at least zero, at least one county part; a refusal leaves them as they were.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out PostgreSQLOrtoDatasAcquisitionUpdateOptions? options, out message) || options is null)
            {
                return false;
            }

            postgreSQLOrtoDatasAcquisitionUpdateOptions.LastYear = options.LastYear;
            postgreSQLOrtoDatasAcquisitionUpdateOptions.PageSize = options.PageSize;
            postgreSQLOrtoDatasAcquisitionUpdateOptions.CommandTimeout = options.CommandTimeout;
            postgreSQLOrtoDatasAcquisitionUpdateOptions.CountyIds = options.CountyIds;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) applies every rule the window has always applied, in the order it has always applied them, and finishes with those messages. Export reads the same controls without the range rules, so a half-prepared window can be saved: no county part selected is written as no county list, never as an empty set, and a last year below 1900 still exports, to be refused by OK on read-back. What is refused on both paths is what could not be written back as typed: a box that does not hold its number. The coverage figures beside the parts are a display-only window field and never reach the file.</para>
        /// </summary>
        /// <param name="complete">True for OK: every rule applies. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out PostgreSQLOrtoDatasAcquisitionUpdateOptions? options, out string? message)
        {
            options = null;
            message = null;

            bool lastYear_Read = TextBoxControl_LastYear.TryGetValue(out int lastYear);
            bool pageSize_Read = TextBoxControl_PageSize.TryGetValue(out int pageSize);
            bool commandTimeout_Read = TextBoxControl_CommandTimeout.TryGetValue(out int commandTimeout);

            if (!lastYear_Read || (complete && lastYear < 1900))
            {
                message = "The last year has to be a whole number from 1900 on.";
                return false;
            }

            if (!pageSize_Read || (complete && pageSize < 1))
            {
                message = "Page size has to be a whole number of rows, one or greater.";
                return false;
            }

            if (!commandTimeout_Read || (complete && commandTimeout < 0))
            {
                message = "Command timeout has to be a whole number of seconds, zero (no timeout) or greater.";
                return false;
            }

            // No selection is written as no county list (null); this window's scope is always named by OK, so
            // an unfinished export simply carries none.
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
                message = "Select at least one county part.";
                return false;
            }

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            PostgreSQLOrtoDatasAcquisitionUpdateOptions postgreSQLOrtoDatasAcquisitionUpdateOptions_Temp = new(postgreSQLOrtoDatasAcquisitionUpdateOptions)
            {
                LastYear = lastYear,
                PageSize = pageSize,
                CommandTimeout = commandTimeout,
                CountyIds = countyIds,
            };

            options = postgreSQLOrtoDatasAcquisitionUpdateOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before: options with no county list clear the
        /// selection. Internal so a fact can fill a window from a settings file the way Import does, without a
        /// file dialog.
        /// </summary>
        /// <param name="postgreSQLOrtoDatasAcquisitionUpdateOptions">The options to show.</param>
        internal void SetControls(PostgreSQLOrtoDatasAcquisitionUpdateOptions postgreSQLOrtoDatasAcquisitionUpdateOptions)
        {
            TextBoxControl_LastYear.Value = postgreSQLOrtoDatasAcquisitionUpdateOptions.LastYear.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_PageSize.Value = postgreSQLOrtoDatasAcquisitionUpdateOptions.PageSize.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_CommandTimeout.Value = postgreSQLOrtoDatasAcquisitionUpdateOptions.CommandTimeout.ToString(CultureInfo.InvariantCulture);

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run. SelectItems deselects the rest,
            // so a file with no county list clears whatever was selected before.
            HashSet<int> countyIds = postgreSQLOrtoDatasAcquisitionUpdateOptions.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one acquisition backfill options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLOrtoDatasAcquisitionUpdateOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLOrtoDatasAcquisitionUpdateOptions>? postgreSQLOrtoDatasAcquisitionUpdateOptionsList = Core.Convert.ToDiGi<PostgreSQLOrtoDatasAcquisitionUpdateOptions>(json);
            if (postgreSQLOrtoDatasAcquisitionUpdateOptionsList is null || postgreSQLOrtoDatasAcquisitionUpdateOptionsList.Count != 1)
            {
                message = "The file holds no acquisition backfill settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLOrtoDatasAcquisitionUpdateOptionsList[0];
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
