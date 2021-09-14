using FluentValidation;
using Kainos.Comments.Application.Model;

namespace Kainos.Comments.Functions.Validators
{
    class AddCommentRequestValidator : AbstractValidator<AddCommentRequest>
    {
        public AddCommentRequestValidator()
        {
            RuleFor(x => x.Text).MaximumLength(200);
            RuleFor(x => x.Text).NotNull();
            RuleFor(x => x.Text).NotEmpty();
            RuleFor(x => x.Author).MaximumLength(50);
            RuleFor(x => x.Author).NotNull();
            RuleFor(x => x.Author).NotEmpty();
        }
    }
}