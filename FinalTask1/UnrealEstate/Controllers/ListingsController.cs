using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public ListingsController(IListingService listingService, IMapper mapper, IUserService userService)
        {
            _listingService = listingService;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Disable(int listingId, string returnUrl)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                await _listingService.DisableListingAsync(currentUser, listingId);
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

            return LocalRedirect(returnUrl);
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
        public async Task<IActionResult> Edit(int id)
        {
            var listingResponse = await _listingService.GetListingAsync(id);

            var listingRequest = _mapper.Map<ListingRequest>(listingResponse);

            return View(listingRequest);
        }

        [HttpPost]
        [Route("{id}/edit")]
        public async Task<IActionResult> Edit(ListingRequest listingRequest, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userService.GetUserByEmailAsync(User.Claims.
                        FirstOrDefault(c => c.Type == ClaimTypes.Email)
                        ?.Value);
                    await _listingService.EditListingAsync(user, listingRequest, id);
                }
            }
            catch (NotSupportedException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(listingRequest);
        }

        [AllowAnonymous]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id, string returnUrl)
        {
            ListingResponse listingResponse = await _listingService.GetListingAsync(id);
            
            // TODO: REmove
            //ViewBag.Image = base.File(listingResponse.ListingPhoTos.FirstOrDefault()?.PhotoUrl, "image/jpeg");

            ViewBag.ReturnUrl = returnUrl;

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
                var userId = HttpContext.User.Identity.GetUserId();
                await _listingService.CreateListingAsync(listingRequest, userId);
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
