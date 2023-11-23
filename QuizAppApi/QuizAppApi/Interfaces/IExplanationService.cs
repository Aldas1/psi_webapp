using QuizAppApi.Models.Questions;

namespace QuizAppApi.Interfaces
{
    public interface IExplanationService
    {
        Task<string?> GenerateExplanationAsync(SingleChoiceQuestion question, string? chosenOption);

        Task<string?> GenerateExplanationAsync(MultipleChoiceQuestion question, List<string> chosenOptions);

        Task<string?> GenerateExplanationAsync(OpenTextQuestion question, string? answerText);

        Task<string?> GenerateCommentExplanationAsync(string explainQuery);
    }
}