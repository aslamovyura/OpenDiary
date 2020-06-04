using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    /// <summary>
    /// Application Home Controller.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Constructor of Home controller.
        /// </summary>
        /// <param name="logger">Logger service.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method to invoke main page.
        /// </summary>
        /// <returns>Application main page.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Select application language (based on cookies).
        /// </summary>
        /// <param name="culture">Culture (language).</param>
        /// <param name="returnUrl">Return Url.</param>
        /// <returns>Language change.</returns>
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        /// <summary>
        /// Method to invoke error page.
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}