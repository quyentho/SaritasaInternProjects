using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Business.Comment.Service;
using UnrealEstate.Business.Listing.Service;
using UnrealEstate.Business.Listing.ViewModel;
using UnrealEstate.Business.User.Service;
using UnrealEstate.Business.Utils;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Controllers
{
    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> Favorite(int id, ListingFilterCriteriaRequest filterCriteria)
        {
            try
            {
                var user = await  HttpContextHelper.GetCurrentUserAsync(HttpContext, _userService);

                await _listingService.AddOrRemoveFavoriteAsync(id, user.Id);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Search), filterCriteria);
        }

        [HttpGet]
        public async Task<IActionResult> Disable(int id, string returnUrl)
        {
            try
            {
                var currentUser = await  HttpContextHelper.GetCurrentUserAsync(HttpContext, _userService);

                await _listingService.DisableListingAsync(currentUser, id);

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
        public IActionResult Bid(int id, string returnUrl)
        {
            var bidRequest = new ListingBidRequest
            {
                ListingId = id
            };

            ViewData["returnUrl"] = returnUrl;
            return View(bidRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Bid(ListingBidRequest bidRequest, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await  HttpContextHelper.GetCurrentUserAsync(HttpContext, _userService);

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

        [HttpGet]
        public async Task<IActionResult> Edit(int id, string returnUrl)
        {
            // Error message from edit HttpPost method.
            if (TempData["errorMessage"] != null)
            {
                var errors = TempData["errorMessage"].ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            var listingResponse = await _listingService.GetListingAsync(id);

            var listingRequest = _mapper.Map<ListingRequest>(listingResponse);

            ViewData["returnUrl"] = returnUrl;
            ViewData["photos"] = listingResponse.ListingPhoTos;
            ViewData["listingId"] = id;

            return View(listingRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ListingRequest listingRequest, int id, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await  HttpContextHelper.GetCurrentUserAsync(HttpContext, _userService);

                    await _listingService.EditListingAsync(user, listingRequest, id);

                    return RedirectToAction("Detail", new { id, returnUrl });
                }
                catch (NotSupportedException ex) // Not have permission
                {
                    TempData["errorMessage"] = ex.Message; // BUG: lost message because redirect

                    return LocalRedirect(returnUrl);
                }
                catch (InvalidOperationException ex) // Photos Exceed 3.
                {
                    TempData["errorMessage"] = ex.Message;

                    return RedirectToAction("Edit", new { id, returnUrl });
                }
            }

            var errors = ModelState
                .SelectMany(x => x.Value.Errors)
                .Select(e => e.ErrorMessage)
                .Aggregate((current, next) => current + "," + next);

            TempData["errorMessage"] = errors;

            return RedirectToAction("Edit", new { id, returnUrl });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Detail(int id, [FromQuery] string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
               returnUrl = Url.Action(nameof(Search));
            }

            // Error from other actions occur when performs action on detail view.
            if (TempData["errorMessage"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["errorMessage"].ToString());
            }

            var listingResponse = await _listingService.GetListingAsync(id);

            ViewData["returnUrl"] = returnUrl;

            return View(listingResponse);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

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

            // TODO: Find a way to redirect to detail. For now it cannot because doesn't have the created listing id.
            return RedirectToAction(nameof(Index), "Home");
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
        public IActionResult GoNextPage(ListingFilterCriteriaRequest filterCriteria)
        {
            filterCriteria.Offset += 3;

            return RedirectToAction(nameof(Search), filterCriteria);
        }

        [AllowAnonymous]
        public IActionResult BackPreviousPage(ListingFilterCriteriaRequest filterCriteria)
        {
            filterCriteria.Offset -= 3;

            return RedirectToAction(nameof(Search), filterCriteria);
        }
    }
}