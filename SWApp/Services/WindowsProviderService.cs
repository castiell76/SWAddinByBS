using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWApp.Services
{
    public class WindowsProviderService
    {
        private readonly IServiceProvider _serviceProvider;

        public WindowsProviderService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show<T>()
            where T : class
        {
            if (!typeof(System.Windows.Window).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException($"The window class should be derived from {typeof(System.Windows.Window)}.");
            }

            System.Windows.Window windowInstance =
                _serviceProvider.GetService<T>() as System.Windows.Window
                ?? throw new InvalidOperationException("Window is not registered as service.");
            windowInstance.Owner = System.Windows.Application.Current.MainWindow;
            windowInstance.Show();
        }
    }
}
