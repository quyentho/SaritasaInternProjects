using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class ResetPasswordModel : AbstractValidator<ResetPasswordRequestViewModel>
    {
        public ResetPasswordModel()
        {
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Token).NotEmpty();

            RuleFor(x => x.NewPassword).NotEmpty();

            RuleFor(x => x.ConfirmPassword)
                        .Equal(x => x.NewPassword);
        }
    }
}
