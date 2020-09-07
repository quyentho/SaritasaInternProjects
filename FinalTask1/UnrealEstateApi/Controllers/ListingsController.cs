using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        public ListingsController(IListingService listingService)
        {
            this._listingService = listingService;
        }

        /// <summary>
        /// Get all Listings.
        /// </summary>
        /// <returns>List of Listing.</returns>
        // GET: api/Listings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Listing>>> GetListings()
        {
            return await _listingService.GetListings();
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
            var listing = await _listingService.GetListing(id);

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
                await _listingService.EditListing(listing);
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
            await _listingService.CreateListing(listing);

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
                await _listingService.DisableListing(id);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }

            var listing = await _listingService.GetListing(id);

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
                await _listingService.EnableListing(id);
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

            var listing = await _listingService.GetListing(id);

            return listing;
        }

        [HttpGet()]
        public async Task<ActionResult<Comment>> GetComments(int listingId)
        {

        }

    }
}
