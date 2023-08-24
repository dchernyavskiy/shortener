using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shortener.WebUI.Controllers;

public class AboutController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new { Description = "Description" });
    }
}