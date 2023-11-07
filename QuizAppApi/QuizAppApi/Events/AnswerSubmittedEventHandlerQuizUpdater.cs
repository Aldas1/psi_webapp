using QuizAppApi.Interfaces;

namespace QuizAppApi.Events;

public class AnswerSubmittedEventHandlerQuizUpdater : EventHandlerBase
{
    private readonly IServiceProvider _serviceProvider;

    public AnswerSubmittedEventHandlerQuizUpdater(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private void UpdateQuizData(object sender, AnswerSubmittedEventArgs args)
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetService<IQuizRepository>();
        var quiz = repo?.GetQuizById(args.QuizId);
        if (quiz != null)
        {
            quiz.NumberOfSubmitters++;
            repo?.Save();
        }
    }
    
    public override void RegisterEventHandler()
    {
        AnswerSubmittedEvent.Event += UpdateQuizData;
    }

    protected override void UnregisterEventHandler()
    {
        AnswerSubmittedEvent.Event -= UpdateQuizData;
    }
}