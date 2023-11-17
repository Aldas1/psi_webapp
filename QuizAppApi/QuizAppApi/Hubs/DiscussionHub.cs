using Microsoft.AspNetCore.SignalR;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Hubs;

public class DiscussionHub : Hub
{
    private readonly IQuizDiscussionService _discussionService;

    public DiscussionHub(IQuizDiscussionService discussionService)
    {
        _discussionService = discussionService;
    }

    public async Task PostComment(string? username, string content, int quizId)
    {
        var comment = await _discussionService.SaveMessage(quizId, username, content);
        await Clients.Group(quizId.ToString()).SendAsync("NewMessage", comment);
    }

    public async Task AddToGroup(int quizId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, quizId.ToString());
        await Clients.Client(Context.ConnectionId)
            .SendAsync("NewMessages", await _discussionService.GetRecentComments(quizId));
        await Clients.Client(Context.ConnectionId).SendAsync("NewMessage",
            new CommentDto { Content = "Welcome to quiz discussion. Be polite!", Username = "system", Date = DateTime.Now });
    }
}