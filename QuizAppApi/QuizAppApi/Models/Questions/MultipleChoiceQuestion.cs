using QuizAppApi.Enums;

namespace QuizAppApi.Models.Questions
{
    public class MultipleChoiceQuestion : OptionQuestion
    {
        public override QuestionType Type { get => QuestionType.MultipleChoiceQuestion; }
    }
}
