using QuizAppApi.Interfaces;

namespace QuizAppApi.Events;

public class AnswerSubmittedEventHandlerUserUpdater : EventHandlerBase
{
    private readonly IServiceProvider _serviceProvider;

    public AnswerSubmittedEventHandlerUserUpdater(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private async Task UpdateUserDataAsync(object sender, AnswerSubmittedEventArgs args)
    {
        if (args.Username == null) return;
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetService<IUserRepository>();
        var user = await repo?.GetUserAsync(args.Username);
        if (user == null) return;
        user.TotalScore += args.Score;
        user.NumberOfSubmissions++;
        await repo?.SaveAsync();
    }
    
    public override void RegisterEventHandler()
    {
        AnswerSubmittedEvent.Event += UpdateUserDataAsync;
    }

    protected override void UnregisterEventHandler()
    {
        AnswerSubmittedEvent.Event -= UpdateUserDataAsync;
    }
}