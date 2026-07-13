using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class LessonController : Controller
{
    private readonly AppDbContext _context;

    public LessonController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Start(int id)
    {
        var lesson = await _context.Lessons
            .Include(l => l.Questions)
                .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lesson == null)
        {
            return NotFound("Урок не знайдено.");
        }

        if (lesson.Questions == null || !lesson.Questions.Any())
        {
            return BadRequest("В цьому уроці ще немає питань.");
        }

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

        if (option == null)
        {
            return BadRequest(new { message = "Невірні дані запиту." });
        }

        return Json(new { isCorrect = option.IsCorrect });
    }
}