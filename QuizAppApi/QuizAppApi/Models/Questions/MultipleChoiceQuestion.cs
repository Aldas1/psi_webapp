using QuizAppApi.Enums;

namespace QuizAppApi.Models.Questions
{
    public class MultipleChoiceQuestion : Question
    {
        public ICollection<Option> Options { get; set; }
        public ICollection<Option> CorrectOptions { get; set; }

        public override QuestionType Type { get => QuestionType.MultipleChoiceQuestion; }
    }
}
