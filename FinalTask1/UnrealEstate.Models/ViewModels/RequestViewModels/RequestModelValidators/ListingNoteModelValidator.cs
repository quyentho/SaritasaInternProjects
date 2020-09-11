using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class ListingNoteModelValidator : AbstractValidator<ListingNoteRequestViewModel>
    {
        public ListingNoteModelValidator()
        {
            RuleFor(x => x.Text).NotEmpty();
        }
    }
}
