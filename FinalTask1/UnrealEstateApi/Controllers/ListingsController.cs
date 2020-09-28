using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Infrastructure.Models;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;
using UnrealEstate.Services;
using UnrealEstate.Services.Listing.Interface;
using UnrealEstate.Services.Listing.ViewModel.Request;
using UnrealEstate.Services.User.Interface;

namespace UnrealEstateApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IListingService _listingService;
        private readonly IUserService _userService;

        public ListingsController(IListingService listingService, ICommentService commentService,
            IUserService userService)
        {
            _listingService = listingService;
            _commentService = commentService;
            _userService = userService;
        }

        /// <summary>
        ///     Get all Listings.
        /// </summary>
        /// <returns>List of Listing.</returns>
        /// GET: api/Listings
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListingResponse>>> GetListings(
            [FromQuery] ListingFilterCriteriaRequest filterCriteria)
        {
            if (!ModelState.IsValid) return BadRequest();

            return await _listingService.GetActiveListingsWithFilterAsync(filterCriteria);
        }

        /// <summary>
        ///     Get listing by id.
        /// </summary>
        /// <param name="id">Id of listing to get.</param>
        /// <returns>Listing object if found, otherwise 404 status code.</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ListingResponse>> GetListing(int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var listing = await _listingService.GetListingAsync(id);

            if (listing == null) return NotFound();

            return Ok(listing);
        }

        // PUT: api/Listings/5
        /// <summary>
        ///     Update listing by Id.
        /// </summary>
        /// <param name="listingId">Listing id.</param>
        /// <param name="listing">Listing updated.</param>
        /// <returns>
        ///     400 status code if url id not match updated id, 204 status code if completed update, not found if id not
        ///     exists in database.
        /// </returns>
        [HttpPut("{listingId}")]
        public async Task<IActionResult> UpdateListing(int listingId, [FromForm] ListingRequest listing)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var currentUser = await GetCurrentUserAsync();

                await _listingService.EditListingAsync(currentUser, listing, listingId);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // POST: api/Listings
        /// <summary>
        ///     Create new listing.
        /// </summary>
        /// <param name="listing">Listing created.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Listing>> CreateListing([FromForm] ListingRequest listing)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var user = await GetCurrentUserAsync();

                await _listingService.CreateListingAsync(listing, user.Id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction("GetListing", listing);
        }

        // DELETE: api/Listings/5
        /// <summary>
        ///     Delete listing by id.
        /// </summary>
        /// <param name="id">Listing id to delete.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ListingResponse>> DeleteListing(int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var currentUser = await GetCurrentUserAsync();
                await _listingService.DisableListingAsync(currentUser, id);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NotSupportedException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            var listing = await _listingService.GetListingAsync(id);

            return listing;
        }

        /// <summary>
        ///     Enable the disabled listing, only available for admin user.
        /// </summary>
        /// <param name="id">Id to enable.</param>
        /// <returns>404 if not found listing, 400 if listing status is not disabled, otherwise return listing be enabled.</returns>
        [HttpPost("{id}/enable")]
        public async Task<ActionResult<ListingResponse>> EnableListing(int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var currentUser = await GetCurrentUserAsync();
                await _listingService.EnableListingAsync(currentUser, id);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            var listing = await _listingService.GetListingAsync(id);

            return listing;
        }

        // api/listings/1/comments
        /// <summary>
        ///     Gets list comments in listing by listingId.
        /// </summary>
        /// <param name="listingId">Listing id.</param>
        /// <returns>List comments if found, otherwise return not found.</returns>
        //[AllowAnonymous]
        [HttpGet("{listingId}/comments")]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetComments(int listingId)
        {
            if (!ModelState.IsValid) return BadRequest();

            var comments = await _commentService.GetCommentsByListingAsync(listingId);

            if (comments == null) return NotFound();

            return comments;
        }

        // api/1/favorite
        /// <summary>
        ///     Add listing to favorite for current user, Remove listing from favorite if user already favorited.
        /// </summary>
        /// <param name="listingId">listing id.</param>
        /// <returns></returns>
        [HttpPost("{listingId}/favorite")]
        public async Task<IActionResult> SetFavorite(int listingId)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var currentUser = await GetCurrentUserAsync();

                await _listingService.AddOrRemoveFavoriteAsync(listingId, currentUser.Id);

                return NoContent();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///     Make a bid on specified listing. Only available for logged in user.
        /// </summary>
        /// <param name="listingId"></param>
        /// <param name="bidRequestViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{listingId}/bid")]
        public async Task<IActionResult> MakeABid(int listingId, ListingBidRequest bidRequestViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var user = await GetCurrentUserAsync();

                await _listingService.MakeABid(listingId, user, bidRequestViewModel);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(bidRequestViewModel);
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var user = await _userService.GetUserByEmailAsync(email);
            return user;
        }
    }
}