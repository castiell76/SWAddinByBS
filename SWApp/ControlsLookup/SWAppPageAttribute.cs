using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace SWApp.ControlsLookup;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class SWAppPageAttribute(string description, SymbolRegular icon) : Attribute
{
    public string Description { get; } = description;

    public SymbolRegular Icon { get; } = icon;
}
