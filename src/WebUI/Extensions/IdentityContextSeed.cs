using System;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebUI.Contants;

namespace WebUI.Extensions
{
    /// <summary>
    /// Create initial identity data.
    /// </summary>
    public class IdentityContextSeed
    {
        /// <summary>
        /// Fill database with initial date.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="logger">Logging service.</param>
        public static void Initialize(IServiceProvider serviceProvider, ILogger logger)
        {
            try
            {
                var contextOptions = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                using var applicationContext = new ApplicationDbContext(contextOptions);
                ApplicationDbContextSeed.SeedAsync(userManager, roleManager, applicationContext).GetAwaiter().GetResult();

                logger.LogInformation(InitializationConstants.SeedSuccess);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, InitializationConstants.SeedError);
            }
        }
    }
}