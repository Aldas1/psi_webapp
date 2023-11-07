using QuizAppApi.Models;

namespace QuizAppApi.Interfaces;

public interface IUserRepository
{
    void AddUser(User user);
    User? GetUser(string username);
    IEnumerable<User> GetUsers();
}