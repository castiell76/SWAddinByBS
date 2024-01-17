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
        public CalculationModule()
        {
            InitializeComponent();

        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            swTreeView.ItemsSource = swObject.SWTreeInit();

        }
        
    }
}
