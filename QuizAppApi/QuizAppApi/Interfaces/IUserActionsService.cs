using QuizAppApi.DTOs;
using QuizAppApi.Utils;

namespace QuizAppApi.Interfaces;

public delegate void AnswerSubmittedEventHandler(object sender, AnswerSubmittedEventArgs args);

public interface IUserActionsService
{
    event AnswerSubmittedEventHandler AnswerSubmitted;
    AnswerSubmitResponseDTO SubmitAnswers(int id, List<AnswerSubmitRequestDTO> request);
}