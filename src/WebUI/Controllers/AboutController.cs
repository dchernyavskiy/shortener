using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shortener.WebUI.Controllers;

public class AboutController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}

public class AboutModel : PageModel
{
    [BindProperty]
    public string Description { get; set; } = "Default description of the algorithm.";

    public void OnGet()
    {
        // You can fetch the description from a database or a configuration file here
    }

    public void OnPost()
    {
        if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
        {
            // Save the edited description to the database or configuration file
        }
    }
}