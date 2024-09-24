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
using TextBox = Wpf.Ui.Controls.TextBox;

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

            if (e.NewValue is SWTreeNode selectedNode)
            {

            }
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

            var nameTextBlock = new FrameworkElementFactory(typeof(Wpf.Ui.Controls.TextBlock));
            nameTextBlock.SetBinding(Wpf.Ui.Controls.TextBlock.TextProperty, new Binding("Name")); // Powiązanie z Name


            var typeImage = new FrameworkElementFactory(typeof(System.Windows.Controls.Image));
            typeImage.SetValue(System.Windows.Controls.Image.WidthProperty, 25.0);
            typeImage.SetValue(System.Windows.Controls.Image.HeightProperty, 25.0);
            typeImage.SetValue(System.Windows.Controls.Image.VerticalAlignmentProperty, VerticalAlignment.Center);


            var binding = new Binding("Type")
            {
                Converter = (IValueConverter)this.Resources["TypeToIconConverter"]
            };
            typeImage.SetBinding(System.Windows.Controls.Image.SourceProperty, binding);


            var stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal); // Ikona i nazwa obok siebie
            stackPanel.AppendChild(typeImage);
            stackPanel.AppendChild(nameTextBlock);


            template.VisualTree = stackPanel;


            template.ItemsSource = new Binding("Items");


            swTreeView.ItemTemplate = template;
        }

        private void cmAddNode_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddNode();

            //CreateTreeViewTemplate();
        }

        private void cmDeleteNode_Click(object sender, RoutedEventArgs e)
        {
            if (swTreeView.SelectedItem is SWTreeNode selectedNode)
            {
                ViewModel.RemoveNode(selectedNode);
            }
        }

        private void swTreeView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Sprawdź, na którym węźle znajduje się kursor
                var item = GetNodeAtMousePosition(e.GetPosition(swTreeView));
                if (item != null)
                {
                    // Rozpocznij przeciąganie
                    DragDrop.DoDragDrop(swTreeView, item, DragDropEffects.Move);
                }
            }
        }

        private void swTreeView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(SWTreeNode)))
            {
                var draggedNode = (SWTreeNode)e.Data.GetData(typeof(SWTreeNode));
                var targetNode = GetNodeAtMousePosition(e.GetPosition(swTreeView));

                if (targetNode != null && targetNode != draggedNode)
                {
                    // Przenieś węzeł (możesz dostosować tę logikę do swoich potrzeb)
                    ViewModel.MoveNode(draggedNode, targetNode);
                }
            }
        }



        private void swTreeView_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(SWTreeNode)))
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Move;
            }
            e.Handled = true;
        }

        private SWTreeNode GetNodeAtMousePosition(Point position)
        {
            var hitTestResult = VisualTreeHelper.HitTest(swTreeView, position);
            if (hitTestResult != null)
            {
                // Znajdź odpowiedni węzeł
                System.Windows.Controls.TreeViewItem treeViewItem = FindAncestor<System.Windows.Controls.TreeViewItem>(hitTestResult.VisualHit);
                if (treeViewItem != null)
                {
                    return (SWTreeNode)treeViewItem.DataContext; // Zwróć właściwy węzeł
                }
            }
            return null;
        }
        private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
    }
}
