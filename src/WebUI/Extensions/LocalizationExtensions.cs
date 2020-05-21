using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WebUI.Extensions
{
    /// <summary>
    /// Extensions of localization conveyer.
    /// </summary>
    public static class LocalizationExtensions
    {
        /// <summary>
        /// User localization.
        /// </summary>
        /// <param name="app">Application builder.</param>
        public static void UseLocalization(this IApplicationBuilder app)
        {
            app = app ?? throw new ArgumentNullException(nameof(app));

            var locOptions = app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);
        }
    }
}