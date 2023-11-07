using QuizAppApi.Interfaces;

namespace QuizAppApi.Events;

public class AnswerSubmittedEventHandlerUserUpdater : EventHandlerBase
{
    private readonly IServiceProvider _serviceProvider;

    public AnswerSubmittedEventHandlerUserUpdater(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private void UpdateUserData(object sender, AnswerSubmittedEventArgs args)
    {
        if (args.Username == null) return;
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetService<IUserRepository>();
        var user = repo?.GetUser(args.Username);
        if (user == null) return;
        user.TotalScore += args.Score;
        user.NumberOfSubmissions++;
        repo?.Save();
    }
    
    public override void RegisterEventHandler()
    {
        AnswerSubmittedEvent.Event += UpdateUserData;
    }

    protected override void UnregisterEventHandler()
    {
        AnswerSubmittedEvent.Event -= UpdateUserData;
    }
}