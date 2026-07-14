using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Languio.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SettingsController(AppDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Account()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            ViewBag.Email = user.Email;
            ViewBag.EmailConfirmed = user.EmailConfirmed;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Profile(string email, string currentPassword, string newPassword)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!string.IsNullOrWhiteSpace(email) && email != user.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, email);
                if (!setEmailResult.Succeeded)
                {
                    foreach (var error in setEmailResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                else
                {
                    await _userManager.SetUserNameAsync(user, email);
                }
            }

            if (!string.IsNullOrWhiteSpace(currentPassword) && !string.IsNullOrWhiteSpace(newPassword))
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Email = user.Email;
                ViewBag.EmailConfirmed = user.EmailConfirmed;
                return View(user);
            }

            await _signInManager.RefreshSignInAsync(user);

            TempData["StatusMessage"] = "Зміни збережено.";
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public IActionResult DeleteAccount()
        {
            return View();
        }

        [HttpPost]
        [ActionName("DeleteAccount")]
        public async Task<IActionResult> DeleteAccountConfirmed(string password)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var passwordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordValid)
            {
                ModelState.AddModelError(string.Empty, "Невірний пароль.");
                return View();
            }

            var progresses = _context.UserProgresses.Where(p => p.User.Id == user.Id);
            _context.UserProgresses.RemoveRange(progresses);
            await _context.SaveChangesAsync();

            await _signInManager.SignOutAsync();
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Courses()
        {
            var userId = _userManager.GetUserId(User);

            var progresses = await _context.UserProgresses
                .Where(p => p.User.Id == userId)
                .Include(p => p.Course)
                .ToListAsync();

            // Фільтрація старих дублікатів
            var distinctProgresses = progresses
                .GroupBy(p => p.LanguageCourseId)
                .Select(g => g.First())
                .ToList();

            return View(distinctProgresses);
        }

        [HttpPost]
        public async Task<IActionResult> ResetCourse(int courseId)
        {
            var userId = _userManager.GetUserId(User);

            var progress = await _context.UserProgresses
                .Include(p => p.Course)
                    .ThenInclude(c => c.Groups)
                        .ThenInclude(g => g.Lessons)
                .FirstOrDefaultAsync(p => p.User.Id == userId && p.LanguageCourseId == courseId);

            if (progress == null) return NotFound();

            var firstLesson = progress.Course.Groups
                .SelectMany(g => g.Lessons)
                .OrderBy(l => l.Order)
                .FirstOrDefault();

            progress.LanguageLesson = firstLesson;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Courses));
        }
    }
}