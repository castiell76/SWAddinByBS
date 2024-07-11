﻿using CommunityToolkit.Mvvm.ComponentModel;
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
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;

namespace SWApp.Viewmodels.Pages
{
    public sealed partial class SettingsViewModel : ObservableObject,INavigationAware
    {
        private bool _isInitialized = false;


        [ObservableProperty]
        private string _appVersion = string.Empty;
        [ObservableProperty]
        private IThemeService _themeService;

        [ObservableProperty]
        private Wpf.Ui.Appearance.ApplicationTheme _currentApplicationTheme = Wpf.Ui
            .Appearance
            .ApplicationTheme
            .Unknown;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
            {
                InitializeViewModel();
            }
        }
        public SettingsViewModel(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            CurrentApplicationTheme = Wpf.Ui.Appearance.ApplicationThemeManager.GetAppTheme();
            AppVersion = $"Wpf.Ui.Demo.Mvvm - {GetAssemblyVersion()}";

            _isInitialized = true;
        }

        private static string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                ?? string.Empty;
        }
        [RelayCommand]
        public void OnChangeTheme(string parameter)
        {
            _themeService.SetTheme(ApplicationTheme.Dark);
            switch (parameter)
            {
                case "theme_light":
                    if (CurrentApplicationTheme == Wpf.Ui.Appearance.ApplicationTheme.Light)
                    {
                        break;
                    }

                    Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Light);
                    CurrentApplicationTheme = Wpf.Ui.Appearance.ApplicationTheme.Light;

                    break;

                default:
                    if (CurrentApplicationTheme == Wpf.Ui.Appearance.ApplicationTheme.Dark)
                    {
                        break;
                    }

                    Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Dark);
                    CurrentApplicationTheme = Wpf.Ui.Appearance.ApplicationTheme.Dark;

                    break;
            }
        }
    }
}