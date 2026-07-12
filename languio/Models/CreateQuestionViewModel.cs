namespace Languio.Models
{
    public class CreateQuestionViewModel
    {
        public int LessonId { get; set; }
        public string PromptText { get; set; }
        public QuestionType Type { get; set; }

        public List<CreateOptionViewModel> Options { get; set; } = new();
    }

    public class CreateOptionViewModel
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
