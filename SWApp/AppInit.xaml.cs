using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SWApp.Services;
using SWApp.Services.Contracts;
using SWApp.Viewmodels;
using SWApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Wpf.Ui;

namespace SWApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class AppInit 
    {
        static AppInit()
        {

        }
        private static readonly Microsoft.Extensions.Hosting.IHost _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(c =>
            {
                _ = c.SetBasePath(AppContext.BaseDirectory);
            })
            .ConfigureServices(
                (_1, services) =>
                {
                    // App Host
                    _ = services.AddHostedService<ApplicationHostService>();

                    // Main window container with navigation
                    _ = services.AddSingleton<IWindow, MainWindow>();
                    _ = services.AddSingleton<MainWindowViewModel>();
                    _ = services.AddSingleton<INavigationService, Wpf.Ui.NavigationService>();
                    _ = services.AddSingleton<ISnackbarService, SnackbarService>();
                    _ = services.AddSingleton<IContentDialogService, ContentDialogService>();
                    _ = services.AddSingleton<WindowsProviderService>();


                    // Top-level pages
                    //_ = services.AddSingleton<DashboardPage>();
                    //_ = services.AddSingleton<DashboardViewModel>();
                    //_ = services.AddSingleton<AllControlsPage>();
                    //_ = services.AddSingleton<AllControlsViewModel>();
                    //_ = services.AddSingleton<SettingsPage>();
                    //_ = services.AddSingleton<SettingsViewModel>();

                    //// All other pages and view models
                    //_ = services.AddTransientFromNamespace("Wpf.Ui.Gallery.Views", GalleryAssembly.Asssembly);
                    //_ = services.AddTransientFromNamespace(
                    //    "Wpf.Ui.Gallery.ViewModels",
                    //    GalleryAssembly.Asssembly
                    //);
                }
            )
            .Build();

        public static T GetRequiredService<T>()
        where T : class
        {
            return _host.Services.GetRequiredService<T>();
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            _host.Start();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private void OnExit(object sender, ExitEventArgs e)
        {
            _host.StopAsync().Wait();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
        public static async Task InitializeAsync()
        {
            await _host.StartAsync(); // Rozpoczęcie hosta
        }

        public static async Task ShutdownAsync()
        {
            await _host.StopAsync(); // Zatrzymanie hosta
        }
    }

}
