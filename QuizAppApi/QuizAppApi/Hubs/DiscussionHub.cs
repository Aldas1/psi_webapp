using Microsoft.AspNetCore.SignalR;
using QuizAppApi.Dtos;

namespace QuizAppApi.Hubs;

public class DiscussionHub : Hub
{
    public async Task PostComment(string username, string content, int quizId)
    {
        Console.WriteLine($"Got message. Quiz id: {quizId}");
        await Clients.Group(quizId.ToString()).SendAsync("NewMessage",
            new CommentDto { Content = content, Username = username, Date = DateTime.Now });
    }

    public async Task AddToGroup(int quizId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, quizId.ToString());
    }
}