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
            NavigationCards = new ObservableCollection<NavigationCard>();
            string targetNamespace = "SWApp.Views.Pages.Calculations";
            var pages = Assembly.GetExecutingAssembly()
                   .GetTypes()
                   .Where(t => t.IsClass &&
                               t.Namespace == targetNamespace &&
                               typeof(Page).IsAssignableFrom(t)) // Filtrujemy tylko klasy typu Page
                   .ToList();

            var cards = pages.Select(page => new NavigationCard
            {
                Name = page.Name,
                Icon = Wpf.Ui.Controls.SymbolRegular.Accessibility16, // Możesz zdefiniować jak przypisać ikonę
                Description = "DefaultDescription", // Możesz zdefiniować jak przypisać opis
                PageType = page
            }).ToList();

            foreach(var card in cards)
            {
                NavigationCards.Add(card);
            }


        }
      
    }
}
