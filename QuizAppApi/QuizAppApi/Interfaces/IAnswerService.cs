using QuizAppApi.Dtos;
using QuizAppApi.Utils;

namespace QuizAppApi.Interfaces;

public interface IAnswerService
{
    AnswerSubmitResponseDto SubmitAnswers(int id, List<AnswerSubmitRequestDto> request);
}