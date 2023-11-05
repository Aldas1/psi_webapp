using QuizAppApi.Dtos;
using QuizAppApi.Dtos.Responses;
using QuizAppApi.Dtos.Requests;

namespace QuizAppApi.Interfaces;
public interface IQuizService
{
    QuizManipulationResponseDto CreateQuiz(QuizManipulationRequestDto request);
    QuizManipulationResponseDto UpdateQuiz(int id, QuizManipulationRequestDto editRequest);
    IEnumerable<QuestionResponseDto>? GetQuestions(int id);
    IEnumerable<QuizResponseDto> GetQuizzes();
    QuizResponseDto? GetQuiz(int id);
    Task<AnswerSubmitResponseDto> SubmitAnswers(int id, List<AnswerSubmitRequestDto> request);
    bool DeleteQuiz(int id);
}