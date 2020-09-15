using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;
using UnrealEstate.Services;

namespace UnrealEstate.Controllers
{
    public class ListingsController : Controller
    {
        private readonly IListingService _listingService;
        public ListingsController(IListingService listingService)
        {
            _listingService = listingService;
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
