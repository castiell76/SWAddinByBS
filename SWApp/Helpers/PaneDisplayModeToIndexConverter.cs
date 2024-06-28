using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Wpf.Ui.Controls;

namespace SWApp.Helpers
{
    internal sealed class PaneDisplayModeToIndexConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                NavigationViewPaneDisplayMode.LeftFluent => 1,
                NavigationViewPaneDisplayMode.Top => 2,
                NavigationViewPaneDisplayMode.Bottom => 3,
                _ => 0
            };
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                1 => NavigationViewPaneDisplayMode.LeftFluent,
                2 => NavigationViewPaneDisplayMode.Top,
                3 => NavigationViewPaneDisplayMode.Bottom,
                _ => NavigationViewPaneDisplayMode.Left
            };
        }
    }
}
