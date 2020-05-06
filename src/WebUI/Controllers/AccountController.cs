using System.Threading.Tasks;
using WebUI.ViewModels.Account;
using CustomIdentityApp.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// Constructor of account controller.
        /// </summary>
        /// <param name="userManager">Manager of the users in persistence store.</param>
        /// <param name="signInManager">Manager of users' sing in.</param>
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Show registration view.
        /// </summary>
        /// <returns>View for user registration.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            ViewData["OnConfirming"] = "false";
            return View();
        }
        

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
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account",
                                                 new { userId = user.Id, code = code },
                                                 protocol: HttpContext.Request.Scheme);

                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Confirm your account in OpenDiary",
                        $"Confirm registration by clicking this link: <a href='{callbackUrl}'>link</a>");

                    // Show confirming page
                    ViewData["OnConfirming"] = "true";
                    return View(model);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            ViewData["OnConfirming"] = "false";
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }

            // Mark email as confirmed 
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                //await _signInManager.SignInAsync(user, false);
                //return RedirectToAction("Index", "Home");
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
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
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