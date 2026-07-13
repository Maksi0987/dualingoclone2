using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Languio.Controllers
{
    [Authorize]
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
            var user = await _context.Users
                .Include(u => u.ActiveLanguageCourse)
                .Include(u => u.Progresses)
                    .ThenInclude(p => p.LanguageLesson)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            if (user == null) return Challenge();
            if (user.ActiveLanguageCourse == null) return RedirectToAction("LanguageChoice", "Home");

            if ((DateTime.UtcNow.Date - user.LastActiveDate.Date).TotalDays >= 2)
            {
                user.DayStreak = 0;
            }
            user.LastActiveDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var currentLangCode = string.IsNullOrEmpty(lang) ? user.ActiveLanguageCourse.LanguageCode : lang;
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.LanguageCode == currentLangCode);

            if (course == null) return NotFound();

            var groups = await _context.Groups
                .Where(g => g.LanguageCourseId == course.Id)
                .Include(g => g.Lessons)
                .OrderBy(g => g.Id)
                .ToListAsync();

            var userProgress = user.Progresses?.FirstOrDefault(p => p.LanguageCourseId == course.Id);

            if (userProgress == null)
            {
                var firstLesson = groups
                    .SelectMany(g => g.Lessons)
                    .OrderBy(l => l.Order)
                    .FirstOrDefault();

                userProgress = new UserProgress
                {
                    User = user,
                    LanguageCourseId = course.Id,
                    LanguageLesson = firstLesson
                };

                _context.UserProgresses.Add(userProgress);
                await _context.SaveChangesAsync();
            }

            var learnViewModel = new LearnViewModel
            {
                LanguageCode = currentLangCode,
                Coins = user.Coins,
                DayStreak = user.DayStreak,
                LanguageLessonGroups = groups,
                CurrentLanguageLesson = userProgress.LanguageLesson
            };

            return View(learnViewModel);
        }
    }
}