using QuizAppApi.Models.Questions;

namespace QuizAppApi.Utils;

public static class MultipleChoiceAnswerChecker
{
    public static bool IsCorrect(MultipleChoiceQuestion question, ICollection<Option> answer)
    {
        var correctAnswers = new HashSet<Option>(question.Options.Where(opt => opt.Correct), new OptionEqualityComparer());
        return correctAnswers.SetEquals(answer);
    }
}