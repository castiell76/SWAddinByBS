using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Input;
using Color = System.Windows.Media.Color;


namespace SWApp.Controls
{
    /// <summary>
    /// Interaction logic for CodeBlock.xaml
    /// </summary>
    public class CodeBlock : ContentControl
    {
        private string _sourceCode = string.Empty;

        /// <summary>
        /// Property for <see cref="SyntaxContent"/>.
        /// </summary>
        public static readonly DependencyProperty SyntaxContentProperty = DependencyProperty.Register(
            nameof(SyntaxContent),
            typeof(object),
            typeof(CodeBlock),
            new PropertyMetadata(null)
        );

        /// <summary>
        /// Property for <see cref="ButtonCommand"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.Register(
            nameof(ButtonCommand),
            typeof(IRelayCommand),
            typeof(CodeBlock)
        );

        /// <summary>
        /// Formatted <see cref="System.Windows.Controls.ContentControl.Content"/>.
        /// </summary>
        public object SyntaxContent
        {
            get => GetValue(SyntaxContentProperty);
            internal set => SetValue(SyntaxContentProperty, value);
        }

        /// <summary>
        /// Command triggered after clicking the control button.
        /// </summary>
        public IRelayCommand ButtonCommand => (IRelayCommand)GetValue(ButtonCommandProperty);

        /// <summary>
        /// Creates new instance and assigns <see cref="ButtonCommand"/> default action.
        /// </summary>
        public CodeBlock()
        {
            SetValue(ButtonCommandProperty, new RelayCommand<string>(OnTemplateButtonClick));

            ApplicationThemeManager.Changed += ThemeOnChanged;
        }

        private void ThemeOnChanged(ApplicationTheme currentApplicationTheme, Color systemAccent)
        {
            UpdateSyntax();
        }

        /// <summary>
        /// This method is invoked when the Content property changes.
        /// </summary>
        /// <param name="oldContent">The old value of the Content property.</param>
        /// <param name="newContent">The new value of the Content property.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            UpdateSyntax();
        }

        protected virtual void UpdateSyntax()
        {
            _sourceCode = Highlighter.Clean(Content as string ?? string.Empty);

            var richTextBox = new Wpf.Ui.Controls.RichTextBox()
            {
                IsTextSelectionEnabled = true,
                VerticalContentAlignment = VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Left
            };

            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(Highlighter.FormatAsParagraph(_sourceCode));

            SyntaxContent = richTextBox;
        }

        private void OnTemplateButtonClick(string? _)
        {
            Debug.WriteLine($"INFO | CodeBlock source: \n{_sourceCode}", "SWApp.CodeBlock");

            try
            {
                Clipboard.Clear();
                Clipboard.SetText(_sourceCode);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
