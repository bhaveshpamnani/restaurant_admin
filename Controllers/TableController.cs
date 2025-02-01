using Microsoft.AspNetCore.Mvc;

namespace Theam.Controllers;

public class TableController : Controller
{
    // GET
    public IActionResult TableList()
    {
        return View();
    }
    
    public IActionResult AddDinningTableForm()
    {
        return View();
    }
}