using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SWApp.Services.Contracts;
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
    public class HelpService : IWindow
    {
        private static Microsoft.Extensions.Hosting.IHost _host;
        private static INavigationService _navigationService;
        private static IServiceProvider _serviceProvider;
        private static ISnackbarService _snackbarService;
        private static IContentDialogService _contentDialogService;
        private static IThemeService _themeService;

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

                          _ = services.AddSingleton<INavigationService, Wpf.Ui.NavigationService>();
                          _ = services.AddSingleton<IThemeService, ThemeService>();
                          _ = services.AddSingleton<ISnackbarService, SnackbarService>();
                          _ = services.AddSingleton<IContentDialogService, ContentDialogService>();
                          _ = services.AddSingleton<WindowsProviderService>();

                         // _ = services.AddSingleton<INavigationWindow, Views.MainWindow>();
                          _ = services.AddSingleton<Views.Pages.SettingsPage>();
                          _ = services.AddSingleton<Views.Pages.CrossSectionsPage>();
                          _ = services.AddSingleton<Views.Pages.SortTreePage>();
                          _ = services.AddSingleton<Views.Pages.FilesPropertiesPage>();
                          _ = services.AddSingleton<Views.Pages.ExportFilesPage>();
                          _ = services.AddSingleton<Views.Pages.DrawingsPage>();

                          services.AddTransient<SettingsViewModel>();   
                          services.AddTransient<CrossSectionsViewmodel>();
                          
                      }
                  )
                  .Build();

            _navigationService = _host.Services.GetRequiredService<INavigationService>();
            _serviceProvider = _host.Services.GetRequiredService<IServiceProvider>();
            _snackbarService = _host.Services.GetRequiredService<ISnackbarService>();
            _contentDialogService = _host.Services.GetRequiredService<IContentDialogService>();
            _themeService = _host.Services.GetRequiredService<IThemeService>();
        }
        //private void InjectDependencies()
        //{
        //    _navigationService = GetRequiredService<INavigationService>();
        //    _serviceProvider = GetRequiredService<IServiceProvider>();
        //    _snackbarService = GetRequiredService<ISnackbarService>();
        //    _contentDialogService = GetRequiredService<IContentDialogService>();

        //    // Use injected dependencies
        //    //ainWindow2.Initialize(_navigationService, _serviceProvider, _snackbarService, _contentDialogService);
        //}
        public static T GetRequiredService<T>()
        where T : class
        {
            return _host.Services.GetRequiredService<T>();
        }
    }
}
