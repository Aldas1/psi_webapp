using QuizAppApi.Enums;

namespace QuizAppApi.Models.Questions
{
    public class SingleChoiceQuestion : OptionQuestion
    {
        public override QuestionType Type { get => QuestionType.SingleChoiceQuestion; }
    }
}
