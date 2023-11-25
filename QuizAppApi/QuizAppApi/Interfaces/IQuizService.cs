using QuizAppApi.Dtos;
using QuizAppApi.Models;

namespace QuizAppApi.Interfaces;
public interface IQuizService
{
    Task<QuizManipulationResponseDto> CreateQuizAsync(QuizManipulationRequestDto request, User? user = null);
    Task<QuizManipulationResponseDto> UpdateQuizAsync(int id, QuizManipulationRequestDto editRequest);
    Task<IEnumerable<QuizResponseDto>> GetQuizzesAsync();
    Task<QuizResponseDto?> GetQuizAsync(int id);
    Task<bool> DeleteQuizAsync(int id);
    Task<bool> CanUserEditQuizAsync(User? user, int quizId);
}