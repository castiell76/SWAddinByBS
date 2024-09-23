using CommunityToolkit.Mvvm.ComponentModel;
using SWApp.Controls;
using SWApp.ControlsLookup;
using SWApp.Views.Pages.Calculations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SWApp.Viewmodels.Pages.Calculations
{
    public partial class CalculationsPageViewModel
    {
        public ObservableCollection<NavigationCard> NavigationCards { get; set; }
        public CalculationsPageViewModel()
        {
    //        NavigationCards = new ObservableCollection<NavigationCard>(
    //    ControlPages
    //        .FromNamespace(typeof(CalculationsPage).Namespace!)
    //        .Select(x => new NavigationCard()
    //        {
    //            Name = x.Name,
    //            Icon = x.Icon,
    //            Description = x.Description,
    //            PageType = x.PageType
    //        })
    //);


        }
      
    }
}
