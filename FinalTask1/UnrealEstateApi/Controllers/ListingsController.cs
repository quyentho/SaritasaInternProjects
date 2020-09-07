using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnrealEstate.Models;
using UnrealEstate.Services;

namespace UnrealEstateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly IListingService _listingService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public ListingsController(IListingService listingService,ICommentService commentService, UserManager<User> userManager)
        {
            _listingService = listingService;
            _commentService = commentService;
            _userManager = userManager;
        }

        /// <summary>
        /// Get all Listings.
        /// </summary>
        /// <returns>List of Listing.</returns>
        // GET: api/Listings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Listing>>> GetListings()
        {
            return await _listingService.GetListingsAsync();
        }

        /// <summary>
        /// Get listing by id.
        /// </summary>
        /// <param name="id">Id of listing to get.</param>
        /// <returns>Listing object if found, otherwise 404 status code.</returns>
        // GET: api/Listings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Listing>> GetListing(int id)
        {
            var listing = await _listingService.GetListingAsync(id);

            if (listing == null)
            {
                return NotFound();
            }

            return listing;
        }

        // PUT: api/Listings/5
        /// <summary>
        /// Update listing by Id.
        /// </summary>
        /// <param name="id">Listing id.</param>
        /// <param name="listing">Listing updated.</param>
        /// <returns>400 status code if url id not match updated id, 204 status code if completed update, not found if id not exists in database.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateListing(int id, Listing listing)
        {
            // TODO: Introduce listing DTO to prevent update status field.
            if (id != listing.Id)
            {
                return BadRequest();
            }

            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                await _listingService.EditListingAsync(currentUser, listing);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Listings
       /// <summary>
       /// Create new listing.
       /// </summary>
       /// <param name="listing">Listing created.</param>
       /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Listing>> CreateListing(Listing listing)
        {
            await _listingService.CreateListingAsync(listing);

            return CreatedAtAction("GetListing", new { id = listing.Id }, listing);
        }

        // DELETE: api/Listings/5
        /// <summary>
        /// Delete listing by id.
        /// </summary>
        /// <param name="id">Listing id to delete.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Listing>> DeleteListing(int id)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                await _listingService.DisableListingAsync(currentUser, id);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }

            var listing = await _listingService.GetListingAsync(id);

            return listing;
        }

        /// <summary>
        /// Enable the disabled listing.
        /// </summary>
        /// <param name="id">Id to enable.</param>
        /// <returns>404 if not found listing, 400 if listing status is not disabled, otherwise return listing be enabled.</returns>
        [HttpPost("{id}/enable")]
        public async Task<ActionResult<Listing>> EnableListing(int id)
        {
            try
            {
                await _listingService.EnableListingAsync(id);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                throw;
            }

            var listing = await _listingService.GetListingAsync(id);

            return listing;
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(int listingId)
        {
            var comments = await _commentService.GetCommentsByListingAsync(listingId);

            if (comments == null)
            {
                return NotFound();
            }

            return comments;
        }

    }
}
