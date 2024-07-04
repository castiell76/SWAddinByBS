﻿
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Wpf.Ui.Controls;
using SWApp.Views.Pages;
using System.ComponentModel;

namespace SWApp.Viewmodels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<NavigationViewItem> _menuItems = new ObservableCollection<NavigationViewItem>
        {
            new NavigationViewItem("Generator profili",typeof(CrossSectionsPage))
            
        };

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    

}
}