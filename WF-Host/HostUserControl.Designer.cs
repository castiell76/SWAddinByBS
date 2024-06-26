using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using SWApp; 

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
            elementHost2.Location = new Point(0, 0);
            elementHost2.Margin = new Padding(4, 3, 4, 3);
            elementHost2.Name = "elementHost2";
            elementHost2.TabIndex = 1;
            elementHost2.Child = mainWindow2;
           
            // HostUserControl
            AccessibleRole = AccessibleRole.ScrollBar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(elementHost2);
            ForeColor = SystemColors.Control;
            Margin = new Padding(4, 3, 4, 3);
            Name = "HostUserControl";
            Size = new Size(646, 721);

            ResumeLayout(false);
        }
    }
}