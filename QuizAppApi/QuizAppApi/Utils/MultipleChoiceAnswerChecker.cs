using QuizAppApi.Models.Questions;

namespace QuizAppApi.Utils
{
    public static class MultipleChoiceAnswerChecker
    {
        public static bool IsCorrect(MultipleChoiceQuestion question, ICollection<MultipleChoiceOption> answer)
        {
            var correctAnswers = new HashSet<MultipleChoiceOption>(question.MultipleChoiceOptions.Where(opt => opt.Correct), new MultipleChoiceOptionEqualityComparer());
            return correctAnswers.SetEquals(answer);
        }
    }
}