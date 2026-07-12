using Languio.Models;
using Microsoft.AspNetCore.Identity;

namespace languio.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Coins { get; set; }
        public int DayStreak { get; set; }
        public DateTime LastActiveDate { get; set; }
        public LanguageCourse ActiveLangeageCourse { get; set; }
        public List<UserProgress> Progresses { get; set; } 
    }
}