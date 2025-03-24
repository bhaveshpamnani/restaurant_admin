using System.ComponentModel.DataAnnotations;

namespace Theam.Models;

public class CategoryModel
{
    public int CategoryID { get; set; }
    [Required(ErrorMessage = "Category Name is required.")]
    [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters.")]
    public string CategoryName { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Please upload an image.")]
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
