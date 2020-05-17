using System;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            string adminEmail = "admin@gmail.com";
            string userEmail = "user@gmail.com";
            string password = "ctyjdfkbnh";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail, EmailConfirmed = true};
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");

                    var adminAuthor = new Author { UserId = admin.Id, FirstName = "admin", LastName = "admin", BirthDate = DateTime.Now };
                    context.Authors.Add(adminAuthor);
                    await context.SaveChangesAsync();

                }
                else
                {
                    throw new Exception("Impossible to set admin role!");
                }
            }

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
                else
                {
                    throw new Exception("Impossible to set user role!");
                }
            }
        }
    }
}