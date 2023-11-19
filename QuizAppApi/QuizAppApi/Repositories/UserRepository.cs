using Microsoft.EntityFrameworkCore;
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

    public async Task AddUserAsync(User user)
    {
        await _quizContext.Users.AddAsync(user);
        await _quizContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserAsync(string username)
    {
        return await _quizContext.Users.FindAsync(username);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _quizContext.Users.ToListAsync();
    }

    public async Task SaveAsync()
    {
        await _quizContext.SaveChangesAsync();
    }
}