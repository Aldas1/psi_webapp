using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;
public interface IQuizService
{
    QuizManipulationResponseDto CreateQuiz(QuizManipulationRequestDto request);
    QuizManipulationResponseDto UpdateQuiz(int id, QuizManipulationRequestDto editRequest);
    IEnumerable<QuizResponseDto> GetQuizzes();
    QuizResponseDto? GetQuiz(int id);
    bool DeleteQuiz(int id);
}