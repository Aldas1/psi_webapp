using QuizAppApi.DTOs;
using QuizAppApi.Utils;

namespace QuizAppApi.Interfaces;

public interface IAnswerService
{
    AnswerSubmitResponseDTO SubmitAnswers(int id, List<AnswerSubmitRequestDTO> request);
}