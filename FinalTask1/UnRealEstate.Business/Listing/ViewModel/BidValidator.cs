using FluentValidation;
using UnrealEstate.Business.Listing.ViewModel.Request;

namespace UnrealEstate.Business.Listing.ViewModel
{
    public class BidValidator : AbstractValidator<ListingBidRequest>
    {
        public BidValidator()
        {
            RuleFor(x => x.Price).NotEmpty().Must(x => x > 0 && x < 100_000_000_000);
        }
    }
}