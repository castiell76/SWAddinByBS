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

namespace SWApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : INavigableView<SettingsViewModel>
    {
        public SettingsViewModel ViewModel { get; }
        public SettingsViewModel SettingsViewModel { get; }

        public event EventHandler ThemeChanged;
        private MainWindow _mainWindow;
        public SettingsPage(SettingsViewModel viewModel, MainWindow mainWindow)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();

        }
        public SettingsPage() : this(HelpService.GetRequiredService<SettingsViewModel>())
        {
            DataContext = new SettingsViewModel(HelpService.GetRequiredService<IThemeService>());
            InitializeComponent();
        }

        public SettingsPage(SettingsViewModel settingsViewModel)
        {
            SettingsViewModel = settingsViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }



    }
}
