using System.ComponentModel.DataAnnotations;

namespace OmdbScrubber.Models.ViewModels
{
    public class UploadViewModel
    {
        [Required(ErrorMessage = "Imdb id is required to search")]
        public string Input { get; set; }
    }
}
