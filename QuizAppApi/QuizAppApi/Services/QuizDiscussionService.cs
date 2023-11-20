using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Exceptions;

namespace QuizAppApi.Services;

public class QuizDiscussionService : IQuizDiscussionService
{
    private readonly ICacheRepository _cacheRepository;

    public QuizDiscussionService(ICacheRepository cacheRepository)
    {
        _cacheRepository = cacheRepository;
    }

    public CommentDto SaveMessage(int quizId, string? username, string content)
    {
        var comment = new Comment
        {
            Content = content,
            Date = DateTime.Now,
            Username = username,
            QuizId = quizId
        };
        Monitor.Enter(_cacheRepository.Lock);
        try
        {
            _cacheRepository.Add("comments", comment);
        }
        catch(TypeMismatchException)
        {
            _cacheRepository.Clear("comments");
            _cacheRepository.Add("comments", comment);
            throw;
        }
        finally
        {
            Monitor.Exit(_cacheRepository.Lock);
        }

        return ConvertToDto(comment);
    }

    public IEnumerable<CommentDto> GetRecentComments(int quizId)
    {
        Monitor.Enter(_cacheRepository.Lock);
        IEnumerable<Comment> comments;
        try
        {
            comments = _cacheRepository.Retrieve<Comment>("comments").Where(c => c.QuizId == quizId);
        }
        catch (TypeMismatchException)
        {
            _cacheRepository.Clear("comments");
            comments = _cacheRepository.Retrieve<Comment>("comments").Where(c => c.QuizId == quizId);
            throw;
        }
        finally
        {
            Monitor.Exit(_cacheRepository.Lock);
        }

        return comments.Select(ConvertToDto);
    }

    private static CommentDto ConvertToDto(Comment c)
    {
        return new CommentDto
        {
            Content = c.Content,
            Username = c.Username,
            Date = c.Date
        };
    }
}