using QuizAppApi.Enums;

namespace QuizAppApi.Models.Questions
{
    public class MultipleChoiceQuestion : Question
    {
        public override QuestionType Type { get => QuestionType.MultipleChoiceQuestion; }
        
        public virtual ICollection<MultipleChoiceOption> MultipleChoiceOptions { get; set; } = new List<MultipleChoiceOption>();
    }
}
