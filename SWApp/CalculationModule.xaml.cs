using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
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
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            swTreeView.ItemsSource = swObject.SWTreeInit();
        }

        private void cmAddMaterial_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (SWTreeNode)swTreeView.SelectedItem;
            selectedItem.AddMaterial(new Material() { Name = "Blacha", Description="JANEK", ID=1 });
            dgCalculationModuleMaterials.ItemsSource = selectedItem.Materials;
        }

        private void swTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is SWTreeNode selectedNode)
            {
                dgCalculationModuleMaterials.ItemsSource = selectedNode.Materials;
                dgCalculationModuleOperations.ItemsSource = selectedNode.Operations;
            }
        }
    }
}
