using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class UserFilterCriteriaModelValidator : AbstractValidator<UserFilterCriteriaRequestViewModel>
    {
        public UserFilterCriteriaModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
