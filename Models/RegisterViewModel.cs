using System.ComponentModel.DataAnnotations;

namespace Languio.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ел. пошта обов'язкова")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Оберіть мову для вивчення")]
        public string SelectedLanguageCode { get; set; }
    }
}