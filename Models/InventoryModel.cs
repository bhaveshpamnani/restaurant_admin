using System.ComponentModel.DataAnnotations;

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
    public IFormFile? Image { get; set; }
    public string? ImageURL { get; set; }  // For existing image
}


public class InventoryCreateModel
{
    public int? InventoryID { get; set; }
    [Required(ErrorMessage = "Item Name is required.")]
    [StringLength(100, ErrorMessage = "Item Name cannot exceed 100 characters.")]
    public string ItemName { get; set; }

    [Required(ErrorMessage = "Please upload an image.")]
    public IFormFile? ImageURL { get; set; }

    [Required(ErrorMessage = "Quantity Available is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Quantity Available must be a positive number.")]
    public decimal QuantityAvailable { get; set; }

    [Required(ErrorMessage = "Quantity Wanted is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Quantity Wanted must be a positive number.")]
    public decimal QuantityWanted { get; set; }
}

