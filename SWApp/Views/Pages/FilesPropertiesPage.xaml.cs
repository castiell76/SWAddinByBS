using SWApp.Controls;
using SWApp.Models;
using SWApp.Services;
using SWApp.Viewmodels.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
        private bool _isMiddleMouseButtonPressed = false;
        private Point _lastMousePosition;
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
            _viewModel.ReadPropertiesAsync();
            stackpanelWithDatagrid.Visibility = Visibility.Visible;

        }

        private void btnSetProperties_Click(object sender, RoutedEventArgs e)
        {
            SetProperties();

        }
        private void SetProperties()
        {
            List<CustomProperty> customPropertiesToGive = new List<CustomProperty>();
            string material =  comboMaterial.Text;
            string[] optionsStr =
 {
                "",
                "",
                "",
                "",
                material,
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
                cbCustomProperties.IsChecked ?? false,
                cbSetIndex.IsChecked ?? false,
                cbSetMaterial.IsChecked ?? false,

            };
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
                
            }
        if (!(options.All(x => x == false)))
        {
            _viewModel.SetProperties(customPropertiesToGive, optionsStr, options);
            _viewModel.ReadPropertiesAsync();
            stackpanelWithDatagrid.Visibility = Visibility.Visible;
        }
        else
        {
            _viewModel.OnErrorOccured("Uwaga", "Wybierz właściwości do nadania", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24));
        }
    }

        private void dgAllProperties_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var attName = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            var attVis = desc.Attributes[typeof(ColumnVisibilityAttribute)] as ColumnVisibilityAttribute;
            if (attName != null)
            {
                e.Column.Header = attName.Name;

            }
            if (attVis != null)
            {
                e.Column.Visibility = attVis.IsVisible ? Visibility.Visible : Visibility.Hidden;
            }
            if (desc.Name == "filepath")
            {
                var templateColumn = new DataGridTemplateColumn
                {
                    Header = e.Column.Header,
                    SortMemberPath = desc.Name
                };

                var template = new DataTemplate();
                var factory = new FrameworkElementFactory(typeof(System.Windows.Controls.TextBlock));
                factory.SetBinding(System.Windows.Controls.TextBlock.TextProperty, new System.Windows.Data.Binding(desc.Name)
                {
                    Converter = (IValueConverter)this.Resources["FileNameConverter"]
                });


                template.VisualTree = factory;

                templateColumn.CellTemplate = template;

                e.Column = templateColumn;
            }

            if (desc?.Name == "type")
            {
                var templateColumn = new DataGridTemplateColumn
                {
                    Header = e.Column.Header,
                    SortMemberPath = desc.Name
                };

                var template = new DataTemplate();
                var factory = new FrameworkElementFactory(typeof(System.Windows.Controls.Image));
                factory.SetValue(System.Windows.Controls.Image.WidthProperty, 25.0);
                factory.SetValue(System.Windows.Controls.Image.HeightProperty, 25.0);

                factory.SetValue(System.Windows.Controls.Image.VerticalAlignmentProperty, VerticalAlignment.Center);

                var binding = new System.Windows.Data.Binding("type")
                {
                    Converter = (IValueConverter)this.Resources["TypeToIconConverter"]
                };
                factory.SetBinding(System.Windows.Controls.Image.SourceProperty, binding);

                template.VisualTree = factory;
                templateColumn.CellTemplate = template;

                e.Column = templateColumn;
            }
        }

        private void miPropertiesOpen_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.OpenSelectedComponent(dgAllProperties);
        }

        private void dgAllProperties_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((System.Windows.Controls.Control)sender).Parent as UIElement;
                parent?.RaiseEvent(eventArg);
            }
        }


        private ScrollViewer GetScrollViewer(DependencyObject obj)
        {
            if (obj is ScrollViewer)
            {
                return (ScrollViewer)obj;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                var result = GetScrollViewer(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private void dgAllProperties_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                _isMiddleMouseButtonPressed = true;
                _lastMousePosition = e.GetPosition(dgAllProperties);
                dgAllProperties.CaptureMouse();  // Przechwycenie myszy
            }
        }
    

        private void dgAllProperties_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isMiddleMouseButtonPressed)
            {
                var currentPosition = e.GetPosition(dgAllProperties);
                var deltaX = currentPosition.X - _lastMousePosition.X;
                var deltaY = currentPosition.Y - _lastMousePosition.Y;

                var scrollViewer = GetScrollViewer(dgAllProperties);
                if (scrollViewer != null)
                {

                    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - deltaX);
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - deltaY);
                }


                _lastMousePosition = currentPosition;
            }
        }

        private void dgAllProperties_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                _isMiddleMouseButtonPressed = false;
                dgAllProperties.ReleaseMouseCapture();  // Zakończenie przechwytywania myszy
            }
        }

        private void dgAllProperties_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.Header.ToString() == "nr rysunku")
            {
                e.Handled = true;

                var dataView = CollectionViewSource.GetDefaultView(dgAllProperties.ItemsSource) as ListCollectionView;
                if (dataView != null)
                {
                    var direction = (e.Column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;

                    dataView.CustomSort = new NaturalSorting.NaturalStringComparer();

                    e.Column.SortDirection = direction;
                }
            }
        }

        private void btnRevision_Click(object sender, RoutedEventArgs e)
        {
            char revision = _viewModel.SetRevision();
            if(revision != ' ')
            {
                tbRevision.Text = "Obecna rewizja pliku: " + revision;
            }
            
        }

        private void cbSetMaterial_Checked(object sender, RoutedEventArgs e)
        {
            _viewControl.ShowWithTransition(comboMaterial);
        }

        private void cbSetMaterial_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewControl.HideWithTransition(comboMaterial);
            comboMaterial.Text = string.Empty;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveToExcel(dgAllProperties);
        }
    }
}
