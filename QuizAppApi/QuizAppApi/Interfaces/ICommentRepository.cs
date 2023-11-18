using System.Linq.Expressions;
using QuizAppApi.Models;

namespace QuizAppApi.Interfaces;

public interface ICommentRepository
{
    IEnumerable<Comment> GetByCondition(Expression<Func<Comment, bool>> condition);
    Comment? AddComment(Comment c);
}