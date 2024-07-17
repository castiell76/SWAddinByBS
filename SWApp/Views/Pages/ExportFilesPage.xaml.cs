using SWApp.Controls;
using SWApp.Services;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace SWApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for ExportFilesPage.xaml
    /// </summary>
    public partial class ExportFilesPage : INavigableView<ExportFilesViewModel>
    {

        public ExportFilesViewModel ViewModel { get; }
        public ViewControl _viewControl;
        private HelpService _helpSerivce;

        public ExportFilesPage() 
        {
            InitializeComponent();
            ViewModel = new ExportFilesViewModel();
            _helpSerivce = new HelpService();
            _viewControl= new ViewControl();
        }

        private void CbAllDXF_Checked(object sender, RoutedEventArgs e)
        {
            cbPBSheet.IsEnabled = false;
            cbPTSheet.IsEnabled = false;
            cbPBSheet.IsChecked = true;
            cbPTSheet.IsChecked = true;
        }

        private void CbAllDXF_Unchecked(object sender, RoutedEventArgs e)
        {
            cbPBSheet.IsEnabled = true;
            cbPTSheet.IsEnabled = true;
            cbPBSheet.IsChecked = false;
            cbPTSheet.IsChecked = false;
        }
        private void CbCreateDXFForSigma_Checked(object sender, RoutedEventArgs e)
        {
            cbCreateDXF.IsChecked = true;
            _viewControl.ShowWithTransition(txtSigmaQuantity);
        }



        private void cbCreateDXFForSigma_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewControl.HideWithTransition(txtSigmaQuantity);
        }

        private void miExportOpen_Click(object sender, RoutedEventArgs e)
        {
            //SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            //ExportStatus exportToOpen = (ExportStatus)dgExport.SelectedItem;
            //string filepath = exportToOpen.filepath;
            //swApp.OpenDoc6(filepath, (int)swDocumentTypes_e.swDocPART, 0, "", 0, 0);
            //swApp.ActivateDoc3(System.IO.Path.GetFileName(filepath), false, 0, 0);
        }


        private void btnShowTable_Click(object sender, RoutedEventArgs e)
        {
            if(dgExport.Visibility == Visibility.Hidden || dgExport.Visibility == Visibility.Collapsed)
            {
                _viewControl.ShowWithTransition(dgExport);
            }
            else
            {
                _viewControl.HideWithTransition(dgExport);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(cbCreateDXF.IsChecked == false || cbCreateSTEP.IsChecked == false)
            {
                _helpSerivce.SnackbarService.Show("Uwaga!", "Wybierz opcję eksportu DXF lub STEP", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Fluent24),
                TimeSpan.FromSeconds(3));
            }
            else
            {
                ViewModel.ExportFiles();
            }

        }
    }
}
