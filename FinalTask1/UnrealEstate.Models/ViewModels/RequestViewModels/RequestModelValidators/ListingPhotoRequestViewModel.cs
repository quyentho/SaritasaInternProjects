using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class ListingPhotoRequestViewModel : AbstractValidator<RequestViewModels.ListingPhotoRequestViewModel>
    {
        public ListingPhotoRequestViewModel()
        {
            // TODO: will change when model changed.
            RuleFor(x => x.PhotoUrl).NotEmpty();
        }
    }
}
