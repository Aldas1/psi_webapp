using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;

namespace QuizAppApi.Services;

public class QuizDiscussionService : IQuizDiscussionService
{
    private readonly ICommentRepository _commentRepository;

    public QuizDiscussionService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<CommentDto> SaveMessage(int quizId, string? username, string content)
    {
        var comment = new Comment
        {
            Content = content,
            Date = DateTime.Now,
            Username = username,
            QuizId = quizId
        };
        var addedComment = await _commentRepository.AddCommentAsync(comment);
        return ConverToDto(addedComment);
    }

    public async Task<IEnumerable<CommentDto>> GetRecentComments(int quizId)
    {
        var comments = await _commentRepository.GetByConditionAsync(c => c.QuizId == quizId);
        return comments.Select(ConverToDto);
    }

    private CommentDto ConverToDto(Comment c)
    {
        return new CommentDto
        {
            Content = c.Content,
            Username = c.Username,
            Date = c.Date
        };
    }
}