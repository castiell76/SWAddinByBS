using SWApp.Controls;
using SWApp.Models;
using SWApp.Services;
using SWApp.Viewmodels.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace SWApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for FilesPropertiesPage.xaml
    /// </summary>
    public partial class FilesPropertiesPage : Page
    {
        private ViewControl _viewControl;
        private FilesPropertiesViewModel _viewModel;
        private IContentDialogService _contentDialogService;
        public FilesPropertiesPage()
        {

            InitializeComponent();
            _viewControl = new ViewControl();
            _viewModel = new FilesPropertiesViewModel();
            DataContext = _viewModel;
            _contentDialogService=HelpService.GetRequiredService<IContentDialogService>();
            ApplicationThemeManager.Apply(this);

        }

        private void cbSetIndex_Checked(object sender, RoutedEventArgs e)
        {
            _viewControl.ShowWithTransition(tbIndex);
        }

        private void cbCheckedBy_Checked(object sender, RoutedEventArgs e)
        {
            _viewControl.ShowWithTransition(comboCheckedBy);
        }

        private void cbCheckedBy_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewControl.HideWithTransition(comboCheckedBy);
        }

        private void cbDevelopedBy_Checked(object sender, RoutedEventArgs e)
        {
            _viewControl.ShowWithTransition(comboDevelopedBy);
        }

        private void cbDevelopedBy_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewControl.HideWithTransition(comboDevelopedBy);
        }

        private void cbSetIndex_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewControl.HideWithTransition(tbIndex);
        }

        private void cbCustomProperties_Checked(object sender, RoutedEventArgs e)
        {
            _viewControl.ShowWithTransition(customProperty);
        }

        private void cbCustomProperties_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (UIElement dockpanel in customProperties.Children)
            {
                if (dockpanel is DockPanel)
                {
                    _viewControl.HideWithTransition(dockpanel);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DockPanel newDockPanel = new DockPanel();
            newDockPanel.Margin = new Thickness(0, 10, 0, 0);
            // Tworzenie TextBox dla nazwy właściwości
            Wpf.Ui.Controls.TextBox nameTextBox = new Wpf.Ui.Controls.TextBox
            {
                PlaceholderText = "Nazwa właściwości"
            };
            DockPanel.SetDock(nameTextBox, Dock.Left);
            newDockPanel.Children.Add(nameTextBox);

            // Tworzenie TextBox dla wartości właściwości
            Wpf.Ui.Controls.TextBox valueTextBox = new Wpf.Ui.Controls.TextBox
            {
                PlaceholderText = "Wartość właściwości",
                Margin = new Thickness(10, 0, 0, 0)
            };
            DockPanel.SetDock(valueTextBox, Dock.Left);
            newDockPanel.Children.Add(valueTextBox);

            Wpf.Ui.Controls.Button deleteButton = new Wpf.Ui.Controls.Button
            {
                BorderThickness = new Thickness(0),
                Icon = new SymbolIcon(SymbolRegular.DismissSquare24),
                FontSize = 25,
            };
            deleteButton.Click += DeleteButton_Click;
            DockPanel.SetDock(deleteButton, Dock.Left);
            newDockPanel.Children.Add(deleteButton);
            // Dodanie nowego DockPanel do StackPanel
            customProperties.Children.Add(newDockPanel);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Wpf.Ui.Controls.Button;
            if (button != null)
            {
                var dockPanel = button.Parent as DockPanel;
                if (dockPanel != null)
                {
                    var stackPanel = dockPanel.Parent as StackPanel;
                    if (stackPanel != null)
                    {
                        stackPanel.Children.Remove(dockPanel);
                    }
                }
            }
        }

        private void btnShowTable_Click(object sender, RoutedEventArgs e)
        {
            if (dgAllProperties.Visibility == Visibility.Hidden || dgAllProperties.Visibility == Visibility.Collapsed)
            {
                _viewControl.ShowWithTransition(dgAllProperties);
            }
            else
            {
                _viewControl.HideWithTransition(dgAllProperties);
            }
        }

        private void btnSetProperties_Click(object sender, RoutedEventArgs e)
        {
            SetProperties();
            dgAllProperties.ItemsSource = _viewModel.ReadProperties();
            dgAllProperties.Visibility = Visibility.Visible;
        }
        private void SetProperties()
        {
            List<CustomProperty> customPropertiesToGive = new List<CustomProperty>();
            foreach (var child in customProperties.Children)
            {
                if (child is DockPanel dockPanel)
                {
                    string name = string.Empty;
                    string value = string.Empty;

                    foreach (var panelChild in dockPanel.Children)
                    {
                        if (panelChild is Wpf.Ui.Controls.TextBox textBox)
                        {
                            if (string.IsNullOrEmpty(name))
                            {
                                name = textBox.Text;
                            }
                            else
                            {
                                value = textBox.Text;
                            }
                        }
                        customPropertiesToGive.Add(new CustomProperty { name = name, value = value });
                    }

                }

                string[] optionsStr =
            {
                "",
                "",
                "",
                "",
                "materialTest",
                comboDevelopedBy.Text,
                comboCheckedBy.Text,
                tbIndex.Text,
            };
                bool[] options =
                {
                cbCopyPropsToAllConfigs.IsChecked ?? false,
                cbSetQuantity.IsChecked ?? false,
                cbSetThickness.IsChecked ?? false,
                cbClearNums.IsChecked ?? false,
                cbSetNums.IsChecked ?? false,
                cbDevelopedBy.IsChecked ?? false,
                cbCheckedBy.IsChecked ?? false,
                false,
                cbSetIndex.IsChecked ?? false,

            };
                _viewModel.SetProperties(customPropertiesToGive, optionsStr, options);
            }
        
    }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dgAllProperties.ItemsSource = _viewModel.ReadProperties();
            dgAllProperties.Visibility = Visibility.Visible;
        }
    }
}
