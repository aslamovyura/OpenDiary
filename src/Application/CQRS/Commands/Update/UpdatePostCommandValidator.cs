using FluentValidation;

namespace Application.CQRS.Commands.Update
{
    /// <summary>
    /// Validation of Post DTO.
    /// </summary>
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public UpdatePostCommandValidator()
        {
            RuleFor(post => post.Model.Title).MaximumLength(50).NotEmpty();
            RuleFor(post => post.Model.Text).NotEmpty();
        }
    }
}