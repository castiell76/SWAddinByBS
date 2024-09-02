
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Wpf.Ui.Controls;
using SWApp.Views.Pages;
using System.ComponentModel;
using Wpf.Ui.Appearance;
using System;
using MenuItem = Wpf.Ui.Controls.MenuItem;
using SWApp.Views.Pages.Calculations;

namespace SWApp.Viewmodels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<NavigationViewItem> _menuItems = new ObservableCollection<NavigationViewItem>
        {
            new NavigationViewItem("Generator profili", SymbolRegular.Cube32,typeof(CrossSectionsPage)),
            new NavigationViewItem("Sortowanie drzewa", SymbolRegular.ArrowSort24,typeof(SortTreePage)),
            new NavigationViewItem("Eksport plików",SymbolRegular.SaveArrowRight24, typeof(ExportFilesPage)),
            new NavigationViewItem("Właściwości plików",SymbolRegular.DocumentBulletList24, typeof(FilesPropertiesPage)),
            new NavigationViewItem("Działania na rysunkach",SymbolRegular.DrawShape24, typeof(DrawingsPage)),
            new NavigationViewItem("Konwertowanie na arkusz blachy",SymbolRegular.ConvertRange24, typeof(ConvertToSheetPage)),
            new NavigationViewItem("Kolorowanie elementów", SymbolRegular.BuildingSwap48, typeof(ColorElements)),
            new NavigationViewItem("Kosztorysowanie", SymbolRegular.CheckboxChecked24, typeof(CalculationsPage))
            {
            MenuItemsSource = new object[]
            {
                new NavigationViewItem("Test", SymbolRegular.Predictions24, typeof(TestPage))
            }
            },

        };
        //[ObservableProperty]
        //private ObservableCollection<object> _bottomMenuItems = new ObservableCollection<object>
        //{
        //    new DropDownButton
        //    {
        //        Icon = new SymbolIcon { Symbol = SymbolRegular.ConvertRange24 },
        //        Content = "Wybierz wygląd",
        //        VerticalAlignment = System.Windows.VerticalAlignment.Bottom,
        //        Flyout = new ContextMenu
        //        {
        //            Items =
        //            {
        //                new MenuItem { Header = "Jasny" },
        //                new MenuItem { Header = "Ciemny" }
        //            }
        //        }
        //    }
        //};

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    } 
}
