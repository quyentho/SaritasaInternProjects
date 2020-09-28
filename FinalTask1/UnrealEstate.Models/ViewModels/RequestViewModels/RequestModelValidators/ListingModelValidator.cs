using FluentValidation;
using System;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class ListingModelValidator : AbstractValidator<ListingRequest>
    {
        public ListingModelValidator()
        {
            RuleFor(x => x.StatingPrice).NotEmpty().Must(x => x > 0 && x < 100_000_000_000);

            RuleFor(x => x.DueDate)
                .NotEmpty()
                .Must(x => x.DateTime.Date > DateTime.Now.Date && x.DateTime.Date <= x.DateTime.AddMonths(2))
                .WithMessage("Due date must greater than today and not exceeds 2 months from now");

            RuleFor(x => x.BuiltYear).Must(x => x > 0 && x <= DateTimeOffset.Now.Year).When(x => x.BuiltYear.HasValue)
                .WithMessage("Build year cannot greater than now ");
        }
    }
}
