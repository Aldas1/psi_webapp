using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;

public interface IQuizDiscussionService
{
    Task<CommentDto> SaveMessage(int quizId, string? username, string content);
    Task<IEnumerable<CommentDto>> GetRecentComments(int quizId);
}