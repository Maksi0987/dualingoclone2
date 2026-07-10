using Microsoft.AspNetCore.Identity;

namespace Languio.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Кастомні поля для навчання
        public int Coins { get; set; } = 0; // Баланс монеток
        public int Experience { get; set; } = 0; // Досвід / Прогрес
        public int StreakDays { get; set; } = 0; // Ударний режим (скільки днів поспіль заходив)
    }
}