using QuizAppApi.Models.Questions;

namespace QuizAppApi.Utils
{
    public static class OpenTextAnswerChecker
    {
        public static bool IsCorrect(OpenTextQuestion question, string answer)
        {
            return question.CorrectAnswer == answer;
        }
    }
}