using System;
using System.Threading.Tasks;
using Application.CQRS.Commands.Update;
using Application.CQRS.Queries.Get;
using Application.DTO;
using Application.Enums;
using Application.Interfaces;
using AutoMapper;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="identityService">Application identity service.</param>
        /// <param name="mediator">Layers mediator.</param>
        /// <param name="mapper">Automapper.</param>
        public ProfileController(IIdentityService identityService,
                                 IMediator mediator,
                                 IMapper mapper)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

            var model = _mapper.Map<AuthorDTO, ProfileViewModel>(authorDTO);

            model.Age = authorDTO.BirthDate.Age(AgeUnits.Year);
            model.TotalPostsNumber = postsNumber;
            model.TotalCommentsNumber = commentsNumber;
            model.Posts = postsDTO;
            model.CurrentReaderId = currentReaderId;
            model.BirthDate = authorDTO.BirthDate.ToString("MMMM d, yyyy");

            return View(model);
        }


        /// <summary>
        /// Edit author profile.
        /// </summary>
        /// <param name="userId">Application user identifier.</param>
        /// <returns>User profile page.</returns>
        [HttpGet]
        public async Task<ActionResult> Edit(int authorId)
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
            var model = _mapper.Map<AuthorDTO, ProfileViewModel>(authorDTO);

            model.Age = authorDTO.BirthDate.Age(AgeUnits.Year);
            model.TotalPostsNumber = postsNumber;
            model.TotalCommentsNumber = commentsNumber;
            model.Posts = postsDTO;
            model.CurrentReaderId = currentReaderId;
            model.BirthDate = authorDTO.BirthDate.ToString("MMMM d, yyyy");

            return View(model);
        }

        /// <summary>
        /// Edit author profile.
        /// </summary>
        /// <param name="model">Profile view model</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var authorDTO = _mapper.Map<ProfileViewModel, AuthorDTO>(model);
            var authorCommand = new UpdateAuthorCommand { Model = authorDTO };

            await _mediator.Send(authorCommand);

            return RedirectToAction("Index", "Profile", new { authorId = model.Id });
        }
    }
}