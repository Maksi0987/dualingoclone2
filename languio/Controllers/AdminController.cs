using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Mvc;
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
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View(new LanguageCourse());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(LanguageCourse course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            if (string.IsNullOrWhiteSpace(course.LanguageCode))
            {
                ModelState.AddModelError("", "Код мови обов'язковий!");
                return View(course);
            }

            course.LanguageCode = course.LanguageCode.Trim().ToLower();

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult CreateGroup(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View(new LanguageLessonGroup());
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(LanguageLessonGroup group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return RedirectToAction("Groups", new { courseId = group.LanguageCourseId });
        }

        [HttpGet]
        public IActionResult CreateLesson(int groupId)
        {
            ViewBag.GroupId = groupId;
            return View(new LanguageLesson());
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson(LanguageLesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction("Lessons", new { groupId = lesson.LanguageLessonGroupId });
        }

        [HttpGet]
        public IActionResult CreateQuestion(int lessonId)
        {
            var model = new CreateQuestionViewModel { LessonId = lessonId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(CreateQuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

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
        [HttpGet]
        public async Task<IActionResult> Groups(int courseId)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null) return NotFound();

            var groups = await _context.Groups
                .Where(g => g.LanguageCourseId == courseId)
                .ToListAsync();

            ViewBag.CourseId = courseId;
            ViewBag.CourseName = course.LanguageCode;
            return View(groups);
        }

        [HttpGet]
        public async Task<IActionResult> Lessons(int groupId)
        {
            var group = await _context.Groups
                .FirstOrDefaultAsync(g => g.Id == groupId);

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
        public async Task<IActionResult> Questions(int lessonId)
        {
            var lesson = await _context.Lessons
                .Include(l => l.LanguageGroup)
                .FirstOrDefaultAsync(l => l.Id == lessonId);

            if (lesson == null)
            {
                return NotFound("Урок не знайдено");
            }

            var questions = await _context.Questions
                .Include(q => q.Options)
                .Where(q => q.LanguageLessonId == lessonId)
                .ToListAsync();

            ViewBag.LessonId = lesson.Id;
            ViewBag.LessonTitle = lesson.Title;
            ViewBag.GroupId = lesson.LanguageLessonGroupId;

            return View(questions);
        }

    }
}