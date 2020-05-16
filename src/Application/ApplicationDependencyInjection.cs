using System;
using System.Reflection;
using Application.Behaviors;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    /// <summary>
    /// Extension method to add services of Application lay
    /// </summary>
    public static class ApplicationDependencyInjection
    {
        /// <summary>
        /// Add services of application layer .
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <returns>Services collection.</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Add validation behavior.
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;
        }
    }
}