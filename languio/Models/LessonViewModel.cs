namespace Languio.Models
{
    public class LessonViewModel
    {
        public int LessonId { get; set; }
        public List<QuestionViewModel> Questions { get; set; } = new();
    }

    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string PromptText { get; set; }
        public string Type { get; set; }
        public List<AnswerOptionViewModel> Options { get; set; } = new();
    }

    public class AnswerOptionViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
