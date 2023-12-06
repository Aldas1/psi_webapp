using QuizAppApi.Models.Questions;

namespace QuizAppApi.Interfaces;
public interface IExplanationService
{
    Task<string?> GenerateExplanationAsync(SingleChoiceQuestion question);
    Task<string?> GenerateExplanationAsync(MultipleChoiceQuestion question);
    Task<string?> GenerateExplanationAsync(OpenTextQuestion question);
    Task<string?> GenerateCommentExplanationAsync(string? userComment);
}