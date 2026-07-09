namespace Languio.Models
{
    public class LanguageLesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }

        public int LanguageCoruseId { get; set; }
        public LanguageCourse LanguageCourse { get; set; }

        public List<LanguageQuestion> Questions { get; set; }
    }
}
