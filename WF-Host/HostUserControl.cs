using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WF_Host
{
    [ProgId(Server.SWTASKPANE_PROGID)]
    public partial class HostUserControl : UserControl
    {
        public HostUserControl()
        {
            InitializeComponent();
            mainWindow2.ThemeChanged += OnThemeChanged;
        }

    }
}
