using System;
using System.Threading.Tasks;
using WebUI.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTO;
using Application.CQRS.Commands.Create;
using Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;
using WebUI.ViewModels;
using Application.CQRS.Queries.Get;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace WebUI.Controllers
{
    /// <summary>
    /// Controller to manage user account.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        /// <summary>
        /// Constructor of account controller.
        /// </summary>
        /// <param name="identityService">Application identity service.</param>
        /// <param name="mediator">Mediator to access application entities.</param>
        /// <param name="emailService">Service to manage email activities.</param>
        /// <param name="localizer">Service for string localization.</param>
        /// <param name="razorViewToStringRenderer">Renderer for Razor pages (to convert to string format)</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AccountController(IIdentityService identityService,
                                 IEmailService emailService,
                                 IMediator mediator,
                                 IStringLocalizer<AccountController> localizer,
                                 IRazorViewToStringRenderer razorViewToStringRenderer)
        {
            _identityService = identityService ?? throw new ArgumentNullException();
            _emailService = emailService ?? throw new ArgumentNullException();
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _razorViewToStringRenderer = razorViewToStringRenderer ?? throw new ArgumentNullException(nameof(razorViewToStringRenderer));
        }

        /// <summary>
        /// Show registration view.
        /// </summary>
        /// <returns>View for user registration.</returns>
        [HttpGet]
        public IActionResult SignUp()
        {
            // Get info on current user agent. If it is 'Safari', use specific datepicker.
            var userAgent = Request.Headers["User-Agent"].ToString();
            if (Regex.IsMatch(userAgent, "Safari") && !Regex.IsMatch(userAgent, "Chrome"))
                ViewBag.isSafari = true;
            else
                ViewBag.isSafari = false;
            
            return View();
        }
        
        /// <summary>
        /// Process user input on the registration view.
        /// </summary>
        /// <param name="model">User registration view model.</param>
        /// <returns>Redirection to main page.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            model = model ?? throw new ArgumentNullException();

            if (ModelState.IsValid)
            {
                // Create new application user.
                var (result, userId, token) = await _identityService.CreateUserAsync(model.Email, model.Email, model.Password);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, _localizer["UserExist"]);
                    return View(model);
                }

                if (result.Succeeded)
                {
                    // Create command to add new author.
                    var authorDTO = new AuthorDTO
                    {
                        UserId = userId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        BirthDate = model.BirthDate,
                        Email = model.Email
                    };

                    var authorCommand = new CreateAuthorCommand { Model = authorDTO };

                    try
                    {
                        await _mediator.Send(authorCommand);
                    }
                    catch (RequestValidationException failures)
                    {
                        foreach (var error in failures.Failures)
                        {
                            ModelState.AddModelError(string.Empty, error.Value[0]);
                        }
                        return View(model);
                    }

                    // Send confirmation letter to user email.
                    var callbackUrl = Url.Action("SignUpSucceeded", "Account",
                                                 new { id = userId, token = token },
                                                 protocol: HttpContext.Request.Scheme);

                    var homePageUrl = Url.Action("Index", "Home", null, protocol: HttpContext.Request.Scheme);
                    var logoUrl = string.Format("{0}://{1}{2}", Request.Scheme, Request.Host, "/resources/logo-1.png");

                    var emailModel = new EmailViewModel
                    {
                        AuthorName = model.FirstName + " " + model.LastName,
                        CallbackUrl = callbackUrl,
                        HomePageUrl = homePageUrl,
                        LogoUrl = logoUrl,
                    };

                    var body = await _razorViewToStringRenderer.RenderViewToStringAsync("Views/Email/Email_ConfirmEmail.cshtml", emailModel);
                    await _emailService.SendEmailAsync(model.Email, _localizer["ConfirmEmail"], body);

                    return View("ConfirmEmail", model);
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
        /// <param name="id">User identifier.</param>
        /// <param name="token">Confirmation token.</param>
        /// <returns>Page to sign up.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignUpSucceeded(string id, string token)
        {
            if (id != null && token != null)
            {
                var (result, _) = await _identityService.ConfirmEmail(id, token);

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
                var (result, errorKey) = await _identityService.EmailConfirmCheckerAsync(model.Email);

                if (!result)
                {
                    ModelState.AddModelError(string.Empty, _localizer[errorKey]);
                    return View(model);
                }

                var isSignIn = await _identityService.LoginUserAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (isSignIn.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, _localizer["IncorrectPassword"]);
                }    
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

                var userId = await _identityService.GetUserIdByNameAsync(userName);
                var author = await _mediator.Send(new GetAuthorByUserIdQuery { UserId = userId });

                var callbackUrl = Url.Action("ResetPassword", "Account", new { userName, token }, protocol: HttpContext.Request.Scheme);
                var homePageUrl = Url.Action("Index", "Home", null, protocol: HttpContext.Request.Scheme);
                var logoUrl = string.Format("{0}://{1}{2}", Request.Scheme, Request.Host, "/resources/logo-1.png");

                var emailModel = new EmailViewModel
                {
                    AuthorName = author.FirstName + " " + author.LastName,
                    CallbackUrl = callbackUrl,
                    HomePageUrl = homePageUrl,
                    LogoUrl = logoUrl,
                };

                var body = await _razorViewToStringRenderer.RenderViewToStringAsync("Views/Email/Email_ResetPassword.cshtml", emailModel);
                await _emailService.SendEmailAsync(model.Email, _localizer["ResetPassword"], body);

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