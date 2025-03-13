namespace Theam.Models;

public class InventoryModel
{
    public int? InventoryID { get; set; }
    public string ItemName { get; set; }
    public string ImageURL { get; set; }
    public decimal QuantityAvailable { get; set; }
    public decimal QuantityWanted { get; set; }
}

public class InventoryEditModel
{
    public int InventoryID { get; set; }
    public string ItemName { get; set; }
    public int QuantityAvailable { get; set; }
    public int QuantityWanted { get; set; }
    public IFormFile? ImageURL { get; set; }
    public string? ImageString { get; set; }  // For existing image
}


public class InventoryCreateModel
{
    public int? InventoryID { get; set; }
    public string ItemName { get; set; }
    public IFormFile? ImageURL { get; set; }
    public decimal QuantityAvailable { get; set; }
    public decimal QuantityWanted { get; set; }
}

