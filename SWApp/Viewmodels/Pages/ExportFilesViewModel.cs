using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using SolidWorks.Interop.swconst;
using SWApp.Controls;
using SWApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace SWApp.Viewmodels.Pages
{
    public partial class ExportFilesViewModel : ObservableObject, INotifyPropertyChanged
    {

        private SWObject _swObject;
        private HelpService _helpService;
        private ViewControl _viewControl;
        private ObservableCollection<ExportStatus> _exportStatuses;
        private readonly Dispatcher _dispatcher;


        public ExportFilesViewModel()
        {
            _swObject = new SWObject();
            _helpService = new HelpService();
            _dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;
            _viewControl = new ViewControl();
            _swObject.ErrorOccurred += OnModelErrorOccurred;
        }
        public ObservableCollection<ExportStatus> ExportStatuses
        {
            get => _exportStatuses ?? (_exportStatuses = new ObservableCollection<ExportStatus>());
            set
            {
                _exportStatuses = value;
                OnPropertyChanged(nameof(ExportStatuses));
            }
        }
        public  void ExportFilesAsync(bool[] options, int quantitySigma, string filedirToSave, List<string> filters)
        {
            try
            {
                _exportStatuses = _swObject.ExportFromAssembly(options, quantitySigma, filedirToSave, filters);
            }

            catch(Exception ex)
            {
                OnModelErrorOccurred("Uwaga", "Błąd", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24));
            }
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
        private void OnModelErrorOccurred(string title, string message, ControlAppearance controlAppearance, SymbolIcon icon)
        {
          
            _dispatcher.Invoke(() =>
            {
                _helpService.SnackbarService.Show(title, message, controlAppearance, icon, TimeSpan.FromSeconds(3));
            });

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void OpenSelectedComponent(System.Windows.Controls.DataGrid dataGrid)
        {
            ExportStatus component = (ExportStatus)dataGrid.SelectedItem;
            string filepath = component.filepath;
            _swObject.OpenSelectedPart(filepath);
           
        }
    }
}
