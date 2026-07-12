using languio.Models;

namespace Languio.Models
{
    public class UserProgress
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public int LanguageCourseId { get; set; }
        public LanguageCourse Course { get; set; }
        public int CurrentLessonId { get; set; }
    }
}
