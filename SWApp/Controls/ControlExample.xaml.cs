﻿using System;
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
    /// <summary>
    /// Interaction logic for ControlExample.xaml
    /// </summary>
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

        ///// <summary>Identifies the <see cref="XamlCode"/> dependency property.</summary>
        //public static readonly DependencyProperty XamlCodeProperty = DependencyProperty.Register(
        //    nameof(XamlCode),
        //    typeof(string),
        //    typeof(ControlExample),
        //    new PropertyMetadata(null)
        //);

        ///// <summary>Identifies the <see cref="XamlCodeSource"/> dependency property.</summary>
        //public static readonly DependencyProperty XamlCodeSourceProperty = DependencyProperty.Register(
        //    nameof(XamlCodeSource),
        //    typeof(Uri),
        //    typeof(ControlExample),
        //    new PropertyMetadata(
        //        null,
        //        static (o, args) =>
        //        {
        //            ((ControlExample)o).OnXamlCodeSourceChanged((Uri?)args.NewValue);
        //        }
        //    )
        //);

        ///// <summary>Identifies the <see cref="CsharpCode"/> dependency property.</summary>
        //public static readonly DependencyProperty CsharpCodeProperty = DependencyProperty.Register(
        //    nameof(CsharpCode),
        //    typeof(string),
        //    typeof(ControlExample),
        //    new PropertyMetadata(null)
        //);

        ///// <summary>Identifies the <see cref="CsharpCodeSource"/> dependency property.</summary>
        //public static readonly DependencyProperty CsharpCodeSourceProperty = DependencyProperty.Register(
        //    nameof(CsharpCodeSource),
        //    typeof(Uri),
        //    typeof(ControlExample),
        //    new PropertyMetadata(
        //        null,
        //        static (o, args) =>
        //        {
        //            ((ControlExample)o).OnCsharpCodeSourceChanged((Uri?)args.NewValue);
        //        }
        //    )
        //);

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

        //public string? XamlCode
        //{
        //    get => (string?)GetValue(XamlCodeProperty);
        //    set => SetValue(XamlCodeProperty, value);
        //}

        //public Uri? XamlCodeSource
        //{
        //    get => (Uri?)GetValue(XamlCodeSourceProperty);
        //    set => SetValue(XamlCodeSourceProperty, value);
        //}

        //public string? CsharpCode
        //{
        //    get => (string?)GetValue(CsharpCodeProperty);
        //    set => SetValue(CsharpCodeProperty, value);
        //}

        //public Uri? CsharpCodeSource
        //{
        //    get => (Uri?)GetValue(CsharpCodeSourceProperty);
        //    set => SetValue(CsharpCodeSourceProperty, value);
        //}

        //private void OnXamlCodeSourceChanged(Uri? uri)
        //{
        //    SetCurrentValue(XamlCodeProperty, LoadResource(uri));
        //}

        //private void OnCsharpCodeSourceChanged(Uri? uri)
        //{
        //    SetCurrentValue(CsharpCodeProperty, LoadResource(uri));
        //}

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
