using Microsoft.WindowsAPICodePack.Dialogs;
using SWApp.Services;
using System;
using System.Collections.Generic;
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
        public ExportFilesViewModel()
        {
            _swObject = new SWObject();
            _helpService = new HelpService();
        }
        public void ExportFiles() 
        {
            _swObject.ExportFromAssembly();
        }

        public string ChooseDirectory()
        {
            try
            {
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                CommonFileDialogResult result = dialog.ShowDialog();
                string filepath = dialog.FileName;
                return filepath;
            }

            catch (System.InvalidOperationException)
            {
                _helpService.SnackbarService.Show("Błąd", "Nie wybranu folderu", Wpf.Ui.Controls.ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Fluent24),
                TimeSpan.FromSeconds(3));
                return string.Empty;
            }
        }
    }
}
