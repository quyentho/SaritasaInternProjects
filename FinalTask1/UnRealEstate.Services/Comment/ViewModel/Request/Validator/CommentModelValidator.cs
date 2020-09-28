using FluentValidation;
using UnrealEstate.Models.ViewModels.RequestViewModels;

namespace UnrealEstate.Services.Comment.ViewModel.Request.Validator
{
    public class CommentModelValidator : AbstractValidator<CommentRequest>
    {
        public CommentModelValidator()
        {
            RuleFor(x => x.Text).NotEmpty();

            RuleFor(x => x.ListingId).NotNull();
        }
    }
}