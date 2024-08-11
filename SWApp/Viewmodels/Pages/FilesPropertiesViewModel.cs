using CommunityToolkit.Mvvm.ComponentModel;
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWApp.Viewmodels.Pages
{
    public partial class FilesPropertiesViewModel
    {
        private SWObject _swObject;
        public ObservableCollection<string> EngineersList { get; set; }

        public FilesPropertiesViewModel()
        {
            EngineersList = new ObservableCollection<string> { "Błaz", "Ktoś", "ktoś2s" };
            _swObject = new SWObject();
        }

        public void SetProperties()
        {
            //_swObject.Set
        }
    }
}
