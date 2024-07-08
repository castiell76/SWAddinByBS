using NPOI.SS.Formula.Functions;
using SWApp.Services;
using SWApp.Viewmodels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui;
using Wpf.Ui.Controls;


namespace SWApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for CrossSectionsPage.xaml
    /// </summary>
    public partial class CrossSectionsPage : INavigableView<CrossSectionsViewmodel>
    {
        public CrossSectionsViewmodel ViewModel { get; }
        public CrossSectionsPage(CrossSectionsViewmodel viewModel) 
        {
            
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

        }
        public CrossSectionsPage() : this(HelpService.GetRequiredService<CrossSectionsViewmodel>())
        {
            DataContext = ViewModel;
            InitializeComponent();
        }
        private void miAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Add();
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
           // ViewModel.Delete(dataGridProfile);

        }
        private void MiCopy_Click(object sender, RoutedEventArgs e)
        {
            //ViewModel.Copy(dataGridProfile);
        }

        private void MiPaste_Click(object sender, RoutedEventArgs e)
        {
            //ViewModel.Paste(dataGridProfile);
        }

        private void DataGridProfile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
               // ViewModel.Delete(dataGridProfile);
            }
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
               // ViewModel.Copy(dataGridProfile);
            }
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
               // ViewModel.Paste(dataGridProfile);
            }
            if (e.Key == Key.N && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                ViewModel.Add();
            }
            if(e.Key == Key.Tab)
            {
                System.Windows.MessageBox.Show("grid");
            }
        }

        private void btnGenerateCrossSections_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GenerateCrossSections();
        }

        private void StackPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                System.Windows.MessageBox.Show("stackpanel");
            }
        }
    }
}

