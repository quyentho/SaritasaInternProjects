using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Business.Comment.Service;
using UnrealEstate.Business.Comment.ViewModel;
using UnrealEstate.Business.Listing.Service;
using UnrealEstate.Business.Listing.ViewModel;
using UnrealEstate.Business.User.Service;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ListingsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IListingService _listingService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ListingsController(IListingService listingService, IMapper mapper, IUserService userService,
            ICommentService commentService)
        {
            _listingService = listingService;
            _mapper = mapper;
            _userService = userService;
            _commentService = commentService;
        }

        [Route("{listingId}/favorite")]
        [HttpGet]
        public async Task<IActionResult> Favorite(int listingId, ListingFilterCriteriaRequest filterCriteria)
        {
            try
            {
                var user = await GetCurrentUser();

                await _listingService.AddOrRemoveFavoriteAsync(listingId, user.Id);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Search), filterCriteria);
        }

        [HttpGet]
        [Route("{listingId}/Disable")]
        public async Task<IActionResult> Disable(int listingId, string returnUrl)
        {
            try
            {
                var currentUser = await GetCurrentUser();

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

            return RedirectToAction("Edit", new {id = listingId, returnUrl});
        }

        [Route("MakeABid")]
        [HttpGet]
        public IActionResult Bid(int listingId, string returnUrl)
        {
            var bidRequest = new ListingBidRequest
            {
                ListingId = listingId
            };

            ViewData["returnUrl"] = returnUrl;
            return View(bidRequest);
        }

        [Route("MakeABid")]
        [HttpPost]
        public async Task<IActionResult> Bid(ListingBidRequest bidRequest, string returnUrl)
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

        private async Task<ApplicationUser> GetCurrentUser()
        {
            var currentUser = await
                _userService.GetUserByEmailAsync(User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)
                    ?.Value);
            return currentUser;
        }

        [HttpGet]
        [Route("{listingId}/edit")]
        public async Task<IActionResult> Edit(int listingId, string returnUrl)
        {
            // Error message from edit HttpPost method.
            if (TempData["errorMessage"] != null)
            {
                foreach (var errorMessage in (IEnumerable<string>) TempData["errorMessage"])
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            var listingResponse = await _listingService.GetListingAsync(listingId);

            var listingRequest = _mapper.Map<ListingRequest>(listingResponse);

            ViewData["returnUrl"] = returnUrl;
            ViewData["photos"] = listingResponse.ListingPhoTos;
            ViewData["listingId"] = listingId;

            return View(listingRequest);
        }

        [HttpPost]
        [Route("{listingId}/edit")]
        public async Task<IActionResult> Edit(ListingRequest listingRequest, int listingId, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await GetCurrentUser();

                    await _listingService.EditListingAsync(user, listingRequest, listingId);

                    return RedirectToAction("Detail", new {listingId, returnUrl});
                }
            }
            catch (NotSupportedException ex) // Not have permission
            {
                TempData["errorMessage"] = ex.Message; // BUG: lost message because redirect

                return LocalRedirect(returnUrl);
            }
            catch (InvalidOperationException ex) // Photos Exceed 3.
            {
                TempData["errorMessage"] = ex.Message;

                return RedirectToAction("Edit", new {listingId, returnUrl});
            }

            var errors = ModelState.SelectMany(x => x.Value.Errors).Select(e => e.ErrorMessage).ToList();

            TempData["errorMessage"] = errors;

            return RedirectToAction("Edit", new {listingId, returnUrl});
        }

        [AllowAnonymous]
        [Route("{listingId}")]
        [HttpGet]
        public async Task<IActionResult> Detail(int listingId, [FromQuery] string returnUrl)
        {
            // Error from other actions occur when performs action on detail view.
            if (TempData["errorMessage"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["errorMessage"].ToString());
            }

            var listingResponse = await _listingService.GetListingAsync(listingId);

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
                    var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                    var currentUser = await _userService.GetUserByEmailAsync(userEmail);

                    await _listingService.CreateListingAsync(listingRequest, currentUser.Id);
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
            if (TempData["errorMessage"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["errorMessage"].ToString());
            }

            if (!ModelState.IsValid)
            {
                TempData["errorMessage"] = "Fail to get listings";

                return RedirectToAction(nameof(Index), "Home");
            }

            if (filterCriteria.Offset is null)
            {
                SetPaging(filterCriteria);
            }

            var listingResponses =
                await _listingService.GetActiveListingsWithFilterAsync(filterCriteria);


            ViewData["filterCriteria"] = filterCriteria;

            return View(listingResponses);
        }

        private void SetPaging(ListingFilterCriteriaRequest filterCriteria)
        {
            filterCriteria.Offset = 0;
            filterCriteria.Limit = 3;
        }

        [AllowAnonymous]
        [Route("NextPage")]
        public IActionResult GoNextPage(ListingFilterCriteriaRequest filterCriteria)
        {
            filterCriteria.Offset += 3;

            return RedirectToAction(nameof(Search), filterCriteria);
        }

        [AllowAnonymous]
        [Route("PreviousPage")]
        public IActionResult BackPreviousPage(ListingFilterCriteriaRequest filterCriteria)
        {
            filterCriteria.Offset -= 3;

            return RedirectToAction(nameof(Search), filterCriteria);
        }
    }
}