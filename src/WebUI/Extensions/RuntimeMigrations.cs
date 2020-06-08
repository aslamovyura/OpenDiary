using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebUI.Contants;

namespace WebUI.Extensions
{
    /// <summary>
    /// Define runtime migrations.
    /// </summary>
    public class RuntimeMigrations
    {
        /// <summary>
        /// Implement runtime migration.
        /// </summary>
        /// <param name="serviceProvider">Services provider.</param>
        /// <param name="logger">Logging service.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Initialize(IServiceProvider serviceProvider,
                                      ILogger logger)
        {
            serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            try
            {
                var appContextService = serviceProvider.GetRequiredService<ApplicationDbContext>();
                appContextService.Database.Migrate();

                logger.LogInformation(InitializationConstants.MigrationSuccess);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, InitializationConstants.MigrationError);
            }
        }
    }
}