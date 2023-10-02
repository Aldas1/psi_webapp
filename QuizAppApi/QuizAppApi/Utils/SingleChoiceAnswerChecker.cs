using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Utils
{
    public static class SingleChoiceAnswerChecker
    {
        public static bool IsCorrect(SingleChoiceQuestion question, Option answer)
        {
            return answer.Name == question.CorrectOption.Name;
        }
    }
}