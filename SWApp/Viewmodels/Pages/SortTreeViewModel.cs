using CommunityToolkit.Mvvm.ComponentModel;
using SolidWorks.Interop.sldworks;
using SWApp.Models;
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
            _items = new ObservableCollection<string> {
                "Złożenia mieszane (AX)",
                "Złożenia blaszane (AB)",
                "Złożenia profilowe (AR)",
                "Złożenia druciane (AD)",
                "Złożenia drewniane (AP)",
                "Złożenia tworzywowe (AT)",
                "Elementy z blachy (PB)",
                "Elementy z profili (PR)",
                "Elementy z drutu (PD)",
                "Elementy z drewna(PP)",
                "Tworzywa Sztuczne (PT)",
                "Mieszane elementy (PX)"};
        }

        public void SortItems(bool allLevels, List<string>input, bool groupComponents)
        {
            var orderToSort = ExtractParts(input);
            _swObject.SortTree(allLevels, orderToSort, groupComponents);

        }
        private List<string> ExtractParts(List<string> input)
        {
            List<string> output = new List<string>();
            string pattern = @"\((.{2})\)";
            foreach (string text in input)
            {
                Match match = Regex.Match(text, pattern);
                string part1 = match.Groups[1].Value;
                output.Add(part1);

            }
            return output;
        }
    }
}
