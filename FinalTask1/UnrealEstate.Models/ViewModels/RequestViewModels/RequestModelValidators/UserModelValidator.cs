using FluentValidation;
using System;

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
