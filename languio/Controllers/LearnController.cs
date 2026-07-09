using Languio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Languio.Controllers
{
    public class LearnController : Controller
    {
        [HttpGet]
        public IActionResult Index(string lang)
        {
            var viewModel = new LearnViewModel
            {
                LanguageCode = lang,
                ActiveSectionTitle = "ЧАСТИНА 1, РОЗДІЛ 1",
                ActiveSectionDescription = "Зробіть замовлення в кафе",
                MapItems = new List<LessonMapItemViewModel>
        {
            new() { LessonId = 1, Order = 1, Type = "Lesson", Status = "Active" }
        }
            };
            return View(viewModel);
        }
    }
}
