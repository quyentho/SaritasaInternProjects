using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UnrealEstate.Business.Listing.Service;
using UnrealEstate.Business.User.Service;
using UnrealEstate.Business.Utils;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Controllers
{
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IListingService _listingService;
        private readonly IUserService _userService;


        public PhotosController(IConfiguration configuration, IListingService listingService, IUserService userService)
        {
            _configuration = configuration;
            _listingService = listingService;
            _userService = userService;
        }

        [Route("api/GetImage")]
        public ActionResult GetImage(string photoUrl)
        {
            var path = _configuration.GetSection("ImagesPhysicalPath").Value;

            path = Path.Combine(path, Path.GetFileName(photoUrl));

            var fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, "image/jpeg");
        }

        [HttpGet]
        [Route("[controller]/[action]/{photoId}")]
        public async Task<IActionResult> Delete([FromQuery]int listingId, int photoId, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await HttpContextHelper.GetCurrentUserAsync(HttpContext, _userService);

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

            return RedirectToAction("Edit","Listings", new { id = listingId, returnUrl });
        }
    }
}