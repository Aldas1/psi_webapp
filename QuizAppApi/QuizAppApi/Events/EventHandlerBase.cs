namespace QuizAppApi.Events;

public abstract class EventHandlerBase : IDisposable
{
    public abstract void RegisterEventHandler();
    protected abstract void UnregisterEventHandler();

    public void Dispose()
    {
        UnregisterEventHandler();
        GC.SuppressFinalize(this); // Rider suggestion
    }
}
