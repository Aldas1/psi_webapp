using QuizAppApi.Enums;

namespace QuizAppApi.Models
{
    public abstract class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public abstract QuestionType Type { get; }
    }
}
