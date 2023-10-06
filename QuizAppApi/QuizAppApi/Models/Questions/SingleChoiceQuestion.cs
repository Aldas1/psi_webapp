using QuizAppApi.Enums;

namespace QuizAppApi.Models.Questions
{
    public class SingleChoiceQuestion : Question
    {
        public ICollection<Option> Options { get; set; }
        public Option CorrectOption { get; set; }
        public override QuestionType Type { get => QuestionType.SingleChoiceQuestion; }
    }
}
