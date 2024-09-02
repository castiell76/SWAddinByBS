using System.Windows;
using System;
using Wpf.Ui;
using System.Windows.Markup;
using SWApp.Services;

namespace SWApp.Controls
{
    public class NavigationPresenter : System.Windows.Controls.Control
    {
        /// <summary>Identifies the <see cref="ItemsSource"/> dependency property.</summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(object),
            typeof(NavigationPresenter),
            new PropertyMetadata(null)
        );

        /// <summary>Identifies the <see cref="TemplateButtonCommand"/> dependency property.</summary>
        public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(
            nameof(TemplateButtonCommand),
            typeof(Wpf.Ui.Input.IRelayCommand),
            typeof(NavigationPresenter),
            new PropertyMetadata(null)
        );

        public object? ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// Gets the command triggered after clicking the titlebar button.
        /// </summary>
        public Wpf.Ui.Input.IRelayCommand TemplateButtonCommand =>
            (Wpf.Ui.Input.IRelayCommand)GetValue(TemplateButtonCommandProperty);


        public NavigationPresenter()
        {
             SetValue(TemplateButtonCommandProperty, new Wpf.Ui.Input.RelayCommand<Type>(o => OnTemplateButtonClick(o)));
        }

        private void OnTemplateButtonClick(Type? pageType)
        {
            INavigationService navigationService = HelpService.GetRequiredService<INavigationService>();

            if (pageType is not null && navigationService is not null)
            {
                navigationService.Navigate(pageType);
            }
        }
    }
}

