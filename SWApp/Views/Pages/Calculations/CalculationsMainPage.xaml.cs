using SWApp.ControlsLookup;
using SWApp.Viewmodels;
using SWApp.Viewmodels.Pages.Calculations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Wpf.Ui.Controls;

namespace SWApp.Views.Pages.Calculations
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    /// 
    [SWAppPage("CalculationsMainPage.", SymbolRegular.ControlButton24)]
    public partial class CalculationsMainPage : Page
    {
        public CalculationsMainPageViewModel ViewModel;
        public CalculationsMainPage()
        {
            ViewModel = new CalculationsMainPageViewModel();
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void cmAddMaterial_Click(object sender, RoutedEventArgs e)
        {
            //var selectedItem = (SWTreeNode)swTreeView.SelectedItem;
            //selectedItem.AddMaterial(new Material() { Name = "Blacha", Description = "JANEK", ID = 1 });

        }

        private void swTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //if (e.NewValue is SWTreeNode selectedNode)
            //{

            //    dgCalculationModuleOperations.ItemsSource = selectedNode.Operations;
            //}
        }

        private void cmOpenNode_Click(object sender, RoutedEventArgs e)
        {
            var selectedNode = (SWTreeNode)swTreeView.SelectedItem;
            ViewModel.OpenSelectedPart(selectedNode.Path);

        }


        private void swTreeView_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            swTreeView.Focus();
            var selectedNode = (SWTreeNode)swTreeView.SelectedItem;

            if (selectedNode == null)
            {
                cmDeleteNode.IsEnabled = false;
                cmOpenNode.IsEnabled = false;
            }
            else
            {
                cmDeleteNode.IsEnabled = true;
                cmOpenNode.IsEnabled = true;
            }
        }

        private void AssignOperations_Click(object sender, RoutedEventArgs e)
        {
            //var selectedNode = (SWTreeNode)swTreeView.SelectedItem;
            //swObject.OpenSelectedPart(selectedNode.Path);
            //CalculationPartVM calcPart = swObject.CalculateConvertedPart(true);
            //selectedNode.Assets = calcPart.CalculationItems;
            ////selectedNode.Materials = calcPart.Materials;
            ////dgCalculationModuleMaterials.ItemsSource = selectedNode.Materials;
            //dgCalculationModuleOperations.ItemsSource = selectedNode.Assets;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            swTreeView.ItemsSource = ViewModel.SWTreeInit();
            CreateTreeViewTemplate();
        }

        private void CreateTreeViewTemplate()
        {

            var template = new HierarchicalDataTemplate(typeof(SWTreeNode));

            // Tworzenie TextBlock dla nazwy
            var nameTextBlock = new FrameworkElementFactory(typeof(Wpf.Ui.Controls.TextBlock));
            nameTextBlock.SetBinding(Wpf.Ui.Controls.TextBlock.TextProperty, new Binding("Name")); // Powiązanie z Name

            // Tworzenie Image dla ikony typu
            var typeImage = new FrameworkElementFactory(typeof(System.Windows.Controls.Image));
            typeImage.SetValue(System.Windows.Controls.Image.WidthProperty, 25.0);
            typeImage.SetValue(System.Windows.Controls.Image.HeightProperty, 25.0);
            typeImage.SetValue(System.Windows.Controls.Image.VerticalAlignmentProperty, VerticalAlignment.Center);

            // Powiązanie ikony z typem i użycie konwertera
            var binding = new Binding("Type")
            {
                Converter = (IValueConverter)this.Resources["TypeToIconConverter"]
            };
            typeImage.SetBinding(System.Windows.Controls.Image.SourceProperty, binding);

            // Tworzenie StackPanel jako kontenera dla ikony i nazwy
            var stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal); // Ikona i nazwa obok siebie
            stackPanel.AppendChild(typeImage);  // Dodanie ikony
            stackPanel.AppendChild(nameTextBlock);  // Dodanie nazwy

            // Ustawienie StackPanel jako VisualTree dla HierarchicalDataTemplate
            template.VisualTree = stackPanel;

            // Powiązanie ItemsSource dla elementów dzieci
            template.ItemsSource = new Binding("Items");

            // Ustawienie ItemTemplate dla TreeView
            swTreeView.ItemTemplate = template;
        }
    }
    }
