using QuizAppApi.Data;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;

namespace QuizAppApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly QuizContext _quizContext;

    public UserRepository(QuizContext quizContext)
    {
        _quizContext = quizContext;
    }

    public void AddUser(User user)
    {
        _quizContext.Users.Add(user);
        _quizContext.SaveChanges();
    }

    public User? GetUser(string username)
    {
        return _quizContext.Users.Find(username);
    }

    public IEnumerable<User> GetUsers()
    {
        return _quizContext.Users;
    }

    public void Save()
    {
        _quizContext.SaveChanges();
    }
}