namespace Languio.Models
{
    public class LearnViewModel
    {
        public string LanguageCode { get; set; }
        public int Coins { get; set; } = 0;
        public int DayStreak { get; set; } = 0;
        public List<LanguageLessonGroup> LanguageLessonGroups { get; set; } = new();
        public LearnViewModel()
        {
            
        }
    }
}
