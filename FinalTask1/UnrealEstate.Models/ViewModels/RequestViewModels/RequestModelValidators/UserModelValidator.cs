using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class UserModelValidator : AbstractValidator<UserRequest>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.BirthDay).Must(b => b.DateTime.Date <= DateTimeOffset.Now.DateTime.Date);
        }
    }
}
