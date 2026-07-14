using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Languio.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        // Додано AppDbContext у конструктор
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Courses = _context.Courses.ToList();
            return View();
        }

        [HttpGet] public IActionResult Login() => View();
        [HttpGet] public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = _context.Courses.ToList();
                return View(model);
            }

            // Шукаємо обраний курс разом із його структурою
            var course = await _context.Courses
                .Include(c => c.Groups)
                    .ThenInclude(g => g.Lessons)
                .FirstOrDefaultAsync(c => c.LanguageCode == model.SelectedLanguageCode);

            if (course == null)
            {
                ModelState.AddModelError(string.Empty, "Обраний курс не знайдено.");
                ViewBag.Courses = _context.Courses.ToList();
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                ActiveLanguageCourseId = course.Id // Відразу встановлюємо курс користувачу
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Налаштування початкового прогресу
                var firstLesson = course.Groups
                    .OrderBy(g => g.Id)
                    .SelectMany(g => g.Lessons)
                    .OrderBy(l => l.Order)
                    .FirstOrDefault();

                if (firstLesson != null)
                {
                    var newProgress = new UserProgress
                    {
                        UserId = user.Id,
                        LanguageCourseId = course.Id,
                        LanguageLessonId = firstLesson.Id
                    };
                    _context.UserProgresses.Add(newProgress);
                    await _context.SaveChangesAsync();
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                // Одразу переходимо до навчання, оскільки мова вже вибрана!
                return RedirectToAction("Index", "Learn");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            ViewBag.Courses = _context.Courses.ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _signInManager.PasswordSignInAsync(model.EmailOrUsername, model.Password, model.RememberMe, false);
            if (result.Succeeded) return RedirectToAction("Index", "Learn");
            ModelState.AddModelError("", "Невірний логін або пароль");
            return View(model);
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) return RedirectToAction("Login");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded) return RedirectToAction("Index", "Learn");

            var email = info.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email).Value;
            var user = new ApplicationUser { UserName = email, Email = email };

            var createResult = await _userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                // При вході через Google користувач ще не вибрав мову, тому перекидаємо на LanguageChoice
                return RedirectToAction("LanguageChoice", "Home");
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}