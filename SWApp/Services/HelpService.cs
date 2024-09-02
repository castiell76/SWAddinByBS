using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SWApp.Viewmodels;
using SWApp.Viewmodels.Pages;
using SWApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui;

namespace SWApp.Services
{
    public class HelpService 
    {
        private static Microsoft.Extensions.Hosting.IHost _host;
        private static INavigationService _navigationService;
        private static IServiceProvider _serviceProvider;
        private static ISnackbarService _snackbarService;
        private static IContentDialogService _contentDialogService;
        private static IThemeService _themeService;
        private MainWindow _mainWindow;

        public INavigationService NavigationService { get { return _navigationService; } }
        public IServiceProvider ServiceProvider { get { return _serviceProvider; } }
        public ISnackbarService SnackbarService { get { return _snackbarService; } }
        public IContentDialogService ContentDialogService {  get { return _contentDialogService; } }
        public IThemeService ThemeService { get { return _themeService; } }

        public event RoutedEventHandler Loaded;



        public void Show()
        {
            throw new NotImplementedException();
        }
        public void GetServices()
        {
            _host = Host.CreateDefaultBuilder()
                  //.ConfigureAppConfiguration(c =>
                  //{
                  //    _ = c.SetBasePath(AppContext.BaseDirectory);
                  //})
                  .ConfigureServices(
                      (_1, services) =>
                      {
                          // App Host
                          _ = services.AddHostedService<ApplicationHostService>();

                          


                          //// Main window container with navigation
                          _ = services.AddSingleton<MainWindow>();
                          _ = services.AddSingleton<MainWindowViewModel>();
                          _ = services.AddSingleton<Views.MainWindow>();

                          _ = services.AddSingleton<INavigationService, Wpf.Ui.NavigationService>();
                          _ = services.AddSingleton<IThemeService, ThemeService>();
                          _ = services.AddSingleton<ISnackbarService, SnackbarService>();
                          _ = services.AddSingleton<IContentDialogService, ContentDialogService>();
                          _ = services.AddSingleton<WindowsProviderService>();

                         // _ = services.AddSingleton<INavigationWindow, Views.MainWindow>();
                          
                          _ = services.AddSingleton<Views.Pages.CrossSectionsPage>();
                          _ = services.AddSingleton<Views.Pages.SortTreePage>();
                          _ = services.AddSingleton<Views.Pages.FilesPropertiesPage>();
                          _ = services.AddSingleton<HelpService>();
                          _ = services.AddSingleton<Views.Pages.ExportFilesPage>();
                          _ = services.AddSingleton<Views.Pages.DrawingsPage>();
                          _ = services.AddSingleton<Views.Pages.Calculations.CalculationsPage>();
                          _ = services.AddSingleton<Views.Pages.Calculations.TestPage>();
                          services.AddTransient<CrossSectionsViewmodel>();
                          services.AddTransient<FilesPropertiesViewModel>();
                          
                      }
                  )
                  .Build();

            _navigationService = _host.Services.GetRequiredService<INavigationService>();
            _serviceProvider = _host.Services.GetRequiredService<IServiceProvider>();
            _snackbarService = _host.Services.GetRequiredService<ISnackbarService>();
            _contentDialogService = _host.Services.GetRequiredService<IContentDialogService>();
            _themeService = _host.Services.GetRequiredService<IThemeService>();
        }
        public static T GetRequiredService<T>()
        where T : class
        {
            return _host.Services.GetRequiredService<T>();
        }

    }
}
