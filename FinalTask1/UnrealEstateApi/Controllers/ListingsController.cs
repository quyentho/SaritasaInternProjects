using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public ListingsController(IListingService listingService, ICommentService commentService, UserManager<User> userManager)
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
                User currentUser = await GetCurrentUser();

                await _listingService.EditListingAsync(currentUser, listing);
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
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
            try
            {
                await _listingService.CreateListingAsync(listing);

                return CreatedAtAction("GetListing", new { id = listing.Id }, listing);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
           
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
                User currentUser = await GetCurrentUser();
                await _listingService.DisableListingAsync(currentUser, id);
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NotSupportedException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
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
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }

            var listing = await _listingService.GetListingAsync(id);

            return listing;
        }

        // api/1/comments
        /// <summary>
        /// Gets list comments in listing by listingId.
        /// </summary>
        /// <param name="listingId">Listing id.</param>
        /// <returns>List comments if found, otherwise return not found.</returns>
        [HttpGet("{listingId}/comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(int listingId)
        {
            try
            {
                var comments = await _commentService.GetCommentsByListingAsync(listingId);

                if (comments == null)
                {
                    return NotFound();
                }

                return comments;
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
         
        }

        // api/1/favorite
        /// <summary>
        /// Add listing to favorite for current user, Remove listing from favorite if user already favorited.
        /// </summary>
        /// <param name="listingId">listing id.</param>
        /// <returns></returns>
        [HttpPost("{listingId}/favorite")]
        public async Task<ActionResult> SetFavorite(int listingId)
        {
            bool isFavorite;
            try
            {
                User currentUser = await GetCurrentUser();
                
                // HACK: Hard code user id to test.
                isFavorite = await _listingService.AddOrRemoveFavoriteUserAsync(listingId, "4c79e6d0-311c-4004-a9d0-c88e1e83de8d");
                
                return Ok(isFavorite);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private async Task<User> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
