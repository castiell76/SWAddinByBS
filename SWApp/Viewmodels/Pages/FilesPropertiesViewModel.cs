using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnvDTE80;
using SWApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;
using Wpf.Ui;
using SWApp.Services;
using System.Threading;

namespace SWApp.Viewmodels.Pages
{
    public partial class FilesPropertiesViewModel
    {
        private SWObject _swObject;
        private IContentDialogService _contentDialogService;
        public ObservableCollection<string> EngineersList { get; set; }

        public FilesPropertiesViewModel()
        {
            EngineersList = new ObservableCollection<string> { "Błaz", "Ktoś", "ktoś2s" };
            
            _swObject = new SWObject();
            _contentDialogService = HelpService.GetRequiredService<IContentDialogService>();
        }

        public void SetProperties(List<CustomProperty> customProperties, string[]optionsStr, bool[] options)
        {
            List<string> doneParts = new List<string>();
            _swObject.SetProperties(doneParts, customProperties,optionsStr, options);
            
        }

        public List<SWFileProperties> ReadProperties()
        {
            List<SWFileProperties> swFilesProperties = new List<SWFileProperties>();
            swFilesProperties = _swObject.ReadProperties();
            return swFilesProperties;
        }

    }
}
