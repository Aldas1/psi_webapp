using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QuizAppApi.Data;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;

namespace QuizAppApi.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly QuizContext _quizContext;

    public CommentRepository(QuizContext quizContext)
    {
        _quizContext = quizContext;
    }

    public async Task<IEnumerable<Comment>> GetByConditionAsync(Expression<Func<Comment, bool>> condition)
    {
        return await _quizContext.Set<Comment>().Where(condition).ToListAsync();
    }

    public async Task<Comment?> AddCommentAsync(Comment c)
    {
        await _quizContext.Comments.AddAsync(c);
        await _quizContext.SaveChangesAsync();
        return c;
    }
}