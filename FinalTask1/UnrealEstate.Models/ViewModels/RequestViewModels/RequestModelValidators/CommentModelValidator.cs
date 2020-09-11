using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class CommentModelValidator : AbstractValidator<CommentRequestViewModel>
    {
        public CommentModelValidator()
        {
            RuleFor(x => x.Text).NotEmpty();

            RuleFor(x => x.ListingId).NotNull();
        }
    }
}
