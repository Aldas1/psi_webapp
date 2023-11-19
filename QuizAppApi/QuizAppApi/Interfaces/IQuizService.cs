using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;
public interface IQuizService
{
    Task<QuizManipulationResponseDto> CreateQuizAsync(QuizManipulationRequestDto request);
    Task<QuizManipulationResponseDto> UpdateQuizAsync(int id, QuizManipulationRequestDto editRequest);
    Task<IEnumerable<QuizResponseDto>> GetQuizzesAsync();
    Task<QuizResponseDto?> GetQuizAsync(int id);
    Task<bool> DeleteQuizAsync(int id);
}