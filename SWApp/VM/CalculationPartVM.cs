using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWApp.Models;

namespace SWApp.VM
{
    public class CalculationPartVM
    {
        public ObservableCollection<Material> Materials { get; set; }
        public ObservableCollection<Operation> Operations { get; set; }
    }
}
