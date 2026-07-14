using Languio.Data;
using Languio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        // ДОПОМІЖНИЙ МЕТОД ДЛЯ ГЕНЕРАЦІЇ ПАРОЛЯ
        private string GenerateRandomPassword()
        {
            var random = new Random();
            const string upper = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijkmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string nonAlphanumeric = "!@$?_-";

            // Обов'язково додаємо по одному символу кожного типу, щоб Identity пропустив пароль
            string password =
                upper[random.Next(upper.Length)].ToString() +
                lower[random.Next(lower.Length)].ToString() +
                digits[random.Next(digits.Length)].ToString() +
                nonAlphanumeric[random.Next(nonAlphanumeric.Length)].ToString();

            // Добиваємо ще 4 випадкових символи (загальна довжина буде 8)
            const string allChars = upper + lower + digits + nonAlphanumeric;
            for (int i = 0; i < 4; i++)
            {
                password += allChars[random.Next(allChars.Length)];
            }

            // Перемішуємо символи випадковим чином
            return new string(password.OrderBy(x => random.Next()).ToArray());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Введіть електронну пошту.");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // 1. Генеруємо новий випадковий пароль
                string newPassword = GenerateRandomPassword();

                // 2. Отримуємо токен і примусово змінюємо пароль у базі даних
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (resetResult.Succeeded)
                {
                    // 3. Відправляємо лист із новим готовим паролем
                    try
                    {
                        var myEmail = "mfxmaxfair@gmail.com";
                        var appPassword = "umwm txwk vmaa yvho";

                        using var smtp = new System.Net.Mail.SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new System.Net.NetworkCredential(myEmail, appPassword)
                        };

                        var fromAddress = new System.Net.Mail.MailAddress(myEmail, "Languio Support");
                        var toAddress = new System.Net.Mail.MailAddress(email);

                        using var message = new System.Net.Mail.MailMessage(fromAddress, toAddress)
                        {
                            Subject = "Ваш новий пароль на Languio",
                            Body = $"<h3>Привіт!</h3>" +
                                   $"<p>Ваш пароль на платформі Languio було успішно скинуто.</p>" +
                                   $"<p>Ваш новий пароль для входу: <strong style='font-size: 18px; color: #0d6efd;'>{newPassword}</strong></p>" +
                                   $"<p>Ви можете використовувати його для входу просто зараз.</p>",
                            IsBodyHtml = true
                        };

                        await smtp.SendMailAsync(message);
                    }
                    catch (System.Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Помилка відправки листа: " + ex.Message);
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Сталася помилка при скиданні пароля в системі.");
                    return View();
                }
            }

            TempData["StatusMessage"] = "Якщо така пошта існує в системі, ми відправили на неї новий пароль.";
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = _context.Courses.ToList();
                return View(model);
            }

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
                ActiveLanguageCourseId = course.Id
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
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