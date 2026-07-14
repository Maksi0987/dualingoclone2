using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Languio.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // КУРСИ
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        [HttpGet]
        public IActionResult CreateCourse() => View(new LanguageCourse());

        [HttpPost]
        public async Task<IActionResult> CreateCourse(LanguageCourse course)
        {
            if (!ModelState.IsValid) return View(course);

            course.LanguageCode = course.LanguageCode.Trim().ToLower();
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            // Виправлено: повертаємось на головну адмінки
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult EditCourse(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(LanguageCourse course)
        {
            if (!ModelState.IsValid) return View(course);
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            // Виправлено: була помилка ("Courses" замість "Index")
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        // ==========================================
        // ГРУПИ (МОДУЛІ)
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Groups(int courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null) return NotFound();

            var groups = await _context.Groups.Where(g => g.LanguageCourseId == courseId).ToListAsync();

            ViewBag.CourseId = courseId;
            ViewBag.CourseName = course.LanguageCode;
            return View(groups);
        }

        [HttpGet]
        public IActionResult CreateGroup(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View(new LanguageLessonGroup { LanguageCourseId = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(LanguageLessonGroup group)
        {
            if (!ModelState.IsValid) return View(group);
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return RedirectToAction("Groups", new { courseId = group.LanguageCourseId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLessonGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                int courseId = group.LanguageCourseId; // Запам'ятовуємо ID курсу до видалення
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();

                // Виправлено: тепер правильно повертає в список груп цього курсу
                return RedirectToAction("Groups", new { courseId = courseId });
            }
            return RedirectToAction(nameof(Index));
        }


        // ==========================================
        // УРОКИ
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Lessons(int groupId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null) return NotFound();

            var lessons = await _context.Lessons
                .Where(l => l.LanguageLessonGroupId == groupId)
                .OrderBy(l => l.Order)
                .ToListAsync();

            ViewBag.GroupId = groupId;
            ViewBag.CourseId = group.LanguageCourseId;
            ViewBag.GroupName = group.Title;
            return View(lessons);
        }

        [HttpGet]
        public IActionResult CreateLesson(int groupId)
        {
            ViewBag.GroupId = groupId;
            return View(new LanguageLesson { LanguageLessonGroupId = groupId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson(LanguageLesson lesson)
        {
            if (!ModelState.IsValid) return View(lesson);

            var group = await _context.Groups
                .FirstOrDefaultAsync(g => g.Id == lesson.LanguageLessonGroupId);

            if (group == null) return NotFound();

            var maxOrder = await _context.Lessons
                .Where(l => l.LanguageGroup.LanguageCourseId == group.LanguageCourseId)
                .Select(l => (int?)l.Order)
                .MaxAsync() ?? 0;

            lesson.Order = maxOrder + 1;

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            return RedirectToAction("Lessons", new { groupId = lesson.LanguageLessonGroupId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson != null)
            {
                int groupId = lesson.LanguageLessonGroupId; // Запам'ятовуємо ID групи
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();

                // Виправлено: тепер правильно повертає в список уроків цієї групи
                return RedirectToAction("Lessons", new { groupId = groupId });
            }
            return RedirectToAction(nameof(Index));
        }


        // ==========================================
        // ПИТАННЯ
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Questions(int lessonId)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId);
            if (lesson == null) return NotFound();

            var questions = await _context.Questions
                .Include(q => q.Options)
                .Where(q => q.LanguageLessonId == lessonId)
                .ToListAsync();

            ViewBag.LessonId = lesson.Id;
            ViewBag.LessonTitle = lesson.Title;
            ViewBag.GroupId = lesson.LanguageLessonGroupId;

            return View(questions);
        }

        [HttpGet]
        public IActionResult CreateQuestion(int lessonId)
        {
            return View(new CreateQuestionViewModel { LessonId = lessonId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(CreateQuestionViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var question = new LanguageQuestion
            {
                LanguageLessonId = model.LessonId,
                PromptText = model.PromptText,
                Type = model.Type,
                Options = model.Options.Select(o => new AnswerOption
                {
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }).ToList()
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return RedirectToAction("Questions", new { lessonId = model.LessonId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                int lessonId = question.LanguageLessonId; // Запам'ятовуємо ID уроку
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();

                // Виправлено: тепер правильно повертає в список питань цього уроку
                return RedirectToAction("Questions", new { lessonId = lessonId });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}