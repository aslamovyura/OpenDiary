using System;
using System.Threading.Tasks;
using WebUI.ViewModels.Account;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Application.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMessageSender _emailService;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        /// <summary>
        /// Constructor of account controller.
        /// </summary>
        /// <param name="userManager">Manager of the users in persistence store.</param>
        /// <param name="signInManager">Manager of users' sign in.</param>
        /// <param name="emailService">Service to manage email activities.</param>
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            IMessageSender emailService, IRazorViewToStringRenderer razorViewToStringRenderer)
        {
            _userManager = userManager ?? throw new ArgumentNullException();
            _signInManager = signInManager ?? throw new ArgumentNullException();
            _emailService = emailService ?? throw new ArgumentNullException();
            _razorViewToStringRenderer = razorViewToStringRenderer ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Show registration view.
        /// </summary>
        /// <returns>View for user registration.</returns>
        [HttpGet]
        public IActionResult Register() => View();
        
        /// <summary>
        /// Process user input on the registration view.
        /// </summary>
        /// <param name="model">User registration view model.</param>
        /// <returns>Redirection to main page.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
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

                // Add new user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Generate user token
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account",
                                                 new { userId = user.Id, token = token },
                                                 protocol: HttpContext.Request.Scheme);

                    await _emailService.SendEmailAsync(model.Email, "Confirm your account in OpenDiary",
                        $"Confirm registration by clicking this link: <a href='{callbackUrl}'>link</a>");


                    return View("RegisterSucceeded", model);
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
        /// Confirm email.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="token">Confirmation token.</param>
        /// <returns>Certain view.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }

            // Mark email as confirmed 
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            else
                return View("Error");
        }

        /// <summary>
        /// Show view for user login.
        /// </summary>
        /// <param name="returnUrl">Return URL after login.</param>
        /// <returns>View for user login.</returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = null) => View(new LoginViewModel { ReturnUrl = returnUrl });

        /// <summary>
        /// Process user input on the user login page.
        /// </summary>
        /// <param name="model">User login view model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    //TODO: add check whether email is confirmed.

                    // Check if URL is belonging to application.
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Incorrect email or password!");
            }
            return View(model);
        }

        /// <summary>
        /// User logout.
        /// </summary>
        /// <returns>Redirect to the Home page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Delete authentification cookies.
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}