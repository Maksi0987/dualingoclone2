using Languio.Models;
using Microsoft.AspNetCore.Identity;

namespace Languio.Data
{
    public static class SeedData
    {
        public static async Task Initialize(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            context.Database.EnsureCreated();
            
            // Створення Адміна, якщо немає
            if (await userManager.FindByEmailAsync("admin@languio.com") == null)
            {
                var admin = new ApplicationUser { UserName = "admin@languio.com", Email = "admin@languio.com" };
                await userManager.CreateAsync(admin, "Admin123!");
            }
        }
    }
}