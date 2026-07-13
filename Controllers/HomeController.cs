using Languio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Languio.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Learn", new { lang = Request.Cookies["lang"] });
            }
            return View();
        }
        [HttpGet()]
        public IActionResult LanguageChoice()
        {
            var model = new ChooseLanguageViewModel
            {
                Languages = new List<LanguageCardViewModel>
            {
                new() { Code = "en", Name = "Англійська", FlagEmoji = "🇺🇸" },
                new() { Code = "de", Name = "Німецька", FlagEmoji = "🇩🇪" },
                new() { Code = "es", Name = "Іспанська", FlagEmoji = "🇪🇸" },
                new() { Code = "fr", Name = "Французька", FlagEmoji = "🇫🇷" },
                new() { Code = "it", Name = "Італійська", FlagEmoji = "🇮🇹" },
                new() { Code = "ja", Name = "Японська", FlagEmoji = "🇯🇵" },
                new() { Code = "ko", Name = "Корейська", FlagEmoji = "🇰🇷" },
                new() { Code = "zh", Name = "Китайська (спрощена)", FlagEmoji = "🇨🇳" }
            }
            };

            return View(model);
        }
        [HttpPost("LanguageChoice")]
        public IActionResult SelectLanguage(string lang)
        {
            return RedirectToAction("Index", "Learn", new { lang = lang });
        }
    }
}
