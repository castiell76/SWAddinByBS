using CommunityToolkit.Mvvm.ComponentModel;
using EnvDTE80;
using SWApp.Models;
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

        public void SetProperties(List<CustomProperty> customProperties, string[]optionsStr, bool[] options)
        {
            List<string> doneParts = new List<string>();
            _swObject.SetProperties(doneParts, customProperties,optionsStr, options);
        }

        public void ReadProperties()
        {
            _swObject.ReadProperties();
        }
    }
}
