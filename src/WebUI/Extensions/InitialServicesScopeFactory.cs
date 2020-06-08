using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebUI.Extensions
{
    /// <summary>
    /// Define initial services scope factory.
    /// </summary>
    public static class InitialServicesScopeFactory
    {
        /// <summary>
        /// Build services factory.
        /// </summary>
        /// <param name="host">Application Host.</param>
        /// <param name="logger">Logging service.</param>
        public static void Build(IHost host, ILogger logger)
        {
            host = host ?? throw new ArgumentNullException(nameof(host));

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            RuntimeMigrations.Initialize(services, logger);
            IdentityContextSeed.Initialize(services, logger);
        }
    }
}