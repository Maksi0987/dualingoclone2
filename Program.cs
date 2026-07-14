using Languio.Data;
using Languio.Models;
using Languio.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       "Server=(localdb)\\mssqllocaldb;Database=LanguioDb;Trusted_Connection=True;";
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// ДОДАНО: Реєстрація заглушок для Google та Facebook, щоб кнопки не викликали помилку "No authentication handler"
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = "demo-google-client-id";
        googleOptions.ClientSecret = "demo-google-client-secret";
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = "demo-facebook-app-id";
        facebookOptions.AppSecret = "demo-facebook-app-secret";
    });

// ДОДАНО: Реєстрація сервісу відправки email
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedData.Initialize(context, userManager, roleManager);
}

app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<Languio.Data.AppDbContext>();
        context.Database.Migrate(); // Оновлює БД при запуску
        await Languio.Data.DbSeeder.SeedRolesAndUsersAsync(services); // Створює статичні акаунти
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка міграції/сідінгу: {ex.Message}");
    }
}


app.Run();