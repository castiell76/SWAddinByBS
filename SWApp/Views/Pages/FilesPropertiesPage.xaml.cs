using SWApp.Controls;
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

namespace SWApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for FilesPropertiesPage.xaml
    /// </summary>
    public partial class FilesPropertiesPage :  Page
    {
        private ViewControl _viewControl;
        public FilesPropertiesPage()
        {
            InitializeComponent();
            _viewControl = new ViewControl();
        }

        private void cbSetIndex_Checked(object sender, RoutedEventArgs e)
        {
            _viewControl.ShowWithTransition(tbIndex);
        }

        private void cbCheckedBy_Checked(object sender, RoutedEventArgs e)
        {
            _viewControl.ShowWithTransition(comboCheckedBy);
        }

        private void cbCheckedBy_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewControl.HideWithTransition(comboCheckedBy);
        }

        private void cbDevelopedBy_Checked(object sender, RoutedEventArgs e)
        {
            _viewControl.ShowWithTransition(comboDevelopedBy);
        }

        private void cbDevelopedBy_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewControl.HideWithTransition(comboDevelopedBy);
        }

        private void cbSetIndex_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewControl.HideWithTransition(tbIndex);
        }
    }
}
