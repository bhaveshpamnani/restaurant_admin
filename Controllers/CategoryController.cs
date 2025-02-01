using Microsoft.AspNetCore.Mvc;

namespace Theam.Controllers;

public class CategoryController : Controller
{
    // GET
    public IActionResult CategoryList()
    {
        return View();
    }
    
    public IActionResult CategoryForm()
    {
        return View();
    }
}