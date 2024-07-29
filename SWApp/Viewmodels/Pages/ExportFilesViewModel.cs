﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<ExportStatus> ExportFiles(bool[] options, int quantitySigma, string filedirToSave) 
        {
            ObservableCollection<ExportStatus> exportStatuses = _swObject.ExportFromAssembly(options, quantitySigma, filedirToSave);
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
    }
}
