namespace Theam.Models;

public class MenuModel
{
    public int? MenuID { get; set; }
    public int CategoryID { get; set; }
    public string DishName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public IFormFile? ImagePath { get; set; }
    public int Rating { get; set; }
    public bool AvailabilityStatus { get; set; }
}

public class GetMenuModel
{
    public int MenuID { get; set; }
    public int CategoryID { get; set; }
    public string DishName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageURL { get; set; }
    public bool AvailabilityStatus { get; set; }
    public string CategoryName { get; set; }
    public int Rating { get; set; }
}

public class MenuEditModel
{
    public int? MenuID { get; set; }
    public int CategoryID { get; set; }
    public int? Rating { get; set; }
    public string DishName { get; set; }
    public string Description { get; set; }
    public string? ImageString { get; set; }
    public decimal Price { get; set; }
    public IFormFile? ImagePath { get; set; }
    public bool AvailabilityStatus { get; set; }
}