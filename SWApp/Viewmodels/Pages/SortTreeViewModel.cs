using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            _items = new ObservableCollection<string> { "Elementy blaszane (AB/PB)", "Elementy z profili (AR/PR)", "Tworzywa Sztuczne (AT/PT)", "Elementy z drewna(AP/PP)", "Mieszane elementy AX/PX" };
        }

        public void SortItems(bool allLevels)
        {
            _swObject.SortTree(allLevels);
        }
    }
}
