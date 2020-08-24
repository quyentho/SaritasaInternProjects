using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OmdbScrubber.Models;

namespace OmdbScrubber.Controllers
{
    public class ImdbController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View(new InputVM()); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(InputVM input)
        {
            return RedirectToAction(nameof(Index), nameof(HomeController));
        }
    }
}
