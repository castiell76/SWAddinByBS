using SWApp.Services;
using SWApp.Viewmodels;
using SWApp.Viewmodels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Application = System.Windows.Application;

namespace SWApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : INavigableView<SettingsViewModel>
    {
        public SettingsViewModel ViewModel { get; }
        public SettingsViewModel SettingsViewModel { get; }
        private MainWindow _mainWindow;

        public SettingsPage(SettingsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
            ApplicationThemeManager.Apply(this);

        }
        public SettingsPage() : this(HelpService.GetRequiredService<SettingsViewModel>())
        {
            HelpService _helpService = new HelpService();
            _mainWindow = HelpService.GetRequiredService<MainWindow>();
            DataContext = new SettingsViewModel(HelpService.GetRequiredService<IThemeService>());
            _mainWindow = HelpService.GetRequiredService<MainWindow>();
            InitializeComponent();
            ApplicationThemeManager.Apply(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeTheme();



        }

        public void ChangeTheme()
        {
            var isDarkTheme = ApplicationThemeManager.GetAppTheme();


            if (isDarkTheme == ApplicationTheme.Light)
            {
                ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                ApplicationThemeManager.Apply(_mainWindow);
                //ThemeChanged?.Invoke(this, true);

            }
            else
            {
                ApplicationThemeManager.Apply(ApplicationTheme.Light);
                ApplicationThemeManager.Apply(_mainWindow);
                //ThemeChanged?.Invoke(this, false);
            }

        }

    }
}
