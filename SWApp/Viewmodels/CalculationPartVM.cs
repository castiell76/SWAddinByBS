using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using SWApp.Models;
using Material = SWApp.Models.Material;

namespace SWApp.Viewmodels
{
    public class CalculationPartVM
    {
        public ObservableCollection<Operation> Operations {  get; set; }
        public ObservableCollection<Material>  Materials { get; set; }
        public ObservableCollection<CalculationItem> CalculationItems { get; set; }
        public CalculationPartVM()
        {
            // Inicjalizacja kolekcji
            CalculationItems = new ObservableCollection<CalculationItem>();

            // Dodanie przykładowych danych (możesz oczywiście dodać własną logikę pobierania danych)
            Materials = new ObservableCollection<Material>();

            Operations = new ObservableCollection<Operation>();

            // Dodanie elementów z Materials do CalculationItems
            foreach (var material in Materials)
            {
                CalculationItems.Add(new CalculationItem(material));
            }

            // Dodanie elementów z Operations do CalculationItems
            foreach (var operation in Operations)
            {
                CalculationItems.Add(new CalculationItem(operation));
            }
        }
    }
}

