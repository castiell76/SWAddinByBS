using SWApp.Controls;
using SWApp.Helpers;
using SWApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Wpf.Ui.Controls;
using static SWApp.Helpers.RALToRGBConverter;

namespace SWApp.Viewmodels.Pages
{
    public class ColorElementsViewModel 
    {
        private SWObject _swObject;
        private ViewControl _viewControl;
        private HelpService _helpService;
        private RALToRGBConverter.RalColorConverter _rgbConverter;
        private ObservableCollection<RALToRGBConverter.RalColor> _ralColors;

        public ObservableCollection<RALToRGBConverter.RalColor> RalColors { get { return _ralColors; } set { _ralColors = value; } }

        public ColorElementsViewModel()
        {
            _swObject = new SWObject();
            _helpService = new HelpService();
            _viewControl = new ViewControl();
            _rgbConverter = new RALToRGBConverter.RalColorConverter();
            RalColors = _rgbConverter.GetColors();
            _swObject.ErrorOccurred += OnErrorOccured;
        }

        public void SetColorPart(string ralName, bool[] options, string details)
        {
            var ralCode = RalColors.FirstOrDefault(c => c.Name.Equals(ralName, StringComparison.OrdinalIgnoreCase));
            _swObject.SetColorPart(ralCode, options, details);
           
        }
        public void OnErrorOccured(string title, string message, ControlAppearance appearance, SymbolIcon icon)
        {
            _helpService.SnackbarService.Show(title, message, appearance, icon, TimeSpan.FromSeconds(3));
        }
    }
}
