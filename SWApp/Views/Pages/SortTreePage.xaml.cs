using SWApp.Services;
using SWApp.Viewmodels.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace SWApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for SortTreePage.xaml
    /// </summary>
    public partial class SortTreePage : INavigableView<SortTreeViewModel>
    {
        private Point _startPoint;
        private ObservableCollection<string> _items;

        public SortTreeViewModel ViewModel  {get;}

        public SortTreePage()
        {
            
            ViewModel = new SortTreeViewModel();
            InitializeComponent();
            DataContext = ViewModel;
            _items = ViewModel.Items;
            sortTreeListBox.ItemsSource = _items;
            ApplicationThemeManager.Apply(this);
        }
        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }

        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListBox listBox = sender as ListBox;
                ListBoxItem listBoxItem = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);

                if (listBoxItem != null)
                {
                    string item = (string)listBox.ItemContainerGenerator.ItemFromContainer(listBoxItem);
                    DataObject dragData = new DataObject("myFormat", item);
                    DragDrop.DoDragDrop(listBoxItem, dragData, DragDropEffects.Move);
                }
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                string item = e.Data.GetData("myFormat") as string;
                ListBox listBox = sender as ListBox;
                Point dropPosition = e.GetPosition(listBox);
                ListBoxItem listBoxItem = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);

                if (listBoxItem != null)
                {
                    string target = (string)listBox.ItemContainerGenerator.ItemFromContainer(listBoxItem);
                    int targetIndex = listBox.Items.IndexOf(target);

                    _items.Remove(item);
                    _items.Insert(targetIndex, item);
                }
                else
                {
                    _items.Remove(item);
                    _items.Add(item);
                }
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> orderToSort = sortTreeListBox.Items.Cast<string>().ToList();
            bool allLevels = switchAllLevels.IsChecked ?? false;
            bool groupComponents = groupCompononents.IsChecked ?? false;
            ViewModel.SortItems(allLevels, orderToSort, groupComponents);
        }
    }
}
