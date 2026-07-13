using Languio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Languio.Data
{
    public static class SeedData
    {
        public static async Task Initialize(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await context.Database.MigrateAsync();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var adminEmail = "admin@languio.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}