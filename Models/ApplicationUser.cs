using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Languio.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Coins { get; set; } = 0;
        public int Experience { get; set; } = 0;
        public int DayStreak { get; set; } = 0;
        public bool HasStreakFreeze { get; set; } = false;

        public DateTime LastActiveDate { get; set; } = DateTime.UtcNow;
        public List<UserProgress> Progresses { get; set; } = new();

        public int? ActiveLanguageCourseId { get; set; }
        public LanguageCourse ActiveLanguageCourse { get; set; }
    }
}