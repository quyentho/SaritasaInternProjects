using FluentValidation;

namespace UnrealEstate.Business.Listing.ViewModel.Request.Validator
{
    public class BidModelValidator : AbstractValidator<ListingBidRequest>
    {
        public BidModelValidator()
        {
            RuleFor(x => x.Price).NotEmpty().Must(x => x > 0 && x < 100_000_000_000);
        }
    }
}