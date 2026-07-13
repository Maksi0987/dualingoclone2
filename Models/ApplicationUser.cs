using Microsoft.AspNetCore.Identity;
using System;

namespace Languio.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Coins { get; set; }
        public int Experience { get; set; }
        public int DayStreak { get; set; }
        public DateTime LastActiveDate { get; set; }
        public LanguageCourse? ActiveLangeageCourse { get; set; }
    }
}