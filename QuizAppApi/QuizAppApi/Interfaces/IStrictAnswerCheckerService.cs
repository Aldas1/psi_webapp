using QuizAppApi.Models.Questions;

namespace QuizAppApi.Interfaces
{
    public interface IStrictAnswerCheckerService
    {
        bool CheckSingleChoiceAnswer(SingleChoiceQuestion question, string answer);
        bool CheckMultipleChoiceAnswer(MultipleChoiceQuestion question, ICollection<Option> answer);
        bool CheckOpenTextAnswer(OpenTextQuestion question, string answer);
    }
}
