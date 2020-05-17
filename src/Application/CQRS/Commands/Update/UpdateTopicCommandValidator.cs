using FluentValidation;

namespace Application.CQRS.Commands.Update
{
    /// <summary>
    /// Validation of Topic DTO.
    /// </summary>
    public class UpdateTopicCommandValidator : AbstractValidator<UpdateTopicCommand>
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public UpdateTopicCommandValidator()
        {
            RuleFor(post => post.Model.Text).NotEmpty();
        }
    }
}