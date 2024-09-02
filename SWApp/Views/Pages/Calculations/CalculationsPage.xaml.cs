using SWApp.Viewmodels.Pages.Calculations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SWApp.Views.Pages.Calculations
{
    /// <summary>
    /// Interaction logic for CalculationsPage.xaml
    /// </summary>
    public partial class CalculationsPage : Page
    {
        public CalculationsPageViewModel ViewModel;
        public CalculationsPage()
        {
            ViewModel = new CalculationsPageViewModel();
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
