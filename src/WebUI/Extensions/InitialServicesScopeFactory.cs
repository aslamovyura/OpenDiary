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
        public static void Build(IHost host)
        {
            host = host ?? throw new ArgumentNullException(nameof(host));

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            RuntimeMigrations.Initialize(services);
            IdentityContextSeed.Initialize(services);
        }
    }
}