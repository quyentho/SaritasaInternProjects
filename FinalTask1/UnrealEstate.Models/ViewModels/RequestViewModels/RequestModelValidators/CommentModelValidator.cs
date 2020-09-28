using FluentValidation;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
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
