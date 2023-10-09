using QuizAppApi.Enums;

namespace QuizAppApi.Models.Questions
{
    public class SingleChoiceQuestion : Question
    {
        public ICollection<Option> Options { get; set; } = new List<Option>();
        public Option CorrectOption { get; set; } = new Option();
        public override QuestionType Type { get => QuestionType.SingleChoiceQuestion; }
    }
}
