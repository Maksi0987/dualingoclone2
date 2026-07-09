namespace Languio.Models
{
    public class LanguageCourse
    {
        public int Id { get; set; }
        public string LanguageCode { get; set; }
        public string Title { get; set; }
        public List<LanguageLesson> Lessons { get; set; } = new();
    }
}
