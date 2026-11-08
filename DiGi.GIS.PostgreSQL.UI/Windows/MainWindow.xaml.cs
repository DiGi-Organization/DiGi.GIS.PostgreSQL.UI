using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.UI.Enums;
using DiGi.GIS.WebAPI.Classes;
using DiGi.UI.WPF.Interfaces;
using DiGi.User.PostgreSQL.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    public partial class MainWindow : Window
    {
        private readonly GISPostgreSQLConverterManager? gISPostgreSQLConverterManager;
        private readonly GISWebAPIManager? GISWebAPIManager;
        private readonly Mode? mode;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The manager responsible for GIS PostgreSQL conversion processes.</param>
        /// <param name="userPostgreSQLConverterManager">The manager holding the converter for the user database, or null where no User_PostgreSQL_Main.conf names one.</param>
        /// <param name="GISWebAPIManager">The manager responsible for GIS PostgreSQL Web API interactions.</param>
        /// <param name="mode">The operational mode of the application. If null, it is determined based on converter availability.</param>
        public MainWindow(GISPostgreSQLConverterManager? gISPostgreSQLConverterManager, UserPostgreSQLConverterManager? userPostgreSQLConverterManager, GISWebAPIManager? GISWebAPIManager, Mode? mode = null)
        {
            this.gISPostgreSQLConverterManager = gISPostgreSQLConverterManager;
            this.GISWebAPIManager = GISWebAPIManager;
            this.mode = mode;

            if (this.mode is null)
            {
                this.mode = Mode.Client;
                if (this.gISPostgreSQLConverterManager is not null && this.gISPostgreSQLConverterManager.IsAvailable<Building2DPostgreSQLConverter>())
                {
                    this.mode = Mode.ServerAndClient;
                }
            }

            // Initialize collections before InitializeComponent to avoid binding errors
            List<IVisualBackgroundTask>? visualBackgroundTasks;

            visualBackgroundTasks = Create.VisualBackgroundTasks(gISPostgreSQLConverterManager, userPostgreSQLConverterManager, GISWebAPIManager, Mode.Client);
            if (visualBackgroundTasks is not null)
            {
                VisualBackgroundTasks_Client = [.. visualBackgroundTasks];

                //IVisualBackgroundTask? visualBackgroundTask = visualBackgroundTasks.Find(x => x.TypeName == typeof(OrtoDatasFromDatabasePostTask).Name);
                //visualBackgroundTask?.Start();
            }

            visualBackgroundTasks = Create.VisualBackgroundTasks(gISPostgreSQLConverterManager, userPostgreSQLConverterManager, GISWebAPIManager, Mode.Server);
            if (visualBackgroundTasks is not null)
            {
                VisualBackgroundTasks_Server = [.. visualBackgroundTasks];
            }

            InitializeComponent();

            // Setting the DataContext for bindings to work
            DataContext = this;
        }

        /// <summary>
        /// Gets the current operational mode of the application.
        /// </summary>
        public Mode Mode
        {
            get
            {
                if (mode is null || !mode.HasValue)
                {
                    return Mode.Client;
                }

                return mode.Value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of visual background tasks associated with the client operational mode.
        /// </summary>
        public ObservableCollection<IVisualBackgroundTask>? VisualBackgroundTasks_Client { get; set; }

        /// <summary>
        /// Gets or sets the collection of visual background tasks associated with the server operational mode.
        /// </summary>
        public ObservableCollection<IVisualBackgroundTask>? VisualBackgroundTasks_Server { get; set; }

        private void Window_Initialized(object sender, System.EventArgs e)
        {
            if (Mode == Mode.Client)
            {
                TabItem_Server.Visibility = Visibility.Hidden;
            }
        }
    }
}
