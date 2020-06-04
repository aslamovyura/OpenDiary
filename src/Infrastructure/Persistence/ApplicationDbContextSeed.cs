using System;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence
{
    /// <summary>
    /// Class to seed the context.
    /// </summary>
    public class ApplicationDbContextSeed
    {
        /// <summary>
        /// Fill database with seed data.
        /// </summary>
        /// <param name="userManager">Manager of application users.</param>
        /// <param name="roleManager">Manager of user roles.</param>
        /// <param name="context">Applicatino context.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task SeedAsync( UserManager<ApplicationUser> userManager,
                                            RoleManager<IdentityRole> roleManager,
                                            ApplicationDbContext context)
        {
            userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            context = context ?? throw new ArgumentNullException(nameof(context));

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            string adminEmail = "admin@gmail.com";
            string userEmail = "user@gmail.com";
            string password = "ctyjdfkbnh";

            // Add admin user.
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail, EmailConfirmed = true};
                var result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");

                    var adminAuthor = new Author { UserId = admin.Id, FirstName = "admin", LastName = "admin", BirthDate = DateTime.Now };
                    context.Authors.Add(adminAuthor);
                    await context.SaveChangesAsync();
                }
            }

            // Add demo user.
            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                ApplicationUser user = new ApplicationUser { Email = userEmail, UserName = userEmail, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");

                    var userAuthor = new Author { UserId = user.Id, FirstName = "user", LastName = "user", BirthDate = DateTime.Now };
                    context.Authors.Add(userAuthor);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}