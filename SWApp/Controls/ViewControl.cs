using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Forms;

namespace SWApp.Controls
{
   public class ViewControl
    {
        public void ShowWithTransition(UIElement element)
        {

     
            element.Visibility = Visibility.Visible;

  
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.15))
            };


            ThicknessAnimation slideInAnimation = new ThicknessAnimation
            {
                From = new Thickness(0, 20, 0, 0),
                To = new Thickness(0),
                Duration = new Duration(TimeSpan.FromSeconds(0.15))
            };


            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeInAnimation);
            storyboard.Children.Add(slideInAnimation);

            Storyboard.SetTarget(fadeInAnimation, element);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(slideInAnimation, element);
            Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath("Margin"));

            storyboard.Begin();
        }
        public void HideWithTransition(UIElement element)
        {

            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.15))
            };


            ThicknessAnimation slideOutAnimation = new ThicknessAnimation
            {
                From = new Thickness(0),
                To = new Thickness(0, 20, 0, 0),
                Duration = new Duration(TimeSpan.FromSeconds(0.15))
            };


            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeOutAnimation);
            storyboard.Children.Add(slideOutAnimation);


            Storyboard.SetTarget(fadeOutAnimation, element);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(slideOutAnimation, element);
            Storyboard.SetTargetProperty(slideOutAnimation, new PropertyPath("Margin"));


            storyboard.Completed += (s, e) =>
            {
                element.Visibility = Visibility.Collapsed;
            };


            storyboard.Begin();
        }

        public string ChooseDirectory()
        {
            string filepath;
            string systemPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.System);
            FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
            saveFileDialog.ShowDialog();
            filepath = saveFileDialog.SelectedPath;
            return filepath;
        }
        public string SaveDialog(string filename, string extension, string filter)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = filename;
            saveFileDialog1.DefaultExt = extension;
            saveFileDialog1.Filter = filter;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
            }
            else
            {

                filename = string.Empty;
            }
            return filename;
        }
    }
}
