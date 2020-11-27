using System.ComponentModel;

namespace UnrealEstate.Business.Listing.ViewModel
{
    public class ListingPhotoResponse
    {
        public int Id { get; set; }

        [DisplayName("Photo Url")] public string PhotoUrl { get; set; }
    }
}