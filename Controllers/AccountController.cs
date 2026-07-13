using Languio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Languio.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet] public IActionResult Register() => View();
        [HttpGet] public IActionResult Login() => View();
        [HttpGet] public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Learn");
            }
            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
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
            var result = await _signInManager.ExternalLoginAsync(info, false);
            if (result.Succeeded) return RedirectToAction("Index", "Learn");

            var email = info.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email).Value;
            var user = new ApplicationUser { UserName = email, Email = email };
            await _userManager.CreateAsync(user);
            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Learn");
        }
    }
}