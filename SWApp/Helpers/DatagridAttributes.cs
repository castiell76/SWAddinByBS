using System;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnVisibilityAttribute : Attribute
{
    public bool IsVisible { get; set; }

    public ColumnVisibilityAttribute(bool isVisible)
    {
        IsVisible = isVisible;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class ColumnNameAttribute : Attribute
{
    public string Name { get; set; }

    public ColumnNameAttribute(string name)
    {
        Name = name;
    }
}