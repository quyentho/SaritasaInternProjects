using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Services;

namespace UnrealEstate.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: AdminController
        public async Task<ActionResult> Users(UserFilterCriteriaRequest filterCriteria)
        {
            if (ModelState.IsValid)
            {

                SetPaging(filterCriteria);

                List<UserResponse> users =
                    await _userService.GetActiveUsersWithFilterAsync(filterCriteria);


                ViewData["Criteria"] = filterCriteria;

                return View(users);
            }

            return View(null);
        }

        [HttpGet]
        [Route("{email}")]
        public async Task<IActionResult> SetStatus(string email)
        {
            if (ModelState.IsValid)
            {
                await _userService.SetUserStatusAsync(email);
            }
            return RedirectToAction(nameof(Users));
        }

        private static void SetPaging(UserFilterCriteriaRequest filterCriteria)
        {
            if (filterCriteria.Offset is null)
            {
                filterCriteria.Offset = 0;
                filterCriteria.Limit = 5;
            }
            else
            {
                filterCriteria.Offset += 5;
            }
        }
    }
}
