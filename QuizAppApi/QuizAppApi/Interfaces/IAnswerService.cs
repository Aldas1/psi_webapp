using QuizAppApi.Dtos;
using QuizAppApi.Utils;

namespace QuizAppApi.Interfaces;

public interface IAnswerService
{
    Task<AnswerSubmitResponseDto> SubmitAnswersAsync(int id, List<AnswerSubmitRequestDto> request, string? username = null);
}