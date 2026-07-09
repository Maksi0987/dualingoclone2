namespace Languio.Models
{
    public class LanguageLessonGroup
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<LanguageLesson> Lessons { get; set; }

        public int LanguageCourseId { get; set; }
    }
}
