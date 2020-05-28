using System;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using Application.Enums;
using Application.Interfaces;
using Infrastructure.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    /// <summary>
    /// Controller to manage user profile page.
    /// </summary>
    public class ProfileController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="identityService">Application identity service.</param>
        /// <param name="mediator">Layers mediator.</param>
        public ProfileController(IIdentityService identityService, IMediator mediator)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Show user profile page.
        /// </summary>
        /// <param name="userId">Application user identifier.</param>
        /// <returns>User profile page.</returns>
        public async Task<ActionResult> Index(int authorId)
        {
            if (authorId == default)
            {
                return RedirectToAction("Index", "Home");
            }

            // Get author current author.
            var authorQuery = new GetAuthorQuery { Id = authorId };
            var authorDTO = await _mediator.Send(authorQuery);
            authorDTO.Email = await _identityService.GetEmailByIdAsync(authorDTO.UserId);

            // Calculate author statistics.
            var postsQuery = new GetPostsByAuthorIdQuery { AuthorId = authorDTO.Id };
            var postsDTO = await _mediator.Send(postsQuery);
            var postsNumber = postsDTO.Count;

            var commentsQuery = new GetCommentsByAuthorIdQuery { AuthorId = authorDTO.Id };
            var commentsDTO = await _mediator.Send(commentsQuery);
            var commentsNumber = commentsDTO.Count;

            // Check current user if he / she is an author of the current post.
            var userName = HttpContext.User.Identity.Name;
            int currentReaderId;
            if (userName == null)
            {
                currentReaderId = default;
            }
            else
            {
                var userId = await _identityService.GetUserIdByNameAsync(userName);
                var reader = await _mediator.Send(new GetAuthorByUserIdQuery { UserId = userId });

                if (reader == null)
                    currentReaderId = default;
                else
                    currentReaderId = reader.Id;
            }


            // Create view model.
            var model = new ProfileViewModel
            {
                Id = authorId,
                FirstName = authorDTO.FirstName,
                LastName = authorDTO.LastName,
                Email = authorDTO.Email,
                BirthDate = authorDTO.BirthDate.ToString("MMMM d, yyyy"),
                Age = authorDTO.BirthDate.Age(AgeUnits.Year),
                TotalPostsNumber = postsNumber,
                TotalCommentsNumber = commentsNumber,
                Posts = postsDTO,
                CurrentReaderId = currentReaderId,
            };

            return View(model);
        }
    }
}