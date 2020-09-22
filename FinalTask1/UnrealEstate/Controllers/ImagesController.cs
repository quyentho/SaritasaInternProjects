using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace UnrealEstate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ImagesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ActionResult GetImage(string photoUrl)
        {
            var path = _configuration.GetSection("ImagesPhysicalPath").Value;

            path = Path.Combine(path, Path.GetFileName(photoUrl));

            var fileBytes = System.IO.File.ReadAllBytes(path);

            return this.File(fileBytes, "image/jpeg");
        }
    }
}
