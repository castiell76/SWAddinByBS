using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SWApp.Controls
{
    [ContentProperty(nameof(ExampleContent))]
    public class ControlExample : Control
    {
        /// <summary>Identifies the <see cref="HeaderText"/> dependency property.</summary>
        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(
            nameof(HeaderText),
            typeof(string),
            typeof(ControlExample),
            new PropertyMetadata(null)
        );

        /// <summary>Identifies the <see cref="ExampleContent"/> dependency property.</summary>
        public static readonly DependencyProperty ExampleContentProperty = DependencyProperty.Register(
            nameof(ExampleContent),
            typeof(object),
            typeof(ControlExample),
            new PropertyMetadata(null)
        );

        public string? HeaderText
        {
            get => (string?)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        public object? ExampleContent
        {
            get => GetValue(ExampleContentProperty);
            set => SetValue(ExampleContentProperty, value);
        }

        private static string LoadResource(Uri? uri)
        {
            try
            {
                if (uri is null || Application.GetResourceStream(uri) is not { } steamInfo)
                {
                    return string.Empty;
                }

                using StreamReader streamReader = new(steamInfo.Stream, Encoding.UTF8);

                return streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return e.ToString();
            }
        }
    }
}
