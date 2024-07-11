using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;

namespace SWApp.Controls
{
   public class ViewControl
    {
        public void ShowWithTransition(UIElement element)
        {

            // Ustaw Visibility na Visible
            element.Visibility = Visibility.Visible;

            // Tworzenie animacji FadeIn
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.15))
            };

            // Tworzenie animacji SlideInFromRight
            ThicknessAnimation slideInAnimation = new ThicknessAnimation
            {
                From = new Thickness(0, 20, 0, 0),
                To = new Thickness(0),
                Duration = new Duration(TimeSpan.FromSeconds(0.15))
            };

            // Tworzenie storyboardu i dodanie animacji
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeInAnimation);
            storyboard.Children.Add(slideInAnimation);

            // Ustawienie celów animacji
            Storyboard.SetTarget(fadeInAnimation, element);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(slideInAnimation, element);
            Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath("Margin"));

            // Uruchomienie animacji
            storyboard.Begin();
        }
        public void HideWithTransition(UIElement element)
        {
            // Tworzenie animacji FadeOut
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.15))
            };

            // Tworzenie animacji SlideOutToRight
            ThicknessAnimation slideOutAnimation = new ThicknessAnimation
            {
                From = new Thickness(0),
                To = new Thickness(0, 20, 0, 0),
                Duration = new Duration(TimeSpan.FromSeconds(0.15))
            };

            // Tworzenie storyboardu i dodanie animacji
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeOutAnimation);
            storyboard.Children.Add(slideOutAnimation);

            // Ustawienie celów animacji
            Storyboard.SetTarget(fadeOutAnimation, element);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(slideOutAnimation, element);
            Storyboard.SetTargetProperty(slideOutAnimation, new PropertyPath("Margin"));

            // Ustawienie zdarzenia Completed dla storyboardu
            storyboard.Completed += (s, e) =>
            {
                element.Visibility = Visibility.Collapsed;
            };

            // Uruchomienie animacji
            storyboard.Begin();
        }
    }
}
