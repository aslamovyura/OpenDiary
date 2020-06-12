using Infrastructure.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using WebUI.Contants;
using WebUI.Extensions;

namespace WebUI
{
    /// <summary>
    /// Define program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Run application.
        /// </summary>
        /// <param name="args">Application arguments.</param>
        public static void Main(string[] args)
        {

            Log.Logger = SerilogConfiguration.LoggerConfig();

            try
            {
                Log.Information(InitializationConstants.WebHostStarting);

                var host = CreateHostBuilder(args).Build();

                InitialServicesScopeFactory.Build(host);

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, InitializationConstants.WebHostTerminated);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Host builder configuration.
        /// </summary>
        /// <param name="args"></param>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                });
    }
}