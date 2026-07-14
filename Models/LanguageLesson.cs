using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Languio.Models
{
    public class LanguageLesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }

        public int LanguageLessonGroupId { get; set; }
        [ValidateNever]
        public LanguageLessonGroup LanguageGroup { get; set; }

        public List<LanguageQuestion> Questions { get; set; } = new();
    }
}
