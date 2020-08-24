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
            // TODO: upgrade better view;
            return View(new InputVM()); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(InputVM input)
        {
            // TODO: Validate input and return back the message.
            if (!ModelState.IsValid)
            {
                return RedirectToAction();
            }

            var imdbIds = input.UserInput.Split(",", StringSplitOptions.RemoveEmptyEntries);
            return RedirectToAction(nameof(List));
        }

        public IActionResult List(string[] imdbIds)
        {
            return View();
        }
    }
}
