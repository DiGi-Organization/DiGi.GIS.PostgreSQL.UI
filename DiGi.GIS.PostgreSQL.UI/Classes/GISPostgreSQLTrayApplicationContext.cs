using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.UI.Enums;
using DiGi.GIS.PostgreSQL.UI.Windows;
using DiGi.GIS.WebAPI.Classes;
using DiGi.UI.WPF.Interfaces;
using DiGi.User.PostgreSQL.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Resources;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Provides the application context for the GIS PostgreSQL tray application, managing its lifecycle and dependencies.
    /// </summary>
    public class GISPostgreSQLTrayApplicationContext : DiGi.UI.Windows.Classes.TrayApplicationContext<MainWindow>
    {
        private readonly Mode? mode = null;

        private readonly GISPostgreSQLConverterManager? gISPostgreSQLConverterManager = PostgreSQL.Create.GISPostgreSQLConverterManager();

        // Fully qualified: from this namespace an unqualified PostgreSQL binds to DiGi.GIS.PostgreSQL, and the
        // users this application creates live in the database DiGi.User.PostgreSQL names, not in that one.
        private readonly UserPostgreSQLConverterManager? userPostgreSQLConverterManager = DiGi.User.PostgreSQL.Create.UserPostgreSQLConverterManager();

        private readonly GISWebAPIManager? GISWebAPIManager = WebAPI.Create.GISWebAPIManager(Create.GISPostgreSQLConverterManagerConfigurationFile()?.Key);

        /// <summary>
        /// Initializes a new instance of the <see cref="GISPostgreSQLTrayApplicationContext"/> class.
        /// <para>The tray icon is animated while any client or server background task is running and returns to the static icon once every task has stopped - finished, failed or cancelled.</para>
        /// </summary>
        public GISPostgreSQLTrayApplicationContext()
            : base("GIS PostgreSQL", new DiGi.UI.Windows.Classes.TrayApplicationContextOptions() { ShowWindowOnStartup = true, HideOnMinimize = true, HideOnDeactivate = false })
        {
            SetIcon(SmallIcon("DiGi.GIS.ico"));

            List<Icon> icons_Animation = [];
            foreach (string fileName in new[] { "DiGi.GIS.Running.1.ico", "DiGi.GIS.Running.2.ico", "DiGi.GIS.Running.3.ico" })
            {
                Icon? icon = SmallIcon(fileName);
                if (icon is not null)
                {
                    icons_Animation.Add(icon);
                }
            }

            SetAnimationIcons(icons_Animation, TimeSpan.FromMilliseconds(250));

            MainWindow? mainWindow = Window;
            if (mainWindow is null)
            {
                return;
            }

            foreach (ObservableCollection<IVisualBackgroundTask>? visualBackgroundTasks in new[] { mainWindow.VisualBackgroundTasks_Client, mainWindow.VisualBackgroundTasks_Server })
            {
                if (visualBackgroundTasks is null)
                {
                    continue;
                }

                visualBackgroundTasks.CollectionChanged += OnVisualBackgroundTasksChanged;
                foreach (IVisualBackgroundTask visualBackgroundTask in visualBackgroundTasks)
                {
                    visualBackgroundTask.PropertyChanged += OnVisualBackgroundTaskPropertyChanged;
                }
            }

            UpdateAnimation();

            static Icon? SmallIcon(string fileName)
            {
                try
                {
                    StreamResourceInfo? streamResourceInfo = System.Windows.Application.GetResourceStream(new Uri(string.Format("pack://application:,,,/DiGi.GIS.PostgreSQL.UI;component/Resources/{0}", fileName)));
                    if (streamResourceInfo?.Stream is not Stream stream)
                    {
                        return null;
                    }

                    using (stream)
                    {
                        return new Icon(stream, SystemInformation.SmallIconSize);
                    }
                }
                catch (IOException)
                {
                    return null;
                }
            }
        }

        private void OnVisualBackgroundTasksChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
            {
                foreach (IVisualBackgroundTask visualBackgroundTask in e.OldItems.OfType<IVisualBackgroundTask>())
                {
                    visualBackgroundTask.PropertyChanged -= OnVisualBackgroundTaskPropertyChanged;
                }
            }

            if (e.NewItems is not null)
            {
                foreach (IVisualBackgroundTask visualBackgroundTask in e.NewItems.OfType<IVisualBackgroundTask>())
                {
                    visualBackgroundTask.PropertyChanged += OnVisualBackgroundTaskPropertyChanged;
                }
            }

            UpdateAnimation();
        }

        private void OnVisualBackgroundTaskPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == nameof(IVisualBackgroundTask.IsRunning))
            {
                UpdateAnimation();
            }
        }

        /// <summary>
        /// Returns the hover popup text listing every running client and server task with its live status and duration.
        /// </summary>
        /// <returns>One line per running task under a header, or <see langword="null"/> when no task runs, which shows the native tooltip.</returns>
        protected override string? GetHoverText()
        {
            MainWindow? mainWindow = Window;
            if (mainWindow is null)
            {
                return null;
            }

            List<IVisualBackgroundTask> visualBackgroundTasks = [];
            foreach (ObservableCollection<IVisualBackgroundTask>? visualBackgroundTasks_Temp in new[] { mainWindow.VisualBackgroundTasks_Client, mainWindow.VisualBackgroundTasks_Server })
            {
                if (visualBackgroundTasks_Temp is not null)
                {
                    visualBackgroundTasks.AddRange(visualBackgroundTasks_Temp.Where(x => x.IsRunning));
                }
            }

            if (visualBackgroundTasks.Count == 0)
            {
                return null;
            }

            StringBuilder stringBuilder = new();
            stringBuilder.Append(string.Format("GIS PostgreSQL - {0} running task{1}", visualBackgroundTasks.Count, visualBackgroundTasks.Count == 1 ? string.Empty : "s"));
            foreach (IVisualBackgroundTask visualBackgroundTask in visualBackgroundTasks)
            {
                stringBuilder.AppendLine();
                stringBuilder.Append(string.Format("{0}: {1} [{2}]", visualBackgroundTask.Name, visualBackgroundTask.Status, visualBackgroundTask.ExecutionTimeSpanText));
            }

            return stringBuilder.ToString();
        }

        private void UpdateAnimation()
        {
            MainWindow? mainWindow = Window;
            if (mainWindow is null)
            {
                return;
            }

            bool running = (mainWindow.VisualBackgroundTasks_Client?.Any(x => x.IsRunning) ?? false) || (mainWindow.VisualBackgroundTasks_Server?.Any(x => x.IsRunning) ?? false);

            SetAnimated(running);
        }

        /// <summary>
        /// Creates and returns the main window associated with this application context.
        /// </summary>
        /// <returns>An instance of the <see cref="MainWindow"/> class.</returns>
        protected override MainWindow GetWindow()
        {
            return new(gISPostgreSQLConverterManager, userPostgreSQLConverterManager, GISWebAPIManager, mode);
        }
    }
}
