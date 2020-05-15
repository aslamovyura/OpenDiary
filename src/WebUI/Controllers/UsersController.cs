using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using WebUI.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Infrastructure.Identity;
using System.Threading;
using System.Collections.Generic;
using Application.DTO;

namespace CustomIdentityApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _db;
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Constructor of user controller.
        /// </summary>
        public UsersController(UserManager<ApplicationUser> userManager, IApplicationDbContext context, IIdentityService identityService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _db = context ?? throw new ArgumentNullException(nameof(context));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        /// <summary>
        /// Show page with user list.
        /// </summary>
        /// <returns>Page with list of users.</returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var models = new List<AuthorDTO>();

            var users = _userManager.Users.ToList();
            if (users != null)
            {
                foreach(var user in users)
                {
                    var author = await _db.Authors.FirstOrDefaultAsync(a => a.UserId == user.Id);
                    models.Add(new AuthorDTO
                    {
                        FirstName = author.FirstName,
                        LastName = author.LastName,
                        BirthDate = author.BirthDate,
                        Posts = author.Posts,
                        Comments = author.Comments,
                        Email = user.Email,
                        UserId = user.Id
                    });
                }
            }

            return View(models);
            //return View(_userManager.Users.ToList());
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

                //(var result, var userId, var token) = await _identityService.CreateUserAsync(model.FirstName, model.LastName, model.Email, model.Email, model.BirthDate, model.Password);
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
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var author = await _db.Authors.FirstOrDefaultAsync(a => a.UserId == id);
            if (author == null)
                return NotFound();

            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Email = user.Email,
                BirthDate = author.BirthDate
            };
            return View(model);
        }

        /// <summary>
        /// Process input date to edit application user.
        /// </summary>
        /// <param name="model">View model to edit user.</param>
        /// <returns>View with EditUserViewModel.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                var author = await _db.Authors.FirstOrDefaultAsync(a => a.UserId == model.Id);

                if (user != null && author != null)
                {
                    author.FirstName = model.FirstName;
                    author.LastName = model.LastName;
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    author.BirthDate = model.BirthDate;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                    _db.Authors.Update(author);
                    await _db.SaveChangesAsync(new CancellationToken());
                }
            }
            return View(model);
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }

            var author = await _db.Authors.FirstOrDefaultAsync(a => a.UserId == id);
            if (author != null)
            {
                _db.Authors.Remove(author);
                await _db.SaveChangesAsync(new CancellationToken());

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

        /// <summary>
        /// View user page
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<IActionResult> ViewUser(string userName)
        {
            if (userName == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var author = await _db.Authors.FirstOrDefaultAsync(a => a.UserId == user.Id);

                // Calculate user
                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan span = DateTime.Now - author.BirthDate;
                int ageYears = (zeroTime + span).Year - 1;

                // Calculate statistics
                var posts = await _db.Posts.Where(post => post.AuthorId == author.Id)
                    .OrderByDescending(post => post.Date)
                    .ToListAsync();

                var postsNumber = posts.Count;
                var commentsNumber = _db.Comments.Where(post => post.AuthorId == author.Id)
                    .OrderByDescending(post => post.Date)
                    .ToListAsync().GetAwaiter().GetResult().Count;

                ViewUserViewModel model = new ViewUserViewModel
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Email = user.Email,
                    BirthDate = author.BirthDate.ToString("MMMM d, yyyy"),
                    Age = ageYears,
                    TotalPostsNumber = postsNumber,
                    TotalCommentsNumber = commentsNumber,
                    Posts = posts
                };
                return View(model);
            }

            // Default action
            return RedirectToAction("Index","Home");
        }
    }
}