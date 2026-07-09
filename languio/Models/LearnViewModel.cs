namespace Languio.Models
{
    public class LearnViewModel
    {
        public string LanguageCode { get; set; }
        public string ActiveSectionTitle { get; set; }
        public string ActiveSectionDescription { get; set; }

        public List<LessonMapItemViewModel> MapItems { get; set; } = new();
    }

    public class LessonMapItemViewModel
    {
        public int LessonId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
    }
}
