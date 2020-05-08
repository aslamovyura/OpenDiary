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

        /// <summary>
        /// Page for recovery of the forgotten password.
        /// </summary>
        /// <returns>View for the password recovery.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword() => View();

        /// <summary>
        /// Recovery of the forgotten password.
        /// </summary>
        /// <param name="model">Password recovery model.</param>
        /// <returns>View for the password recovery confirmation.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));

            if (ModelState.IsValid)
            {
                var (result, _, userName, token) = await _identityService.ForgotPassword(model.Email);

                if (!result)
                {
                    return View("ForgotPasswordUnknownAccount", model);
                }

                var callbackUrl = Url.Action("ResetPassword", "Account", new { userName, token }, protocol: HttpContext.Request.Scheme);

                //await _emailService.SendEmailAsync(model.Email, ErrorConstants.AccountResetPassword, body);
                await _emailService.SendEmailAsync(model.Email, "Reset password of OpenDiary account",
                        $"Reset password by clicking this link: <a href='{callbackUrl}'>link</a>");

                ViewData["IsAccontExist"] = true;
                return View("ForgotPasswordConfirmation", model);
            }

            return View(model);
        }

        /// <summary>
        /// Password reset.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="token">Confirmation Token</param>
        /// <returns>View to reset password.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userName = null, string token = null)
        {
            if (userName != null || token != null)
            {
                return View();
            }

            return RedirectToAction("Error", "Home");
        }

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="model">View model for password reset.</param>
        /// <returns>View for password reset confirmation.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _identityService.ResetPassword(model.UserName, model.Password, model.Token);

            if (result == null || result.Succeeded)
            {
                return View("ResetPasswordSucceeded");
            }

            return View(model);
        }
    }
}