using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UnrealEstate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (TempData["errorMessage"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["errorMessage"].ToString());
            }
            return View();
        }
    }
}