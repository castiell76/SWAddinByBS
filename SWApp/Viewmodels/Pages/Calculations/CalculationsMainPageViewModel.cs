using SWApp.Controls;
using SWApp.ControlsLookup;
using SWApp.Views.Pages.Calculations;
using SWApp.Views.Pages.Calculations.CalculationSteps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWApp.Viewmodels.Pages.Calculations
{
    public class CalculationsMainPageViewModel
    {
        private SWObject swObject;

        private IEnumerable<SWTreeNode> _swTreeNodes;
        public IEnumerable<SWTreeNode> SWTreeNodes {  get { return _swTreeNodes; } set { _swTreeNodes = value; } }

        public CalculationsMainPageViewModel()
        {
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

    }
}
