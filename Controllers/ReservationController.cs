using Microsoft.AspNetCore.Mvc;

namespace Theam.Controllers;

public class ReservationController : Controller
{
    // GET
    public IActionResult ReservationList()
    {
        return View();
    }
}