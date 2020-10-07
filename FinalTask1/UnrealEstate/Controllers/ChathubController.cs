using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Controllers
{
    public class ChathubController : Controller
    {
        [HttpGet("/chathub")]
        public ActionResult ChatHub()
        {
            return View();
        }
    }
}
