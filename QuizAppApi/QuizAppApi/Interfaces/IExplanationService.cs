using QuizAppApi.Models.Questions;

namespace QuizAppApi.Interfaces
{
    public interface IExplanationService
    {
        Task<string?> GenerateExplanationAsync(SingleChoiceQuestion question, string? chosenOption, bool correct);

        Task<string?> GenerateExplanationAsync(MultipleChoiceQuestion question, List<string> chosenOptions, bool correct);

        Task<string?> GenerateExplanationAsync(OpenTextQuestion question, string? answerText, bool correct);
    }
}