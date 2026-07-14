using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Languio.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public StoreController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> BuyStreakFreeze()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (user.HasStreakFreeze)
            {
                TempData["ErrorMessage"] = "У вас вже є активна заморозка!";
            }
            else if (user.Coins < 10)
            {
                TempData["ErrorMessage"] = "Недостатньо лінгвотів (монет) для покупки!";
            }
            else
            {
                // Списуємо монети і видаємо предмет
                user.Coins -= 10;
                user.HasStreakFreeze = true;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Ви успішно придбали 'Заморозку серії'!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Помилка при покупці. Спробуйте пізніше.";
                }
            }

            return RedirectToAction("Index");
        }
    }
}