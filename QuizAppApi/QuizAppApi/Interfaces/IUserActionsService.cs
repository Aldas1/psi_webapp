using QuizAppApi.DTOs;
using QuizAppApi.Utils;

namespace QuizAppApi.Interfaces;

public interface IUserActionsService
{
    AnswerSubmitResponseDTO SubmitAnswers(int id, List<AnswerSubmitRequestDTO> request);
}