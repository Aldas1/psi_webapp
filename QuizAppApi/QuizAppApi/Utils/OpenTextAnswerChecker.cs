using QuizAppApi.Models.Questions;

namespace QuizAppApi.Utils
{
    public static class OpenTextAnswerChecker
    {
        public static bool IsCorrect(OpenTextQuestion question, string answer, bool useLowercaseComparison = false, bool trimWhitespace = false)
        {
            var a = trimWhitespace ? answer.Trim() : answer;
            var b = trimWhitespace ? question.CorrectAnswer.Trim() : question.CorrectAnswer;
            if (useLowercaseComparison)
            {
                return a == b;
            }
            return question.CorrectAnswer == answer;
        }
    }
}