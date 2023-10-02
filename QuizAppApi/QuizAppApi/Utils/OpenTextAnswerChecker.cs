using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Utils
{
    public static class OpenTextAnswerCheker
    {
        public static bool IsCorrect(OpenTextQuestion question, string answer)
        {
            return question.CorrectAnswer == answer;
        }
    }
}