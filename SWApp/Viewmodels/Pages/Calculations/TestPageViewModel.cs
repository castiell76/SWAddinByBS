using SWApp.Controls;
using SWApp.ControlsLookup;
using SWApp.Views.Pages.Calculations;
using SWApp.Views.Pages.Calculations.CalculationSteps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWApp.Viewmodels.Pages.Calculations
{
    public class TestPageViewModel
    {
        public ObservableCollection<NavigationCard> NavigationCards { get; set; }
        public TestPageViewModel()
        {
            NavigationCards = new ObservableCollection<NavigationCard>(
        ControlPages
            .FromNamespace(typeof(BendingPage).Namespace!)
            .Select(x => new NavigationCard()
            {
                Name = x.Name,
                Icon = x.Icon,
                Description = x.Description,
                PageType = x.PageType
            })
    );


        }

    }
}
