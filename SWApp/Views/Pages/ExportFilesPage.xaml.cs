using SWApp.Controls;
using SWApp.Services;
using SWApp.Viewmodels.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui;
using Wpf.Ui.Appearance;
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
            _viewControl = new ViewControl();
            ApplicationThemeManager.Apply(this);
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
            ViewModel.OpenSelectedComponent(dgExport);
        }


        private void btnShowTable_Click(object sender, RoutedEventArgs e)
        {
            if (dgExport.Visibility == Visibility.Hidden || dgExport.Visibility == Visibility.Collapsed)
            {
                _viewControl.ShowWithTransition(dgExport);
            }
            else
            {
                _viewControl.HideWithTransition(dgExport);
            }
        }

        private async void btnExport_Click(object sender, RoutedEventArgs e)
        {
            
            int quantitySigma;
            bool[] options = new bool[9];
            if (txtSigmaQuantity.Text.ToString() == string.Empty)
            {
                quantitySigma = 0;
            }
            List<string> filters = new List<string>();
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
                _helpSerivce.SnackbarService.Show("Uwaga!", "Wybierz opcję eksportu DXF lub STEP", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Important24),
                TimeSpan.FromSeconds(3));
            }
            else if (int.TryParse(txtSigmaQuantity.Text.ToString(), out quantitySigma) && options[6])
            {
                _helpSerivce.SnackbarService.Show("Uwaga!", "Wprowadź poprawny nakład do sigmy!", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Important24),
                TimeSpan.FromSeconds(3));
            }
            else if (!ViewModel.IsValidPath(filedirToSave))
            {
                _helpSerivce.SnackbarService.Show("Uwaga!", "Wprowadź poprawną ścieżkę zapisu!", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Important24),
                TimeSpan.FromSeconds(3));
            }
            else
            {
                try
                {
                    btnExport.IsEnabled = false;
                    tbExport.Text = "Eksportowanie...";
                    progressRing.IsEnabled = true;
                    progressRing.Visibility = Visibility.Visible;
                    progressRing.IsIndeterminate = true;
                    await ViewModel.ExportFilesAsync(options, quantitySigma, filedirToSave, filters);
                    dgExport.Visibility = Visibility.Visible;
                    dgExport.AutoGeneratingColumn += dgPrimaryGrid_AutoGeneratingColumn;
                    dgExport.ItemsSource = ViewModel.ExportStatuses;
                }
                catch (Exception ex)
                {
                    _helpSerivce.SnackbarService.Show("Błąd!", ex.Message, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Important24), TimeSpan.FromSeconds(3));
                }
                finally
                {
                    progressRing.Visibility = Visibility.Collapsed;
                    progressRing.IsIndeterminate = false;
                    progressRing.IsEnabled = false;
                    btnExport.IsEnabled = true;
                    tbExport.Text = "Eksportuj";
                    
                }
            }
        }

        private void btnChooseDir_Click(object sender, RoutedEventArgs e)
        {
            txtPathDir.Text = ViewModel.ChooseDirectory();
        }

        void dgPrimaryGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var attName = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            var attVis = desc.Attributes[typeof(ColumnVisibilityAttribute)] as ColumnVisibilityAttribute;
            if (attName != null)
            {
                e.Column.Header = attName.Name;

            }
            if (attVis != null)
            {
                e.Column.Visibility = attVis.IsVisible ? Visibility.Visible : Visibility.Hidden;
            }
            if (desc.Name == "filepath")
            {
                var templateColumn = new DataGridTemplateColumn
                {
                    Header = e.Column.Header
                };

                var template = new DataTemplate();
                var factory = new FrameworkElementFactory(typeof(System.Windows.Controls.TextBlock));
                factory.SetBinding(System.Windows.Controls.TextBlock.TextProperty, new Binding(desc.Name)
                {
                    Converter = (IValueConverter)this.Resources["FileNameConverter"]
                });
                template.VisualTree = factory;

                templateColumn.CellTemplate = template;

                e.Column = templateColumn;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
           ChangeTheme();
        }

        public void ChangeTheme()
        {
            var isDarkTheme = ApplicationThemeManager.GetAppTheme();
            var mainWindow = HelpService.GetRequiredService<MainWindow>();

            if (isDarkTheme == ApplicationTheme.Light)
            {
                ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                ApplicationThemeManager.Apply(this);
                ApplicationThemeManager.Apply(mainWindow);
                var dataprovider = (System.Windows.Data.XmlDataProvider)(
  ((UserControl)(el.Child)).Resources["rssData"]
);
                dataprovider.Refresh();
                //ThemeChanged?.Invoke(this, true);

            }
            else
            {
                ApplicationThemeManager.Apply(ApplicationTheme.Light);
                ApplicationThemeManager.Apply(this);
                //ThemeChanged?.Invoke(this, false);
            }

        }
    }
}
