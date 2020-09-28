﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace UnrealEstate.Controllers
{
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ImagesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("api/GetImage")]
        public ActionResult GetImage(string photoUrl)
        {
            var path = _configuration.GetSection("ImagesPhysicalPath").Value;

            path = Path.Combine(path, Path.GetFileName(photoUrl));

            var fileBytes = System.IO.File.ReadAllBytes(path);

            return this.File(fileBytes, "image/jpeg");
        }
    }
}
