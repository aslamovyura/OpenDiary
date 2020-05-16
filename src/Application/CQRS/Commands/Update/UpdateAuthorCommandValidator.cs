using FluentValidation;

namespace Application.CQRS.Commands.Update
{
    /// <summary>
    /// Validation of Post DTO.
    /// </summary>
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public UpdateAuthorCommandValidator()
        {
            RuleFor(author => author.Model.FirstName).MaximumLength(50).NotEmpty();
            RuleFor(author => author.Model.LastName).MaximumLength(50).NotEmpty();
            RuleFor(author => author.Model.Email).EmailAddress().NotEmpty();
        }
    }
}