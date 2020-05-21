using System;
using System.Globalization;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace WebUI
{
    /// <summary>
    /// Extension method for services of WebUI layer.
    /// </summary>
    public static class WebUIDependencyInjection
    {
        /// <summary>
        /// Add services for WebUI layer.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <returns>Services of WebUI layer.</returns>
        public static IServiceCollection AddWebUI(this IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            return services;
        }
    }
}