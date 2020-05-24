using System;
using System.Threading.Tasks;
using Domain.Entities;
using WebUI.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using Infrastructure.Identity;
using System.Threading;
using Application.DTO;
using Application.CQRS.Queries.Get;
using MediatR;
using Application.CQRS.Commands.Update;
using Application.CQRS.Commands.Delete;
using AutoMapper;
using WebUI.ViewModels;
using System.Collections.Generic;

namespace CustomIdentityApp.Controllers
{
    /// <summary>
    /// Contoller to manage application users.
    /// </summary>
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _db;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor of user controller.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public UsersController(UserManager<ApplicationUser> userManager, IApplicationDbContext context, IIdentityService identityService, IMediator mediator, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _db = context ?? throw new ArgumentNullException(nameof(context));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Show page with user/aurhors list.
        /// </summary>
        /// <returns>Page with list of users.</returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var authorsQuery = new GetAuthorsQuery();

            var authors = await _mediator.Send(authorsQuery);
            if (authors == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var models = _mapper.Map<IEnumerable<AuthorDTO>, IEnumerable<AuthorViewModel>>(authors);
            foreach (var model in models)
            {
                model.Email = await _identityService.GetEmailByIdAsync(model.UserId);
            }

            return View(models);
        }

        /// <summary>
        /// Show page to add new user. 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        public IActionResult Create() => View();

        /// <summary>
        /// Process user input to add new user.
        /// </summary>
        /// <param name="model">View mode to create user.</param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    var author = new Author
                    {
                        UserId = user.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        BirthDate = model.BirthDate,
                    };

                    _db.Authors.Add(author);
                    await _db.SaveChangesAsync(new CancellationToken());

                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        /// <summary>
        /// Show page to edit user's info.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>View with EditUserViewModel.</returns>
        public async Task<IActionResult> Edit(string id)
        {
            var authorQuery = new GetAuthorByUserIdQuery { UserId = id };
            var authorDTO = await _mediator.Send(authorQuery);

            if (authorDTO == null)
                return NotFound();

            var email = await _identityService.GetEmailByIdAsync(authorDTO.UserId);
            authorDTO.Email = email;

            var model = _mapper.Map<AuthorDTO, AuthorViewModel>(authorDTO);

            return View(model);
        }

        /// <summary>
        /// Process input date to edit application user.
        /// </summary>
        /// <param name="authorDTO">View model to edit user.</param>
        /// <returns>View with EditUserViewModel.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(AuthorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var authorDTO = _mapper.Map<AuthorViewModel, AuthorDTO>(model);

            var authorCommand = new UpdateAuthorCommand { Model = authorDTO };
            await _mediator.Send(authorCommand);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var authorQuery = new GetAuthorQuery { Id = id };
            var authorDTO = await _mediator.Send(authorQuery);

            if (authorDTO == null)
                return NotFound();

            var result = await _identityService.DeleteUserAsync(authorDTO.UserId);
            if( result.Succeeded )
            {
                var authorCommand = new DeleteAuthorCommand { Id = id };
                await _mediator.Send(authorCommand);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Show page to change user password.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns></returns>
        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        /// <summary>
        /// Process input data to change user password.
        /// </summary>
        /// <param name="model">ChangePasswordViewModel.</param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var _passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<ApplicationUser>)) as IPasswordValidator<ApplicationUser>;
                    var _passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<ApplicationUser>)) as IPasswordHasher<ApplicationUser>;

                    IdentityResult result = await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User is not found.");
            }
            return View(model);
        }
    }
}