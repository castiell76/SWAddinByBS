using Microsoft.WindowsAPICodePack.Dialogs;
using SWApp.Controls;
using SWApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace SWApp.Viewmodels.Pages
{
    public class ExportFilesViewModel
    {
        private SWObject _swObject;
        private HelpService _helpService;
        private ViewControl _viewControl;
        public ExportFilesViewModel()
        {
            _swObject = new SWObject();
            _helpService = new HelpService();
            _viewControl = new ViewControl();
        }
        public ObservableCollection<ExportStatus> ExportFiles(bool[] options, int quantitySigma, string filedirToSave, List<string>filters) 
        {
            ObservableCollection<ExportStatus> exportStatuses = _swObject.ExportFromAssembly(options, quantitySigma, filedirToSave, filters);
            return exportStatuses;
        }
        public bool IsValidPath(string path, bool allowRelativePaths = false)
        {
            bool isValid = true;

            try
            {
                string fullPath = System.IO.Path.GetFullPath(path);

                if (allowRelativePaths)
                {
                    isValid = System.IO.Path.IsPathRooted(path);
                }
                else
                {
                    string root = System.IO.Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            return isValid;
        }

        public string ChooseDirectory()
        {
            return _viewControl.ChooseDirectory();
        }
    }
}
