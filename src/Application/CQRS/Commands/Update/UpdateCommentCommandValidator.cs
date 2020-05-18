using FluentValidation;

namespace Application.CQRS.Commands.Update
{
    /// <summary>
    /// Validation of Comment DTO.
    /// </summary>
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public UpdateCommentCommandValidator()
        {
            RuleFor(author => author.Model.Text).NotEmpty();
        }
    }
}