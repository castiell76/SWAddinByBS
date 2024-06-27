using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SWApp.Models;

namespace SWApp.Viewmodels
{
    public class SWTreeNode : INotifyPropertyChanged
    {
        public SWTreeNode()
        {
            Items = new ObservableCollection<SWTreeNode>();
            Materials = new ObservableCollection<Material>();
            Operations = new ObservableCollection<Operation>();
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<Material> _materials { get; set; }
        public ObservableCollection<Material> Materials
        {
            get { return _materials; }
            set { _materials = value; OnPropertyChanged(); }
        }
        public ObservableCollection<CalculationItem> Assets { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public ObservableCollection<Operation> Operations { get; set; }
        public List<SWTreeNode> Childs { get; set; }
        public SWTreeNode Parent { get; set; }
        public int Quantity { get; set; }

        public ObservableCollection<SWTreeNode> Items { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddMaterial(Material material)
        {
            Materials.Add(material);
            OnPropertyChanged();
        }

    }
}
