using System;
using Wpf.Ui.Controls;

namespace SWApp.Controls;

public record NavigationCard
{
    public string? Name { get; init; }

    public SymbolRegular Icon { get; init; }

    public string? Description { get; init; }

    public Type? PageType { get; init; }
}

