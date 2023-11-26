using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Exceptions;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.Data;

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

    public async Task<CommentDto> SaveMessageAsync(int quizId, string? username, string content, bool isAiAnswer = false)
    {
        var comment = new Comment
        {
            Content = content,
            Date = DateTime.Now,
            Username = username,
            QuizId = quizId
        };

        if (isAiAnswer)
        {
            var explanation = await _explanationService.GenerateCommentExplanationAsync(content);

            comment.Content = explanation ?? "Sorry, but we are experiencing technical issues.";
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
        for (int attempt = 0; attempt < 3; attempt++)
        {
            Monitor.Enter(_cacheRepository.Lock);
            try
            {
                var comments = _cacheRepository.Retrieve<Comment>("comments").Where(c => c.QuizId == quizId);
                return comments.Select(ConvertToDto);
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
        
        throw new InternalException("Could not retrieve comments.");
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