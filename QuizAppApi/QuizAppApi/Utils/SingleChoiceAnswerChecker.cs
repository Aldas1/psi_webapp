using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Utils
{
    public class SingleChoiceAnswerChecker : IAnswerChecker<SingleChoiceQuestion,Option>
    {
        public bool IsCorrect(SingleChoiceQuestion question, Option answer)
        {
            throw new NotImplementedException();
        }
    }
}