using Microsoft.AspNetCore.Mvc;

namespace Theam.Controllers;

public class ManageOrdersController : Controller
{
    // GET
    public IActionResult ManageOrders()
    {
        return View();
    }
    
    public IActionResult DetailOrders()
    {
        return View();
    }
}