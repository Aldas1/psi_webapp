using QuizAppApi.Dtos;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Interfaces
{
    public interface IExplanationService
    {
        Task<string?> GenerateExplanationAsync(QuestionResponseDto question);
        Task<string?> GenerateExplanationAsync(SingleChoiceQuestion question);
        Task<string?> GenerateExplanationAsync(MultipleChoiceQuestion question);
        Task<string?> GenerateExplanationAsync(OpenTextQuestion question);
    }
}