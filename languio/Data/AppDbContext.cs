using Languio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Languio.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions options) : base(options) {}
        public DbSet<LanguageCourse> Courses { get; set; }
        public DbSet<LanguageLesson> Lessons { get; set; }
        public DbSet<LanguageLessonGroup> Groups { get; set; }
        public DbSet<LanguageQuestion> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
