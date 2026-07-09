using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Languio.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Groups)
                    .ThenInclude(g => g.Lessons)
                .ToListAsync();

            return View(courses);
        }
        [HttpGet]
        public ActionResult CreateCourse()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CreateLesson()
        {
            var courses = await _context.Courses.ToListAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");

            ViewBag.Groups = new SelectList(new List<LanguageLessonGroup>(), "Id", "Title");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CreateGroup()
        {
            var courses = await _context.Courses.ToListAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");
            return View();
        }
        [HttpPost("Admin/CreateCourse")]
        public async Task<ActionResult> CreateCourse([Bind("Title,LanguageCode")] LanguageCourse course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(course);
        }
        [HttpPost("Admin/CreateLesson")]
        public async Task<IActionResult> CreateLesson(LanguageLesson lesson, int languageLessonGroupId)
        {
            lesson.LanguageLessonGroupId = languageLessonGroupId;

            lesson.Order = await _context.Lessons
                .Where(l => l.LanguageLessonGroupId == languageLessonGroupId)
                .CountAsync() + 1;

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

            var courses = await _context.Courses.ToListAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");
            return View(lesson);
        }
        [HttpPost("Admin/CreateGroup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup(LanguageLessonGroup group, int languageCourseId)
        {
            var course = await _context.Courses.Include(c => c.Groups).FirstOrDefaultAsync(c => c.Id == languageCourseId);
            group.Title = $"Модуль {_context.Groups.Count(g => g.LanguageCourseId == languageCourseId) + 1}";

            if (course != null)
            {
                course.Groups.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var courses = await _context.Courses.ToListAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");
            return View(group);
        }
        [HttpGet("Admin/GetGroupsByCourse/{courseId}")]
        public async Task<IActionResult> GetGroupsByCourse(int courseId)
        {
            var groups = await _context.Groups
                .Where(g => g.LanguageCourseId == courseId)
                .Select(g => new { id = g.Id, title = g.Title })
                .ToListAsync();

            return Json(groups);
        }
        [HttpPost("Admin/DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        [HttpPost("Admin/DeleteLesson/{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
