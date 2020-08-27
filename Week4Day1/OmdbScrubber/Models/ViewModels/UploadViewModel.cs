using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmdbScrubber.Models.ViewModels
{
    public class UploadViewModel
    {
        [Required(ErrorMessage = "Imdb id is required to search")]
        public string Input { get; set; }
    }
}
