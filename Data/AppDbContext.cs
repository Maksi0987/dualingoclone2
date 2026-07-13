using Languio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Languio.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LanguageCourse> Courses { get; set; }
        public DbSet<LanguageLesson> Lessons { get; set; }
        public DbSet<LanguageLessonGroup> Groups { get; set; }
        public DbSet<LanguageQuestion> Questions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
    }
}