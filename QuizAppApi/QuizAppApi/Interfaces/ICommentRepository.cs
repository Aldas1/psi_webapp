using System.Linq.Expressions;
using QuizAppApi.Models;

namespace QuizAppApi.Interfaces;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetByConditionAsync(Expression<Func<Comment, bool>> condition);
    Task<Comment?> AddCommentAsync(Comment c);
}