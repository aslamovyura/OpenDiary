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

namespace CustomIdentityApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Constructor of user controller.
        /// </summary>
        /// <param name="userManager">Object to deal with application users.</param>
        public UsersController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _db = context;
        }

        /// <summary>
        /// Show page with user list.
        /// </summary>
        /// <returns>Page with list of users.</returns>
        [Authorize(Roles = "admin")]
        public IActionResult Index() => View(_userManager.Users.ToList());

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
                User user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    BirthDate = model.BirthDate
                };

                var result = await _userManager.CreateAsync(user, model.Password);
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
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                BirthDate = user.BirthDate
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
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.BirthDate = model.BirthDate;

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
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
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
            User user = await _userManager.FindByIdAsync(id);
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
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var _passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

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

            User user = await _db.Users.FirstOrDefaultAsync(user => user.UserName == userName);
            if (user != null)
            {
                // Calculate user
                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan span = DateTime.Now - user.BirthDate;
                int ageYears = (zeroTime + span).Year - 1;

                // Calculate statistics
                var posts = await _db.Posts.Where(post => post.UserId == user.Id)
                    .OrderByDescending(post => post.Date)
                    .ToListAsync();

                var postsNumber = posts.Count;
                var commentsNumber = _db.Comments.Where(post => post.UserId == user.Id)
                    .OrderByDescending(post => post.Date)
                    .ToListAsync().GetAwaiter().GetResult().Count;

                ViewUserViewModel model = new ViewUserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    BirthDate = user.BirthDate.ToString("MMMM d, yyyy"),
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