using Languio.Models;

namespace Languio.Models
{
    public class UserProgress
    {
        public int Id { get; set; }

       
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; }

        public int LanguageCourseId { get; set; }
        public LanguageCourse Course { get; set; }

        public int LanguageLessonId { get; set; }
        public LanguageLesson LanguageLesson { get; set; }
    }
}
