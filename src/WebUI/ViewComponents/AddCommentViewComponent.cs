using System;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.ViewComponents
{
    /// <summary>
    /// View component to add new comment.
    /// </summary>
    public class AddCommentViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Define view component to add new comment.
        /// </summary>
        /// <param name="mediator">Entities mediator.</param>
        /// <param name="identityService">Application identity service.</param>
        public AddCommentViewComponent(IMediator mediator, IIdentityService identityService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        /// <summary>
        /// Invoke view component for adding comment.
        /// </summary>
        /// <param name="postId">Post identifier.</param>
        /// <returns>Page with full post.</returns>
        public async Task<IViewComponentResult> InvokeAsync (int postId)
        {
            var userName = HttpContext.User.Identity.Name;

            // Anauthorized user.
            if (userName == null)
            {
                return Content(string.Empty);
            }

            var userId = await _identityService.GetUserIdByNameAsync(userName);
            var authorQuery = new GetAuthorByUserIdQuery { UserId = userId };
            var author = await _mediator.Send(authorQuery);

            var model = new CommentViewModel
            {
                PostId = postId,
                AuthorId = author.Id,
                Author = author.FirstName + " " + author.LastName,
            };

            return View("AddComment", model);
        }
    }
}