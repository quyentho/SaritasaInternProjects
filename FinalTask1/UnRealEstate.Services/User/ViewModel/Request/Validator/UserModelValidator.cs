using System;
using FluentValidation;
using UnrealEstate.Models.ViewModels.RequestViewModels;

namespace UnrealEstate.Services.User.ViewModel.Request.Validator
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
