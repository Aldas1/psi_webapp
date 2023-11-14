using QuizAppApi.Interfaces;

namespace QuizAppApi.Events;

public class AnswerSubmittedEventHandlerQuizUpdater : EventHandlerBase
{
    private readonly IServiceProvider _serviceProvider;

    public AnswerSubmittedEventHandlerQuizUpdater(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private async Task UpdateQuizDataAsync(object sender, AnswerSubmittedEventArgs args)
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetService<IQuizRepository>();
        var quiz = await repo?.GetQuizByIdAsync(args.QuizId);
        if (quiz != null)
        {
            quiz.NumberOfSubmitters++;
            await repo?.SaveAsync();
        }
    }

    public override void RegisterEventHandler()
    {
        AnswerSubmittedEvent.Event += UpdateQuizDataAsync;
    }

    protected override void UnregisterEventHandler()
    {
        AnswerSubmittedEvent.Event -= UpdateQuizDataAsync;
    }
}