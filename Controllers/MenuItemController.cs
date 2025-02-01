using Microsoft.AspNetCore.Mvc;

namespace Theam.Controllers;

public class MenuItemController : Controller
{
    // GET
    public IActionResult MenuItemList()
    {
        return View();
    }

    public IActionResult AddForm()
    {
        return View();
    }
}