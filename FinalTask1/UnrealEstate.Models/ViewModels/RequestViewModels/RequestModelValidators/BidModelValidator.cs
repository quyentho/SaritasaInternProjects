using FluentValidation;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class BidModelValidator : AbstractValidator<BidRequest>
    {
        public BidModelValidator()
        {
            RuleFor(x => x.Price).NotEmpty().Must(x => x > 0 && x < 100_000_000_000);
        }
    }
}
