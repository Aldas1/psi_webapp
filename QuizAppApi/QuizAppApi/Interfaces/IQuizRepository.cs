using QuizAppApi.Models;

namespace QuizAppApi.Interfaces;

public interface IQuizRepository
{
    Task<Quiz?> AddQuizAsync(Quiz quiz);
    Task<IEnumerable<Quiz>> GetQuizzesAsync();
    Task<Quiz?> GetQuizByIdAsync(int id);
    Task DeleteQuizAsync(int id);
    Task SaveAsync();
}