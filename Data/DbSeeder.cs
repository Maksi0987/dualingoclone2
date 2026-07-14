using Languio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Languio.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Створюємо ролі, якщо їх немає
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Статичний акаунт Адміністратора
            if (await userManager.FindByEmailAsync("admin@languio.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@languio.com",
                    Email = "admin@languio.com",
                    EmailConfirmed = true,
                    Coins = 9999,
                    Experience = 9999
                };
                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Статичний акаунт Користувача
            if (await userManager.FindByEmailAsync("user@languio.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user@languio.com",
                    Email = "user@languio.com",
                    EmailConfirmed = true,
                    Coins = 100,
                    Experience = 0
                };
                var result = await userManager.CreateAsync(user, "User123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}