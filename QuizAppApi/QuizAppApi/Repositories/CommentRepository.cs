using System.Linq.Expressions;
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

    public IEnumerable<Comment> GetByCondition(Expression<Func<Comment, bool>> condition)
    {
        return _quizContext.Set<Comment>().Where(condition);
    }

    public Comment? AddComment(Comment c)
    {
        var updatedC = _quizContext.Comments.Add(c).Entity;
        _quizContext.SaveChanges();
        return updatedC;
    }
}