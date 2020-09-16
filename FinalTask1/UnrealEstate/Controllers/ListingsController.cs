using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;
using UnrealEstate.Services;

namespace UnrealEstate.Controllers
{
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
                ModelState.AddModelError("error",ex.Message);
            }

            return View(listingRequest);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var listingResponse = await _listingService.GetListingAsync(id);
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
                var userId = HttpContext.User.Identity.GetUserId();
                await _listingService.CreateListingAsync(listingRequest, userId);
            }

            return View();
        }

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
