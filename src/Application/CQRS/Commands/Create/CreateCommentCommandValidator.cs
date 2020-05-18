using FluentValidation;

namespace Application.CQRS.Commands.Create
{
    /// <summary>
    /// Validation of Comment DTO.
    /// </summary>
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public CreateCommentCommandValidator()
        {
            RuleFor(author => author.Model.PostId).NotEmpty();
            RuleFor(author => author.Model.AuthorId).NotEmpty();
            RuleFor(author => author.Model.Text).NotEmpty();
            RuleFor(author => author.Model.Date).NotEmpty();
        }
    }
}