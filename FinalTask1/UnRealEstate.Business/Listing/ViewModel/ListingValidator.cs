using System;
using FluentValidation;

namespace UnrealEstate.Business.Listing.ViewModel
{
    public class ListingValidator : AbstractValidator<ListingRequest>
    {
        public ListingValidator()
        {
            RuleFor(x => x.StatingPrice).NotEmpty().Must(x => x > 0 && x < 100_000_000_000);

            RuleFor(x => x.DueDate)
                .NotEmpty()
                .Must(x => x.DateTime.Date > DateTime.Now.Date && x.DateTime.Date <= x.DateTime.AddMonths(2).Date)
                .WithMessage("Due date must greater than today and not exceeds 2 months from now");

            RuleFor(x => x.BuiltYear).Must(x => x > 0 && x <= DateTimeOffset.Now.Year).When(x => x.BuiltYear.HasValue)
                .WithMessage("Build year cannot greater than now ");
        }
    }
}