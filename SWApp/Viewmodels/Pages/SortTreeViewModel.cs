using CommunityToolkit.Mvvm.ComponentModel;
using SolidWorks.Interop.sldworks;
using SWApp.Models;
using SWApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

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
        private HelpService _helpService;
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
            _helpService = new HelpService();
            _swObject.ErrorOccurred += OnErrorOccured;
        }

        private void OnErrorOccured(string title, string message, ControlAppearance appearance, SymbolIcon icon)
        {
            _helpService.SnackbarService.Show(title, message, appearance, icon, TimeSpan.FromSeconds(3));
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
