using QuizAppApi.Models.Questions;

namespace QuizAppApi.Utils
{
    public static class SingleChoiceAnswerChecker
    {
        public static bool IsCorrect(SingleChoiceQuestion question, Option answer, bool lowercaseComparison = false)
        {
            if (lowercaseComparison)
            {
                return answer.Name.ToLower() == question.CorrectOption.Name.ToLower();
            }
            return answer.Name == question.CorrectOption.Name;
        }
    }
}