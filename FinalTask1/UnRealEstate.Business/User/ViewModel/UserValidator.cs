using System;
using FluentValidation;

namespace UnrealEstate.Business.User.ViewModel.Request.Validator
{
    public class UserValidator : AbstractValidator<UserRequest>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.BirthDay).Must(b => b.DateTime.Date <= DateTimeOffset.Now.DateTime.Date);
        }
    }
}