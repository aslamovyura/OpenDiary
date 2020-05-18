using System;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels.Profile;

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
        public async Task<ActionResult> Index(string userId)
        {
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Get author current author.
            var authorQuery = new GetAuthorByUserIdQuery { UserId = userId };
            var authorDTO = await _mediator.Send(authorQuery);
            authorDTO.Email = await _identityService.GetEmailByIdAsync(authorDTO.UserId);

            // Calculate author age.
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan span = DateTime.Now - authorDTO.BirthDate;
            int ageYears = (zeroTime + span).Year - 1;

            // Calculate author statistics.
            var postsQuery = new GetPostsByAuthorIdQuery { AuthorId = authorDTO.Id };
            var postsDTO = await _mediator.Send(postsQuery);
            var postsNumber = postsDTO.Count;

            // TODO : add comments query.
            var commentsNumber = 0;

            // Create view model.
            var model = new ProfileViewModel
            {
                FirstName = authorDTO.FirstName,
                LastName = authorDTO.LastName,
                Email = authorDTO.Email,
                BirthDate = authorDTO.BirthDate.ToString("MMMM d, yyyy"),
                Age = ageYears,
                TotalPostsNumber = postsNumber,
                TotalCommentsNumber = commentsNumber,
                Posts = postsDTO,
            };

            return View(model);
        }
    }
}