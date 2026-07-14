using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Languio.Controllers
{
    [Authorize]
    public class LessonController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LessonController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Start(int id)
        {
            var user = await _context.Users
                .Include(u => u.Progresses)
                    .ThenInclude(p => p.LanguageLesson)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            if (user == null) return Challenge();

            var lesson = await _context.Lessons
                .Include(l => l.Questions)
                    .ThenInclude(q => q.Options)
                .Include(l => l.LanguageGroup)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null) return NotFound();

            var progress = user.Progresses
                .FirstOrDefault(p => p.LanguageCourseId == lesson.LanguageGroup.LanguageCourseId);

            if (progress == null || progress.LanguageLesson == null)
            {
                if (lesson.Order != 1)
                    return RedirectToAction("Index", "Learn");
            }
            else if (lesson.Order > progress.LanguageLesson.Order)
            {
                return RedirectToAction("Index", "Learn");
            }

            if (lesson.Questions == null || !lesson.Questions.Any())
                return BadRequest("В цьому уроці ще немає питань.");

            foreach (var question in lesson.Questions)
            {
                question.Options = question.Options.OrderBy(o => Guid.NewGuid()).ToList();
            }

            return View("Lesson", lesson);
        }

        [HttpPost]
        public async Task<IActionResult> CheckAnswer(int questionId, int selectedOptionId)
        {
            var option = await _context.AnswerOptions
                .FirstOrDefaultAsync(o => o.Id == selectedOptionId && o.LanguageQuestionId == questionId);

            if (option == null) return BadRequest(new { message = "Невірні дані." });

            return Json(new { isCorrect = option.IsCorrect });
        }

        [HttpPost]
        public async Task<IActionResult> CompleteLesson(int lessonId)
        {
            var user = await _context.Users.Include(u => u.Progresses).FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            var currentLesson = await _context.Lessons.Include(l => l.LanguageGroup).FirstOrDefaultAsync(l => l.Id == lessonId);

            if (user != null && currentLesson != null)
            {
                user.Experience += 10;
                user.Coins += 5;

                var nextLesson = await _context.Lessons
                    .Where(l => l.LanguageGroup.LanguageCourseId == currentLesson.LanguageGroup.LanguageCourseId)
                    .OrderBy(l => l.Order)
                    .FirstOrDefaultAsync(l => l.Order > currentLesson.Order);

                var progress = user.Progresses.FirstOrDefault(p => p.LanguageCourseId == currentLesson.LanguageGroup.LanguageCourseId);

                if (progress != null)
                {
                    progress.LanguageLesson = nextLesson ?? currentLesson;
                }
                await _context.SaveChangesAsync();

                if (user.LastActiveDate.Date < DateTime.UtcNow.Date)
                {
                    user.DayStreak += 1;
                }
                user.LastActiveDate = DateTime.UtcNow;

                await _userManager.UpdateAsync(user);
            }
            return Ok();
        }
    }
}