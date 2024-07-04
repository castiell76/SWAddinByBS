using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using SWApp.Views;
using Microsoft.Extensions.Configuration;
using SWApp.Services;
using SWApp.Services.Contracts;
using SWApp.Viewmodels;
using Wpf.Ui;
using System.Windows.Navigation;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Threading;

namespace WF_Host
{
    [ComVisible(true)]
    [Guid("8F1992E8-2E0D-4110-BC7B-3D9400BB932B")]
    partial class HostUserControl : UserControl
    {
        private System.ComponentModel.IContainer components = null;
        private ElementHost elementHost2;
        private MainWindow mainWindow2;
        //private static IHost _host;
        //private static INavigationService _navigationService;
        //private static IServiceProvider _serviceProvider;
        //private static ISnackbarService _snackbarService;
        //private static IContentDialogService _contentDialogService;
        //public event RoutedEventHandler Loaded;
        //public static void Initialize()
        //{
        //    _host = Host.CreateDefaultBuilder()
        //          .ConfigureAppConfiguration(c =>
        //          {
        //              _ = c.SetBasePath(AppContext.BaseDirectory);
        //          })
        //          .ConfigureServices(
        //              (_1, services) =>
        //              {
        //                  // App Host
        //                  _ = services.AddHostedService<ApplicationHostService>();

        //                  // Main window container with navigation
        //                  _ = services.AddSingleton<IWindow, MainWindow>();
        //                  _ = services.AddSingleton<MainWindowViewModel>();
        //                  _ = services.AddSingleton<INavigationService, Wpf.Ui.NavigationService>();
        //                  _ = services.AddSingleton<ISnackbarService, SnackbarService>();
        //                  _ = services.AddSingleton<IContentDialogService, ContentDialogService>();
        //                  _ = services.AddSingleton<WindowsProviderService>();


        //                  // Top-level pages
        //                  //_ = services.AddSingleton<DashboardPage>();
        //                  //_ = services.AddSingleton<DashboardViewModel>();
        //                  //_ = services.AddSingleton<AllControlsPage>();
        //                  //_ = services.AddSingleton<AllControlsViewModel>();
        //                  //_ = services.AddSingleton<SettingsPage>();
        //                  //_ = services.AddSingleton<SettingsViewModel>();

        //                  //// All other pages and view models
        //                  //_ = services.AddTransientFromNamespace("Wpf.Ui.Gallery.Views", GalleryAssembly.Asssembly);
        //                  //_ = services.AddTransientFromNamespace(
        //                  //    "Wpf.Ui.Gallery.ViewModels",
        //                  //    GalleryAssembly.Asssembly
        //                  //);
        //              }
        //          )
        //          .Build();

        //    _navigationService = _host.Services.GetRequiredService<INavigationService>();
        //    _serviceProvider = _host.Services.GetRequiredService<IServiceProvider>();
        //    _snackbarService = _host.Services.GetRequiredService<ISnackbarService>();
        //    _contentDialogService = _host.Services.GetRequiredService<IContentDialogService>();
        //}
        //private void InjectDependencies()
        //{
        //    _navigationService = GetRequiredService<INavigationService>();
        //    _serviceProvider = GetRequiredService<IServiceProvider>();
        //    _snackbarService = GetRequiredService<ISnackbarService>();
        //    _contentDialogService = GetRequiredService<IContentDialogService>();

        //    // Use injected dependencies
        //    //ainWindow2.Initialize(_navigationService, _serviceProvider, _snackbarService, _contentDialogService);
        //}
        //public static T GetRequiredService<T>()
        //where T : class
        //{
        //    return _host.Services.GetRequiredService<T>();
        //}

        ///// <summary>
        ///// Occurs when the application is loading.
        ///// </summary>
        //private static void OnStartup(object sender, StartupEventArgs e)
        //{
        //    _host.Start();
        //}

        ///// <summary>
        ///// Occurs when the application is closing.
        ///// </summary>
        //private static void OnExit(object sender, ExitEventArgs e)
        //{
        //    _host.StopAsync().Wait();

        //    _host.Dispose();
        //}

        ///// <summary>
        ///// Occurs when an exception is thrown by an application but not handled.
        ///// </summary>
        //private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        //{
        //    // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        //}
        //public static async Task InitializeAsync()
        //{
        //    await _host.StartAsync(); // Rozpoczęcie hosta
        //}

        //public static async Task ShutdownAsync()
        //{
        //    await _host.StopAsync(); // Zatrzymanie hosta
        //}
        private void InitializeComponent()
        {
            //Initialize();
            components = new System.ComponentModel.Container();
            elementHost2 = new ElementHost();
            mainWindow2 = new MainWindow();

            SuspendLayout();

            // elementHost2
            elementHost2.Dock = DockStyle.Fill;
            elementHost2.Location = new System.Drawing.Point(0, 0);
            elementHost2.Margin = new Padding(4, 3, 4, 3);
            elementHost2.Name = "elementHost2";
            elementHost2.TabIndex = 1;
            elementHost2.Child = mainWindow2;
           
            // HostUserControl
            AccessibleRole = AccessibleRole.ScrollBar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            Controls.Add(elementHost2);
            ForeColor = System.Drawing.SystemColors.Control;
            Margin = new Padding(4, 3, 4, 3);
            Name = "HostUserControl";
            Size = new System.Drawing.Size(630, 800);

            ResumeLayout(false);
        }
    }
}