using System.ComponentModel.DataAnnotations;

namespace Languio.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Введіть ел. адресу")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        public string Email { get; set; }
    }
}