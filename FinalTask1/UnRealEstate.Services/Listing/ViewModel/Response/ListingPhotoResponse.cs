using System.ComponentModel;

namespace UnrealEstate.Models.ViewModels.ResponseViewModels
{
    public class ListingPhotoResponse
    {
        public int Id { get; set; }

        [DisplayName("Photo Url")]
        public string PhotoUrl { get; set; }
    }
}