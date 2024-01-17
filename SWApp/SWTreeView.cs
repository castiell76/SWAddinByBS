using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SWApp
{
    public class SWTreeView
    {
        public SWTreeView()
        {
            this.Items = new ObservableCollection<SWTreeNode>();
        }

        public string Title { get; set; }

        public ObservableCollection<SWTreeNode> Items { get; set; }
    }
}
