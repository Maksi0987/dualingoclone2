using languio.Models;
using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Languio.Controllers
{
    public class LearnController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LearnController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string lang)
        {
            var user = await _userManager.GetUserAsync(User);

            var learnViewModel = new LearnViewModel();

            if (user != null)
            {
                var daysPassed = (DateTime.UtcNow.Date - user.LastActiveDate.Date).TotalDays;
                if (daysPassed >= 2)
                {
                    user.DayStreak = 0;
                }

                user.LastActiveDate = DateTime.UtcNow;

                await _userManager.UpdateAsync(user);

                var currentLangCode = string.IsNullOrEmpty(lang) && user.ActiveLangeageCourse != null ? user.ActiveLangeageCourse.LanguageCode : lang;

                var course = await _context.Courses.FirstOrDefaultAsync(c => c.LanguageCode == currentLangCode);

                int courseId = course?.Id ?? 0;

                learnViewModel.LanguageCode = currentLangCode;
                learnViewModel.Coins = user.Coins;
                learnViewModel.DayStreak = user.DayStreak;
                learnViewModel.LanguageLessonGroups = await _context.Groups
                    .Where(g => g.LanguageCourseId == courseId)
                    .Include(g => g.Lessons)
                    .ToListAsync();
            }
            else
            {
                if (string.IsNullOrEmpty(lang)) lang = "en"; 

                var course = await _context.Courses.FirstOrDefaultAsync(c => c.LanguageCode == lang);

                int courseId = course?.Id ?? 0;

                learnViewModel.LanguageCode = lang;
                learnViewModel.Coins = 0;
                learnViewModel.DayStreak = 0;
                learnViewModel.LanguageLessonGroups = await _context.Groups
                    .Where(g => g.LanguageCourseId == courseId)
                    .Include(g => g.Lessons)
                    .ToListAsync();
            }

            return View(learnViewModel);
        }
    }
}