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
        var comment = await _discussionService.SaveMessageAsync(quizId, username, content);
        await Clients.Group(quizId.ToString()).SendAsync("NewMessage", comment);

        const string explainPrefix = "/explain";
        if (content.Trim().StartsWith(explainPrefix))
        {
            int startIndex = explainPrefix.Length;
            string? explainQuery = content.Trim()[startIndex..];

            var aiAnswer = await _discussionService.SaveMessageAsync(quizId, "ChatGPT", explainQuery, isAiAnswer: true);
            await Clients.Group(quizId.ToString()).SendAsync("NewMessage", aiAnswer);
        }
    }

    public async Task AddToGroup(int quizId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, quizId.ToString());
        await Clients.Client(Context.ConnectionId)
            .SendAsync("NewMessages", _discussionService.GetRecentComments(quizId));
        await Clients.Client(Context.ConnectionId).SendAsync("NewMessage",
            new CommentDto { Content = "Welcome to quiz discussion. To call ChatGPT start your message with \"/explain\". Be polite!", Username = "system", Date = DateTime.Now });
    }
}