namespace Theam.Models;

public class CategoryModel
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string? ImgPath { get; set; }
}

public class CategoryGetModel
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; } // Ensure property name matches API
}
