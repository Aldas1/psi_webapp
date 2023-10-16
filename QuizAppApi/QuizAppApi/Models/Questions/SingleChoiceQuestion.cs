using QuizAppApi.Enums;

namespace QuizAppApi.Models.Questions
{
    public class SingleChoiceQuestion : Question
    {
        public override QuestionType Type { get => QuestionType.SingleChoiceQuestion; }
        public string CorrectOptionName { get; set; }
        
        public virtual ICollection<SingleChoiceOption> SingleChoiceOptions { get; set; } = new List<SingleChoiceOption>();
    }
}
