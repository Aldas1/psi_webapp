using QuizAppApi.Models;

namespace QuizAppApi.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetUserAsync(string username);
    Task<IEnumerable<User>> GetUsersAsync();
    Task SaveAsync();
}