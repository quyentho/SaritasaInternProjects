using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Services;
using UnrealEstate.Services.User.Interface;

namespace UnrealEstate.Controllers
{
    [Authorize(Roles = UserRole.Admin)]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ActionResult> Users(UserFilterCriteriaRequest filterCriteria)
        {
            if (ModelState.IsValid)
            {
                SetPaging(filterCriteria);

                var users =
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
            if (ModelState.IsValid) await _userService.SetUserStatusAsync(email);
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