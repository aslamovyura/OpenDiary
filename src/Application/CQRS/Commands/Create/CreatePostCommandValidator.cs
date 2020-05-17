using FluentValidation;

namespace Application.CQRS.Commands.Create
{
    /// <summary>
    /// Validation of Post DTO.
    /// </summary>
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public CreatePostCommandValidator()
        {
            RuleFor(post => post.Model.AuthorId).NotEmpty();
            RuleFor(post => post.Model.TopicId).NotEmpty();
            RuleFor(post => post.Model.Title).MaximumLength(50).NotEmpty();
            RuleFor(post => post.Model.Text).NotEmpty();
            RuleFor(post => post.Model.Date).NotEmpty();
        }
    }
}