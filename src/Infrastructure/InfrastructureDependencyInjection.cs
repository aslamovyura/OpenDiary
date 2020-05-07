using System;
using Application.Interfaces;
using CustomIdentityApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    /// <summary>
    /// Extention method to add services on Infrastructure layer.
    /// </summary>
    public static class InfrastructureDependencyInjection
    {
        /// <summary>
        /// Add services of the Infrastrucure layer.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <returns>Services.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IMessageSender, EmailService>();

            return services;
        }

    }
}