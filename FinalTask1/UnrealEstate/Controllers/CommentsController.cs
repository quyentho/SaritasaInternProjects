using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Business.Comment.Service;
using UnrealEstate.Business.Comment.ViewModel;
using UnrealEstate.Business.User.Service;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;
        public CommentsController(IUserService userService, ICommentService commentService)
        {
            _userService = userService;
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CommentRequest commentRequest, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                TempData["errorMessage"] = "Error when attempt to edit comment";

                return RedirectToAction("Detail", "Listings", new { id = commentRequest.ListingId, returnUrl });
            }

            try
            {
                var currentUser = await GetCurrentUser();

                await _commentService.EditCommentAsync(currentUser.Id, commentRequest, id);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (NotSupportedException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return RedirectToAction("Detail", "Listings", new { id = commentRequest.ListingId, returnUrl });
        }

        [HttpGet]
        public async Task<IActionResult> Create([FromForm]CommentRequest commentRequest, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await GetCurrentUser();

                    await _commentService.CreateCommentAsync(currentUser.Id, commentRequest);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    TempData["errorMessage"] = ex.Message;
                }
                catch (InvalidOperationException ex)
                { 
                    TempData["errorMessage"] = ex.Message;
                }
            }

            return RedirectToAction("Detail", "Listings", new { id = commentRequest.ListingId, returnUrl });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, int listingId, string returnUrl)
        {
            try
            {
                var currentUser = await GetCurrentUser();

                await _commentService.DeleteCommentAsync(currentUser, id);
            }
            catch (NotSupportedException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return RedirectToAction("Detail", "Listings", new { id = listingId, returnUrl });
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            var currentUser = await
                _userService.GetUserByEmailAsync(User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)
                    ?.Value);
            return currentUser;
        }
    }
}
