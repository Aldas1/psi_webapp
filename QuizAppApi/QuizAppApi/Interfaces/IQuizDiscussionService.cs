using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;

public interface IQuizDiscussionService
{
    CommentDto SaveMessage(int quizId, string? username, string content);
    IEnumerable<CommentDto> GetRecentComments(int quizId);
}