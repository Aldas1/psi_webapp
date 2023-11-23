using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Exceptions;

namespace QuizAppApi.Services;

public class QuizDiscussionService : IQuizDiscussionService
{
    private readonly ICacheRepository _cacheRepository;
    private readonly IExplanationService _explanationService;

    public QuizDiscussionService(ICacheRepository cacheRepository, IExplanationService explanationService)
    {
        _cacheRepository = cacheRepository;
        _explanationService = explanationService;
    }

    public CommentDto SaveMessage(int quizId, string? username, string content)
    {
        const string explainPrefix = "/explain";

        var comment = new Comment
        {
            Content = content,
            Date = DateTime.Now,
            Username = username,
            QuizId = quizId
        };

        if (content.Trim().StartsWith(explainPrefix))
        {
            int startIndex = explainPrefix.Length;
            string? explainQuery = content.Trim()[startIndex..];

            var explanation = _explanationService.GenerateCommentExplanationAsync(explainQuery).Result;

            comment.Content = explanation;
            comment.Username = "ChatGPT";
        }
     
        for (int attempt = 0; attempt < 3; attempt++)
        {
            Monitor.Enter(_cacheRepository.Lock);
            try
            {
                _cacheRepository.Add("comments", comment);
                break;
            }
            catch (TypeMismatchException)
            {
                _cacheRepository.Clear("comments");
            }
            finally
            {
                Monitor.Exit(_cacheRepository.Lock);
            }
        }

        return ConvertToDto(comment);
    }

    public IEnumerable<CommentDto> GetRecentComments(int quizId)
    {
        IEnumerable<Comment> comments = null;

        for (int attempt = 0; attempt < 3; attempt++)
        {
            Monitor.Enter(_cacheRepository.Lock);
            try
            {
                comments = _cacheRepository.Retrieve<Comment>("comments").Where(c => c.QuizId == quizId);
                break;
            }
            catch (TypeMismatchException)
            {
                _cacheRepository.Clear("comments");
            }
            finally
            {
                Monitor.Exit(_cacheRepository.Lock);
            }
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