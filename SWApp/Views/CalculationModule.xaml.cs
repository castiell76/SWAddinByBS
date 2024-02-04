using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SWApp.Models;
using SWApp.VM;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using static PdfSharp.Snippets.Font.NewFontResolver;

namespace SWApp
{
    /// <summary>
    /// Interaction logic for CalculationModule.xaml
    /// </summary>
    public partial class CalculationModule : Window
    {
        public SWObject swObject = new SWObject();
        private readonly SWTreeNode sWTreeNode = new SWTreeNode();
        public CalculationModule()
        {
            InitializeComponent();

        }
        private void LoadSWTreeData_Click(object sender, RoutedEventArgs e)
        {
            swTreeView.ItemsSource = swObject.SWTreeInit();
        }

        private void cmAddMaterial_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (SWTreeNode)swTreeView.SelectedItem;
            selectedItem.AddMaterial(new Material() { Name = "Blacha", Description="JANEK", ID=1 });
            
        }

        private void swTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is SWTreeNode selectedNode)
            {
                
                dgCalculationModuleOperations.ItemsSource = selectedNode.Operations;
            }
        }

        private void cmOpenNode_Click(object sender, RoutedEventArgs e)
        {
            var selectedNode = (SWTreeNode) swTreeView.SelectedItem;
            swObject.OpenSelectedPart(selectedNode.Path);
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
            var selectedNode = (SWTreeNode)swTreeView.SelectedItem;
            swObject.OpenSelectedPart(selectedNode.Path);
            CalculationPartVM calcPart = swObject.CalculateConvertedPart(true);
            selectedNode.Assets = calcPart.CalculationItems;
            //selectedNode.Materials = calcPart.Materials;
            //dgCalculationModuleMaterials.ItemsSource = selectedNode.Materials;
            dgCalculationModuleOperations.ItemsSource = selectedNode.Assets;
        }
    }
}
