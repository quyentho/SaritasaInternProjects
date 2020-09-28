namespace UnrealEstate.Models.ViewModels.RequestViewModels
{
    public class ListingFilterCriteriaRequest
    {
        public string Address { get; set; }

        public uint? MaxPrice { get; set; }

        public uint? MinPrice { get; set; }

        public uint? MaxAge { get; set; }

        public uint? Offset { get; set; }

        public uint? Limit { get; set; }

        public double? MinSize { get; set; }

        public string OrderBy { get; set; }
    }
}