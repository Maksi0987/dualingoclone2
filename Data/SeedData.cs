using Languio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Languio.Data
{
    public static class SeedData
    {
        public static async Task Initialize(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await context.Database.MigrateAsync();

            // 1. Створення ролі та Адміністратора
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

            // 2. Додавання стартових курсів, якщо база порожня
            if (!context.Courses.Any())
            {
                var enCourse = new LanguageCourse { LanguageCode = "en", Name = "Англійська" };
                var esCourse = new LanguageCourse { LanguageCode = "es", Name = "Іспанська" };
                var deCourse = new LanguageCourse { LanguageCode = "de", Name = "Німецька" };

                context.Courses.AddRange(enCourse, esCourse, deCourse);
                await context.SaveChangesAsync();

                // 3. Створення базового модуля та уроку
                var enGroup = new LanguageLessonGroup { Title = "Основи", LanguageCourseId = enCourse.Id };
                context.Groups.Add(enGroup);
                await context.SaveChangesAsync();

                var enLesson = new LanguageLesson { Title = "Привітання", Order = 1, LanguageLessonGroupId = enGroup.Id };
                context.Lessons.Add(enLesson);
                await context.SaveChangesAsync();

                // 4. Створення базового тестового запитання
                var question = new LanguageQuestion
                {
                    LanguageLessonId = enLesson.Id,
                    PromptText = "Як сказати 'Привіт' англійською?",
                    Type = QuestionType.MultipleChoice
                };
                context.Questions.Add(question);
                await context.SaveChangesAsync();

                var opt1 = new AnswerOption { LanguageQuestionId = question.Id, Text = "Hello", IsCorrect = true };
                var opt2 = new AnswerOption { LanguageQuestionId = question.Id, Text = "Bye", IsCorrect = false };
                context.AnswerOptions.AddRange(opt1, opt2);
                await context.SaveChangesAsync();
            }
        }
    }
}