using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SWApp
{
    public class SWTreeNode
    {
        public SWTreeNode()
        {
            this.Items = new ObservableCollection<SWTreeNode>();
        }
        public int ID {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SWTreeNode> Childs {  get; set; }
        public SWTreeNode Parent { get; set; }
        public int Quantity { get; set; }

        public ObservableCollection<SWTreeNode> Items { get; set; }

    }
}
