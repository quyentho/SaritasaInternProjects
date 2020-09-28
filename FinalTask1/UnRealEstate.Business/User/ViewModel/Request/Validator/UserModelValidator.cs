using System;
using FluentValidation;

namespace UnrealEstate.Business.User.ViewModel.Request.Validator
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