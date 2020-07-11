using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebUI.Extensions;

namespace WebUI
{
    /// <summary>
    /// Define startup class for application configuration.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration property.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Method to add services to the DI container.
        /// </summary>
        /// <param name="services">Application services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), // Use PostgreSQL for deployment of heroku.com.
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), // Use MS SQL Server for local debug.
                    x => x.MigrationsAssembly("Infrastructure")));

            services.AddApplication();
            services.AddInfrastructure();
            services.AddWebUI();
        }

        /// <summary>
        /// Method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Host enviroment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseLocalization();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHealthChecks("/health").RequireAuthorization();
            });
        }
    }
}