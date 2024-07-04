using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui;
using System.Drawing;

namespace SWApp.Viewmodels.Pages
{
    public sealed partial class SettingsViewModel : ObservableObject,INavigationAware
    {
        private readonly INavigationService _navigationService;
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appVersion = string.Empty;

        [ObservableProperty]
        private ApplicationTheme _currentApplicationTheme = ApplicationTheme.Unknown;

        [ObservableProperty]
        private NavigationViewPaneDisplayMode _currentApplicationNavigationStyle = NavigationViewPaneDisplayMode.Left;

        public SettingsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
            {
                InitializeViewModel();
            }
        }

        public void OnNavigatedFrom() { }

        partial void OnCurrentApplicationThemeChanged(ApplicationTheme oldValue, ApplicationTheme newValue)
        {
            ApplicationThemeManager.Apply(newValue);
        }

        //partial void OnCurrentApplicationNavigationStyleChanged(
        //    NavigationViewPaneDisplayMode oldValue,
        //    NavigationViewPaneDisplayMode newValue
        //)
        //{
        //    _ = _navigationService.SetPaneDisplayMode(newValue);
        //}

        private void InitializeViewModel()
        {
            CurrentApplicationTheme = ApplicationThemeManager.GetAppTheme();
            AppVersion = $"{GetAssemblyVersion()}";

            ApplicationThemeManager.Changed += OnThemeChanged;

            _isInitialized = true;
        }

        private void OnThemeChanged(ApplicationTheme currentApplicationTheme, System.Windows.Media.Color systemAccent)
        {
            if (CurrentApplicationTheme != currentApplicationTheme)
            {
                CurrentApplicationTheme = currentApplicationTheme;
            }
        }


        private static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
        }
    }
}
