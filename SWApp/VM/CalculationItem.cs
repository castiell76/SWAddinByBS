using SWApp.Models;
using SWApp.VM;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

public class CalculationItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int ID { get; set; }

    // Właściwości specyficzne dla klasy Operation
    public string OperationType { get; set; }
    public double Rate { get; set; }
    public double Time { get; set; }
    public double PricePerItem { get; set; }
    public int QuantityPerItem { get; set; }
    public double TPZ { get; set; }
    public string Unit { get; set; }

    // Właściwości specyficzne dla klasy Material
    public List<SWTreeNode> Nodes { get; set; }
    public decimal PricePerSet { get; set; }

    // Konstruktor dla klasy Material
    public CalculationItem(SWApp.Models.Material material)
    {
        Name = material.Name;
        Price = material.Price;
        Description = material.Description;
        ID = material.ID;

        // Przypisz pozostałe właściwości specyficzne dla klasy Material
        Nodes = material.Nodes;
        PricePerSet = material.PricePetSet;
    }

    // Konstruktor dla klasy Operation
    public CalculationItem(Operation operation)
    {
        Name = operation.Name;
        Price = 0; // Cena może być ustawiona na zero lub inną wartość, zależnie od potrzeb
        Description = string.Empty; // Opis może być pusty lub inny, zależnie od potrzeb
        ID = 0; // ID może być ustawione na zero lub inną wartość, zależnie od potrzeb

        // Przypisz pozostałe właściwości specyficzne dla klasy Operation
        OperationType = operation.Type;
        Rate = operation.Rate;
        Time = operation.Time;
        QuantityPerItem = operation.QuantityPerItem;
        TPZ = operation.TPZ;
        Unit = operation.Unit;

        // Oblicz cenę za element (uwzględniając logikę z Property PricePerItem w klasie Operation)
        PricePerItem = operation.PricePerItem;
    }
}