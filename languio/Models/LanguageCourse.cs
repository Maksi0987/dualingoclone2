namespace Languio.Models
{
    public class LanguageCourse
    {
        public int Id { get; set; }
        public string LanguageCode { get; set; }
        public List<LanguageLessonGroup> Groups { get; set; } = new();
    }
}
