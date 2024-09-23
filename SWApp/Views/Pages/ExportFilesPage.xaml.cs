using SWApp.Controls;
using SWApp.Services;
using SWApp.Viewmodels;
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
using Binding = System.Windows.Data.Binding;
using Plotly.NET;

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

        private void btnExport_Click(object sender, RoutedEventArgs e)
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
            options[2] = cbDXFFromDrawing.IsChecked ?? false;
            options[3] = cbAllDXF.IsChecked ?? false;
            options[4] = cbCreateDXFForSigma.IsChecked ?? false;
            options[5] = cbSketchInclude.IsChecked ?? false;
            options[6] = cbFormingToolsInclude.IsChecked ?? false;

            if (cbCreateDXF.IsChecked == false && cbCreateSTEP.IsChecked == false)
            {
                _helpSerivce.SnackbarService.Show("Uwaga!", "Wybierz opcję eksportu DXF lub STEP", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24),
                TimeSpan.FromSeconds(3));
            }
            else if (int.TryParse(txtSigmaQuantity.Text.ToString(), out quantitySigma) && options[6])
            {
                _helpSerivce.SnackbarService.Show("Uwaga!", "Wprowadź poprawny nakład do sigmy!", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24),
                TimeSpan.FromSeconds(3));
            }
            else if (!ViewModel.IsValidPath(filedirToSave))
            {
                _helpSerivce.SnackbarService.Show("Uwaga!", "Wprowadź poprawną ścieżkę zapisu!", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24),
                TimeSpan.FromSeconds(3));
            }
            else
            {
                try
                {
                    btnExport.IsEnabled = false;
                    ViewModel.ExportFilesAsync(options, quantitySigma, filedirToSave, filters);
                    dgExport.Visibility = Visibility.Visible;
                    dgExport.AutoGeneratingColumn += dgPrimaryGrid_AutoGeneratingColumn;
                    dgExport.ItemsSource = ViewModel.ExportStatuses;
            }
                catch (Exception ex)
                {
                _helpSerivce.SnackbarService.Show("Błąd!", ex.Message, ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24), TimeSpan.FromSeconds(3));
            }
                finally
                {
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
            if (desc?.Name == "type")
            {
                var templateColumn = new DataGridTemplateColumn
                {
                    Header = e.Column.Header,
                    SortMemberPath = desc.Name
                };

                var template = new DataTemplate();
                var factory = new FrameworkElementFactory(typeof(System.Windows.Controls.Image));
                factory.SetValue(System.Windows.Controls.Image.WidthProperty, 25.0);
                factory.SetValue(System.Windows.Controls.Image.HeightProperty, 25.0);

                factory.SetValue(System.Windows.Controls.Image.VerticalAlignmentProperty, VerticalAlignment.Center);

                var binding = new System.Windows.Data.Binding("type")
                {
                    Converter = (IValueConverter)this.Resources["TypeToIconConverter"]
                };
                factory.SetBinding(System.Windows.Controls.Image.SourceProperty, binding);

                template.VisualTree = factory;
                templateColumn.CellTemplate = template;

                e.Column = templateColumn;
            }
        }

        private void cbAllDXF_Checked(object sender, RoutedEventArgs e)
        {
            flyout.IsOpen = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            flyout.IsOpen = false;
        }
    }
}
