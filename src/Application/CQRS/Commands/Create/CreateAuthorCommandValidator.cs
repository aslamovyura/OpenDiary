using FluentValidation;

namespace Application.CQRS.Commands.Create
{
    /// <summary>
    /// Validation of Author DTO.
    /// </summary>
    public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public CreateAuthorCommandValidator()
        {
            RuleFor(author => author.Model.UserId).NotEmpty();
            RuleFor(author => author.Model.FirstName).MaximumLength(50).NotEmpty();
            RuleFor(author => author.Model.LastName).MaximumLength(50).NotEmpty();
            RuleFor(author => author.Model.BirthDate).NotEmpty();
            RuleFor(author => author.Model.Email).EmailAddress().NotEmpty();
        }
    }
}