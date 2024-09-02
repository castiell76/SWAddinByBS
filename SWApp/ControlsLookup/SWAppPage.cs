using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace SWApp.ControlsLookup;

internal record SWAppPage(string Name, string Description, SymbolRegular Icon, Type PageType);
