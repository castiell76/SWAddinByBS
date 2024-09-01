using SWApp.Controls;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static SWApp.Helpers.RALToRGBConverter;

namespace SWApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for ColorElements.xaml
    /// </summary>
    public partial class ColorElements : Page
    {
        private ColorElementsViewModel _viewModel;
        private ViewControl _viewControl;
        private string selectedRal;
        public ColorElementsViewModel ViewModel { get; set; }
        public ColorElements()
        {
            _viewModel = new ColorElementsViewModel();
            ViewModel = _viewModel;
            _viewControl= new ViewControl();
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string details = tbColorDetails.Text;
            bool[] options =
            {
                cbSetTransparency.IsChecked ?? false,
                cbSetEmission.IsChecked ?? false,
                cbSetProperty.IsChecked ?? false,
        };

            _viewModel.SetColorPart(selectedRal, options, details);
        }

        private void comboRALColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboRALColors.SelectedItem is RalColor selectedColor)
            {
                selectedRal = selectedColor.Name;

            }
        }

        private void cbSetProperty_Checked(object sender, RoutedEventArgs e)
        {
            _viewControl.ShowWithTransition(tbColorDetails);
        }

        private void cbSetProperty_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewControl.HideWithTransition(tbColorDetails);
        }
    }
}
