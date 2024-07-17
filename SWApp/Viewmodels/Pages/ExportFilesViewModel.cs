using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWApp.Viewmodels.Pages
{
    public class ExportFilesViewModel
    {
        private SWObject _swObject;
        public ExportFilesViewModel()
        {
            _swObject = new SWObject();
        }
        public void ExportFiles() 
        {
            _swObject.ExportFile();
        }
    }
}
