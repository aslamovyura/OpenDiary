using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
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
        /// <exception cref="ArgumentNullException"></exception>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            try
            {
                var appContextService = serviceProvider.GetRequiredService<ApplicationDbContext>();
                appContextService.Database.Migrate();

                Log.Information(InitializationConstants.MigrationSuccess);
            }
            catch (Exception ex)
            {
                Log.Error(ex, InitializationConstants.MigrationError);
            }
        }
    }
}