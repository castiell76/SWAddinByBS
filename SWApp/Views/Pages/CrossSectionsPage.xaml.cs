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
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
            
        }
        public CrossSectionsPage() : this(new CrossSectionsViewmodel())
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

    }
}
