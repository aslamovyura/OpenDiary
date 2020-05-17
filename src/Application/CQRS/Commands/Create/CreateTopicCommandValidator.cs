using FluentValidation;

namespace Application.CQRS.Commands.Create
{
    /// <summary>
    /// Validation of Topic DTO.
    /// </summary>
    public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public CreateTopicCommandValidator()
        {
            RuleFor(post => post.Model.Text).MaximumLength(50).NotEmpty();
        }
    }
}