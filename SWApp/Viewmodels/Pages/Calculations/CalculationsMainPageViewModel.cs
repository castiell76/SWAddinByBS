using SWApp.Controls;
using SWApp.ControlsLookup;
using SWApp.Views.Pages.Calculations;
using SWApp.Views.Pages.Calculations.CalculationSteps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SWApp.Viewmodels.Pages.Calculations
{
    public class CalculationsMainPageViewModel : INotifyPropertyChanged
    {
        private SWObject swObject;

        private ObservableCollection<SWTreeNode> _swTreeNodes;

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<SWTreeNode> SWTreeNodes {  get { return _swTreeNodes; } set { _swTreeNodes = new ObservableCollection<SWTreeNode>(value); OnPropertyChanged(nameof(SWTreeNodes)); } }

        public CalculationsMainPageViewModel()
        {
            _swTreeNodes = new ObservableCollection<SWTreeNode>();
            swObject = new SWObject();
        }

        public IEnumerable<SWTreeNode> SWTreeInit()
        {
            return swObject.SWTreeInit();
        }

        public void OpenSelectedPart(string path)
        {
            swObject.OpenSelectedPart(path);
        }

        public void AddNode()
        {
            SWTreeNode newNode = new SWTreeNode() { Name = "nowa część", Type = "part", Path = "" };
            _swTreeNodes.Add(newNode);
            OnPropertyChanged(nameof(SWTreeNodes));
            
        }

        public void RemoveNode(SWTreeNode node)
        {
            _swTreeNodes.Remove(node);
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MoveNode(SWTreeNode draggedNode, SWTreeNode targetNode)
        {
            if (draggedNode == targetNode || IsChildOf(draggedNode, targetNode))
            {
                // Nie można przenieść węzła jako jego własnego rodzica lub do swojego dziecka
                return;
            }

            // Usuń draggedNode z jego bieżącego rodzica
            if (draggedNode.Parent != null)
            {
                draggedNode.Parent.Items.Remove(draggedNode);
            }

            // Dodaj draggedNode jako dziecko do targetNode
            targetNode.Items.Add(draggedNode);
            draggedNode.Parent = targetNode; // Ustaw targetNode jako nowego rodzica
        }

        private bool IsChildOf(SWTreeNode node, SWTreeNode potentialParent)
        {
            while (node.Parent != null)
            {
                if (node.Parent == potentialParent)
                {
                    return true;
                }
                node = node.Parent;
            }
            return false;
        }

    }
}
