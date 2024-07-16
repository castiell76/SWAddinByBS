using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWApp.Viewmodels.Pages
{
    public class SortTreeViewModel : ObservableObject
    {
        public ObservableCollection<string> Items
        {
            get
            {
                return _items;
            }
            set { _items = value; }
        }

        private ObservableCollection<string> _items;
        private SWObject _swObject = new SWObject();
        public SortTreeViewModel() 
        {
            _items = new ObservableCollection<string> { "Elementy blaszane (AB/PB)", "Elementy z profili (AR/PR)", "Tworzywa Sztuczne (AT/PT)", "Elementy z drewna(AP/PP)", "Elementy z drutu (AD/PD)", "Mieszane elementy (AX/PX)" };
        }

        public void SortItems(bool allLevels, List<string>input)
        {
            var orderToSort = ExtractParts(input);
            _swObject.SortTree(allLevels, orderToSort);
        }
        private List<string> ExtractParts(List<string> input)
        {
            List<string> output = new List<string>();
            string pattern = @"\(([A-Z]{1,2})/([A-Z]{1,2})\)";
            foreach (string text in input)
            {
                Match match = Regex.Match(text, pattern);
                string part1 = match.Groups[1].Value;
                string part2 = match.Groups[2].Value;
                output.Add(part1);
                output.Add(part2);
            }
            return output;
        }
    }
}
