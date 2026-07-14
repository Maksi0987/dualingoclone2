using System.ComponentModel.DataAnnotations;

namespace Languio.Models
{
    public class LoginViewModel
    {
        [Required] public string EmailOrUsername { get; set; }
        [Required][DataType(DataType.Password)] public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}