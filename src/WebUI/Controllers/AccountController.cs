using System;
using System.Threading.Tasks;
using WebUI.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        /// <summary>
        /// Constructor of account controller.
        /// </summary>
        /// <param name="userManager">Manager of the users in persistence store.</param>
        /// <param name="signInManager">Manager of users' sign in.</param>
        /// <param name="emailService">Service to manage email activities.</param>
        public AccountController(IIdentityService identityService,
            IEmailService emailService, IRazorViewToStringRenderer razorViewToStringRenderer)
        {
            _identityService = identityService ?? throw new ArgumentNullException();
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
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            model = model ?? throw new ArgumentNullException();

            if (ModelState.IsValid)
            {

                var (result, userId, token) = await _identityService.CreateUserAsync(model.FirstName, model.LastName, model.Email, model.Email, model.BirthDate, model.Password);

                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "User is already exists!");
                    return View(model);
                }

                //// Add new user
                if (result.Succeeded)
                {
                    var callbackUrl = Url.Action("ConfirmEmail", "Account",
                                                 new { userId = userId, token = token },
                                                 protocol: HttpContext.Request.Scheme);

                    await _emailService.SendEmailAsync(model.Email, "Confirm your account in OpenDiary",
                        $"Confirm registration by clicking this link: <a href='{callbackUrl}'>link</a>");

                    return View("RegisterSucceeded", model);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
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

            if (userId != null && token != null)
            {
                var (result, _) = await _identityService.ConfirmEmail(userId, token);

                if (result.Succeeded)
                    return View();
            }

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
        /// Process user input on the login page.
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
                var (result, message) = await _identityService.EmailConfirmCheckerAsync(model.Email);

                if (!result)
                {
                    ModelState.AddModelError(string.Empty, message);
                    return View(model);
                }

                var isSignIn = await _identityService.LoginUserAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (isSignIn.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                ModelState.AddModelError("", "Incorrect email or password!");

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
            await _identityService.LogoutUserAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}