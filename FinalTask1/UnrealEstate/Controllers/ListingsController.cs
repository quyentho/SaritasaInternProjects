using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;
using UnrealEstate.Services;

namespace UnrealEstate.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ListingsController : Controller
    {
        private readonly IListingService _listingService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;

        public ListingsController(IListingService listingService, IMapper mapper, IUserService userService, ICommentService commentService)
        {
            _listingService = listingService;
            _mapper = mapper;
            _userService = userService;
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> Favorite(int listingId, string returnUrl)
        {
            try
            {
                User user = await GetCurrentUser();
                bool isFavorite = await _listingService.AddOrRemoveFavoriteAsync(listingId, user.Id);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("{listingId}/UpdateComment/{commentId}")]
        public async Task<IActionResult> UpdateComment(CommentRequest commentRequest, int commentId, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                TempData["errorMessage"] = "Error when attempt to edit comment";

                return RedirectToAction(nameof(Detail), new { id = commentRequest.ListingId, returnUrl });
            }

            try
            {
                User currentUser = await GetCurrentUser();
                await _commentService.EditCommentAsync(currentUser.Id, commentRequest, commentId);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (NotSupportedException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Detail), new { id = commentRequest.ListingId, returnUrl });

        }

        [HttpGet]
        [Route("{listingId}/AddComment")]
        public async Task<IActionResult> AddComment(CommentRequest commentRequest, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User currentUser = await GetCurrentUser();

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

            return RedirectToAction(nameof(Detail), new { id = commentRequest.ListingId, returnUrl });
        }

        [HttpGet]
        [Route("{listingId}/DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId, int listingId, string returnUrl)
        {
            try
            {
                User currentUser = await GetCurrentUser();

                await _commentService.DeleteCommentAsync(currentUser, commentId);
            }
            catch (NotSupportedException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Detail), new { id = listingId, returnUrl });
        }

        [HttpGet]
        [Route("{listingId}/Disable")]
        public async Task<IActionResult> Disable(int listingId, string returnUrl)
        {
            try
            {
                User currentUser = await GetCurrentUser();

                await _listingService.DisableListingAsync(currentUser, listingId);

                return LocalRedirect(returnUrl);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return NotFound(e.Message);
            }
            catch (NotSupportedException e)
            {
                return Forbid(e.Message);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return LocalRedirect(returnUrl);
            }

        }

        [HttpGet]
        [Route("{listingId}/DeletePhoto/{photoId}")]
        public async Task<IActionResult> DeleteListingPhoto(int listingId, int photoId, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await GetCurrentUser();
                    await _listingService.DeletePhotoAsync(currentUser, listingId, photoId);

                }
                catch (NotSupportedException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return RedirectToAction("Edit", new { id = listingId, returnUrl });
        }

        [Route("MakeABid")]
        [HttpGet]
        public IActionResult Bid(int listingId, string returnUrl)
        {
            BidRequest bidRequest = new BidRequest()
            {
                ListingId = listingId
            };

            ViewData["returnUrl"] = returnUrl;
            return View(bidRequest);
        }

        [Route("MakeABid")]
        [HttpPost]
        public async Task<IActionResult> Bid(BidRequest bidRequest, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await GetCurrentUser();

                try
                {
                    await _listingService.MakeABid(bidRequest.ListingId, currentUser, bidRequest);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);

                    return View(bidRequest);
                }

                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid bid attempt");
            return View(bidRequest);
        }

        private async Task<User> GetCurrentUser()
        {
            var currentUser = await
                _userService.GetUserByEmailAsync(User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)
                    ?.Value);
            return currentUser;
        }

        [HttpGet]
        [Route("{id}/edit")]
        public async Task<IActionResult> Edit(int id, string returnUrl)
        {
            // Error message from edit HttpPost method.
            if (TempData["errorMessage"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["errorMessage"].ToString());
            }

            var listingResponse = await _listingService.GetListingAsync(id);

            var listingRequest = _mapper.Map<ListingRequest>(listingResponse);

            ViewData["returnUrl"] = returnUrl;
            ViewData["photos"] = listingResponse.ListingPhoTos;
            ViewData["listingId"] = id;

            return View(listingRequest);
        }

        [HttpPost]
        [Route("{id}/edit")]
        public async Task<IActionResult> Edit(ListingRequest listingRequest, int id, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userService.GetUserByEmailAsync(User.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.Email)
                        ?.Value);
                    await _listingService.EditListingAsync(user, listingRequest, id);
                }
            }
            catch (NotSupportedException ex) // Not have permission
            {
                TempData["errorMessage"] = ex.Message;

                return LocalRedirect(returnUrl);
            }
            catch (InvalidOperationException ex) // Photos Exceed 3.
            {
                TempData["errorMessage"] = ex.Message;

                return RedirectToAction("Edit", new { id, returnUrl });
            }

            return RedirectToAction("Detail", new { id, returnUrl });
        }

        [AllowAnonymous]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id, string returnUrl)
        {
            // Error from other actions occur when performs action on detail view.
            if (TempData["errorMessage"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["errorMessage"].ToString());
            }

            ListingResponse listingResponse = await _listingService.GetListingAsync(id);

            ViewData["returnUrl"] = returnUrl;

            return View(listingResponse);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(ListingRequest listingRequest)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = HttpContext.User.Identity.GetUserId();
                    await _listingService.CreateListingAsync(listingRequest, userId);
                }
                catch (InvalidOperationException exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View();
        }

        [AllowAnonymous]
        [Route("Search")]
        [HttpGet]
        public async Task<IActionResult> Search(ListingFilterCriteriaRequest filterCriteria)
        {
            if (ModelState.IsValid)
            {
                SetPaging(filterCriteria);

                List<ListingResponse> listingResponses =
                    await _listingService.GetActiveListingsWithFilterAsync(filterCriteria);


                ViewData["Criteria"] = filterCriteria;

                return View(listingResponses);
            }

            ModelState.AddModelError(string.Empty, "Fail to get listings");
            return View(null);
        }

        private static void SetPaging(ListingFilterCriteriaRequest filterCriteria)
        {
            if (filterCriteria.Offset is null)
            {
                filterCriteria.Offset = 0;
                filterCriteria.Limit = 3;
            }
            else
            {
                filterCriteria.Offset += 3;
            }
        }
    }
}
