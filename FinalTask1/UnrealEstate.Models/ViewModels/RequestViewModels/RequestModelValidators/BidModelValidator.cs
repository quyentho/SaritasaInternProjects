using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class BidModelValidator :AbstractValidator<BidRequestViewModel>
    {
        public BidModelValidator()
        {
            RuleFor(x => x.Price).NotEmpty().Must(x => x > 0 && x < 100_000_000_000);
        }
    }
}
