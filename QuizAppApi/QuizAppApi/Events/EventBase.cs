namespace QuizAppApi.Events;

public class EventBase<TEventArgs> where TEventArgs : EventArgs
{
    public static event EventHandler<TEventArgs>? Event;

    public static void Raise(object sender, TEventArgs args)
    {
        Event?.Invoke(sender, args);
    }
}