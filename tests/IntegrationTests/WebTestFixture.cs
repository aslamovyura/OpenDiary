using System;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebUI;

namespace IntegrationTests
{
    /// <summary>
    /// Fixture for integration tests.
    /// </summary>
    public class WebTestFixture : WebApplicationFactory<Startup>
    {
        /// <summary>
        /// Configuration of the Host.
        /// </summary>
        /// <param name="builder">Host provider.</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services => {

                services.AddEntityFrameworkInMemoryDatabase();

                var provider = services
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
                
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("Identity");
                    options.UseInternalServiceProvider(provider);
                });

                var sp = services.BuildServiceProvider();

                using(var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;

                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var loggerFactory = scopedServices.GetRequiredService<ILoggerFactory>();
                    var logger = scopedServices.GetRequiredService<ILogger<WebTestFixture>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                        ApplicationDbContextSeed.SeedAsync(userManager, roleManager, db).Wait();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred seeding the database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
    }
}