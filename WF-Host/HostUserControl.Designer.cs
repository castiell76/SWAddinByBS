using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using SWApp.Views;
using Microsoft.Extensions.Configuration;
using SWApp.Services;
using SWApp.Services.Contracts;
using SWApp.Viewmodels;
using Wpf.Ui;
using System.Windows.Navigation;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Threading;

namespace WF_Host
{
    [ComVisible(true)]
    [Guid("8F1992E8-2E0D-4110-BC7B-3D9400BB932B")]
    partial class HostUserControl : UserControl
    {
        private System.ComponentModel.IContainer components = null;
        private ElementHost elementHost2;
        private MainWindow mainWindow2;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            elementHost2 = new ElementHost();
            mainWindow2 = new MainWindow();

            SuspendLayout();

            // elementHost2
            elementHost2.Dock = DockStyle.Fill;
            elementHost2.Location = new System.Drawing.Point(0, 0);
            elementHost2.Margin = new Padding(4, 3, 4, 3);
            elementHost2.Name = "elementHost2";
            elementHost2.TabIndex = 1;
            elementHost2.Child = mainWindow2;
           
            // HostUserControl
            AccessibleRole = AccessibleRole.ScrollBar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            Controls.Add(elementHost2);
            ForeColor = Color.Transparent;
            BackColor = Color.Transparent;
            Margin = new Padding(4, 3, 4, 3);
            Name = "HostUserControl";
            Size = new System.Drawing.Size(630, 800);
            
            ResumeLayout(false);
        }
        private void OnThemeChanged(object sender, bool isDarkTheme)
        {
            elementHost2.BackColor = Color.FromArgb(62, 62, 62);
        }

    }
}
