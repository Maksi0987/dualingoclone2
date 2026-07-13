using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Languio.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Learn");
            }
            return View();
        }

        [HttpGet]
        public IActionResult LanguageChoice()
        {
            var model = new ChooseLanguageViewModel
            {
                Languages = _context.Courses.Select(c => new LanguageCardViewModel
                {
                    Code = c.LanguageCode,
                    Name = c.Name,
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SelectLanguage(string lang)
        {
            var user = await _context.Users
                .Include(u => u.Progresses)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            var course = await _context.Courses
                .Include(c => c.Groups)
                    .ThenInclude(g => g.Lessons)
                .FirstOrDefaultAsync(c => c.LanguageCode == lang);

            if (user != null && course != null)
            {
                user.ActiveLanguageCourse = course;

                var progress = user.Progresses.FirstOrDefault(p => p.LanguageCourseId == course.Id);

                if (progress == null)
                {
                    var firstLesson = course.Groups
                        .OrderBy(g => g.Id)
                        .SelectMany(g => g.Lessons)
                        .OrderBy(l => l.Order)
                        .FirstOrDefault();

                    var newProgress = new UserProgress
                    {
                        User = user,
                        Course = course,
                        LanguageCourseId = course.Id,
                        LanguageLesson = firstLesson
                    };

                    _context.UserProgresses.Add(newProgress);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Learn", new { lang = lang });
        }
        [Route("Home/Error")]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue && statusCode.Value == 404)
            {
                return View("NotFound");
            }

            return View("Error");
        }
    }
}