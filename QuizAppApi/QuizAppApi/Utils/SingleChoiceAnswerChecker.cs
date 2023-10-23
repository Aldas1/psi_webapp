using QuizAppApi.Models.Questions;

namespace QuizAppApi.Utils;

public static class SingleChoiceAnswerChecker
{
    public static bool IsCorrect(SingleChoiceQuestion question, string answer)
    {
        return answer == question.Options.First(o => o.Correct).Name;
    }
}