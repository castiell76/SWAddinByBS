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
            cbPBSheet.IsChecked = false;
            cbPTSheet.IsChecked = false;
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
            int quantitySigma;
            bool[] options = new bool[9];
            if(txtSigmaQuantity.Text.ToString() == string.Empty)
            {
                quantitySigma = 0;
            }
            List<string>filters = new List<string>();
            string filedirToSave = txtPathDir.Text.ToString();
            options[0] = cbCreateDXF.IsChecked ?? false;
            options[1] = cbCreateSTEP.IsChecked ?? false;
            options[2] = cbPBSheet.IsChecked ?? false;
            options[3] = cbPTSheet.IsChecked ?? false;
            options[4] = cbDXFFromDrawing.IsChecked ?? false;
            options[5] = cbAllDXF.IsChecked ?? false;
            options[6] = cbCreateDXFForSigma.IsChecked ?? false;
            options[7] = cbSketchInclude.IsChecked ?? false;
            options[8] = cbFormingToolsInclude.IsChecked ?? false;

            if (options[2] && options[3])
            {
                filters.Add("PB");
                filters.Add("PT");
            }
            else if (options[3])
            {
                filters.Add("PT");
            }
            else if (options[2])
            {
                filters.Add("PB");
            }
            if (cbCreateDXF.IsChecked == false && cbCreateSTEP.IsChecked == false)
            {
                _helpSerivce.SnackbarService.Show("Uwaga!", "Wybierz opcję eksportu DXF lub STEP", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Fluent24),
                TimeSpan.FromSeconds(3));
            }
            else if (int.TryParse(txtSigmaQuantity.Text.ToString(), out quantitySigma) && options[6])
            {
                _helpSerivce.SnackbarService.Show("Uwaga!", "Wprowadź poprawny nakład do sigmy!", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Fluent24),
                TimeSpan.FromSeconds(3));
            }
            else if (!ViewModel.IsValidPath(filedirToSave))
            {
                _helpSerivce.SnackbarService.Show("Uwaga!", "Wprowadź poprawną ścieżkę zapisu!", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Fluent24),
                TimeSpan.FromSeconds(3));
            }
            else
            {
                dgExport.ItemsSource = ViewModel.ExportFiles(options, quantitySigma, filedirToSave,filters);
                dgExport.Visibility= Visibility.Visible;
            }

        }

        private void btnChooseDir_Click(object sender, RoutedEventArgs e)
        {
            txtPathDir.Text = ViewModel.ChooseDirectory();
        }
    }
}
