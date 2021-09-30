using System.Text.RegularExpressions;
using FluentValidation;
using Kainos.Comments.Application.Model.Domain;

namespace Kainos.Comments.Functions.Validators
{
    public class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
    {
        public UpdateCommentRequestValidator()
        {
            RuleFor(x => x.Text).MaximumLength(200)
                .NotEmpty()
                .NotNull()
                .Matches(
                    new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$"));
        }
    }
}