using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            var host = CreateHostBuilder(args).Build();

            // TODO: (ASLM) - Add serilog instead!
            var serviceProvider = new ServiceCollection()
                      .AddLogging()
                      .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>()
                        .CreateLogger<Program>();

            InitialServicesScopeFactory.Build(host, logger);

            host.Run();
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
                });
    }
}