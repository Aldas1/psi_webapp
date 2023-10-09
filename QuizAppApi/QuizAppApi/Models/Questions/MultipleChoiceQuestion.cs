using QuizAppApi.Enums;

namespace QuizAppApi.Models.Questions
{
    public class MultipleChoiceQuestion : Question
    {
        public ICollection<Option> Options { get; set; } = new List<Option>();
        public ICollection<Option> CorrectOptions { get; set; } = new List<Option>();

        public override QuestionType Type { get => QuestionType.MultipleChoiceQuestion; }
    }
}
