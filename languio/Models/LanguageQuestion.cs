namespace Languio.Models
{
    public enum QuestionType
    {
        MultipleChoice,
        FillInTheBlank,
        TrueFalse,
        Matching,
        ShortAnswer
    }
    public class LanguageQuestion
    {
        public int Id { get; set; }
        public QuestionType Type{ get; set; }
        public int Difficulty { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public string OptinsRaw { get; set; }

        public int LanguageLessonId { get; set; }
        public LanguageLesson LanguageLesson { get; set; }
    }
}
