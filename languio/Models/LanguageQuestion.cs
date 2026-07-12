namespace Languio.Models
{
    public enum QuestionType
    {
        MultipleChoice,
        Translation,
        MatchingPairs
    }

    public class LanguageQuestion
    {
        public int Id { get; set; }
        public int LanguageLessonId { get; set; }

        public string PromptText { get; set; }

        public QuestionType Type { get; set; }

        public ICollection<AnswerOption> Options { get; set; }
    }

    public class AnswerOption
    {
        public int Id { get; set; }
        public int LanguageQuestionId { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}
