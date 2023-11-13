using Microsoft.EntityFrameworkCore;
using QuizAppApi.Data;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;

namespace QuizAppApi.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly QuizContext _context;

    public QuizRepository(QuizContext context)
    {
        _context = context;
    }

    public async Task<Quiz?> AddQuizAsync(Quiz quiz)
    {
        var entity = await _context.Quizzes.AddAsync(quiz);
        await _context.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<IEnumerable<Quiz>> GetQuizzesAsync()
    {
        return _context.Quizzes;
    }

    public async Task<Quiz?> GetQuizByIdAsync(int id)
    {
        return await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task DeleteQuizAsync(int id)
    {
        var quiz = await GetQuizByIdAsync(id);
        if (quiz is not null)
        {
            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}