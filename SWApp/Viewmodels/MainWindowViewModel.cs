
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Wpf.Ui.Controls;
using SWApp.Views.Pages;

namespace SWApp.Viewmodels
{
    public partial class MainWindowViewModel : ObservableObject
    {

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new ObservableCollection<object>
        {
            new NavigationViewItem("Home",typeof(CrossSectionsPage))
            
        };

    }
}
