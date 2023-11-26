using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;

public interface IQuizDiscussionService
{
    Task<CommentDto> SaveMessageAsync(int quizId, string? username, string content, bool isAiAnswer = false);
    IEnumerable<CommentDto> GetRecentComments(int quizId);
}