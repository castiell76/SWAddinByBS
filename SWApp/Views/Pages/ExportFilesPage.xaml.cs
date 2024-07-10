using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SWApp.Views.Pages
{
    /// <summary>
    /// Interaction logic for ExportFilesPage.xaml
    /// </summary>
    public partial class ExportFilesPage : Page
    {
        public ExportFilesPage()
        {
            InitializeComponent();
        }

        private void CbAllDXF_Checked(object sender, RoutedEventArgs e)
        {
            cbPBSheet.IsEnabled = false;
            cbPTSheet.IsEnabled = false;
            cbPBSheet.IsChecked = true;
            cbPTSheet.IsChecked = true;
        }

        private void CbAllDXF_Unchecked(object sender, RoutedEventArgs e)
        {
            cbPBSheet.IsEnabled = true;
            cbPTSheet.IsEnabled = true;
            cbPBSheet.IsChecked = false;
            cbPTSheet.IsChecked = false;
        }
        private void CbCreateDXFForSigma_Checked(object sender, RoutedEventArgs e)
        {
            cbCreateDXF.IsChecked = true;
            ShowTextBoxWithTransition();
        }



        private void cbCreateDXFForSigma_Unchecked(object sender, RoutedEventArgs e)
        {
            HideTextBoxWithTransition();
        }
        private void ShowTextBoxWithTransition()
        {
            // Ustaw Visibility na Visible
            txtSigmaQuantity.Visibility = Visibility.Visible;

            // Tworzenie animacji FadeIn
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };

            // Tworzenie animacji SlideInFromRight
            ThicknessAnimation slideInAnimation = new ThicknessAnimation
            {
                From = new Thickness(100, 0, 0, 0),
                To = new Thickness(0),
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };

            // Tworzenie storyboardu i dodanie animacji
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeInAnimation);
            storyboard.Children.Add(slideInAnimation);

            // Ustawienie celów animacji
            Storyboard.SetTarget(fadeInAnimation, txtSigmaQuantity);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(slideInAnimation, txtSigmaQuantity);
            Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath("Margin"));

            // Uruchomienie animacji
            storyboard.Begin();
        }
        private void HideTextBoxWithTransition()
        {
            // Tworzenie animacji FadeOut
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };

            // Tworzenie animacji SlideOutToRight
            ThicknessAnimation slideOutAnimation = new ThicknessAnimation
            {
                From = new Thickness(0),
                To = new Thickness(100, 0, 0, 0),
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };

            // Tworzenie storyboardu i dodanie animacji
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeOutAnimation);
            storyboard.Children.Add(slideOutAnimation);

            // Ustawienie celów animacji
            Storyboard.SetTarget(fadeOutAnimation, txtSigmaQuantity);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(slideOutAnimation, txtSigmaQuantity);
            Storyboard.SetTargetProperty(slideOutAnimation, new PropertyPath("Margin"));

            // Ustawienie zdarzenia Completed dla storyboardu
            storyboard.Completed += (s, e) =>
            {
                txtSigmaQuantity.Visibility = Visibility.Collapsed;
            };

            // Uruchomienie animacji
            storyboard.Begin();
        }
    }
}
