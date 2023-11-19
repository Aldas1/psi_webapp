namespace QuizAppApi.Events;

public delegate Task AsyncEventHandler<TEventArgs>(object sender, TEventArgs args);

public class EventBase<TEventArgs> where TEventArgs : EventArgs
{
    public static event AsyncEventHandler<TEventArgs>? Event;

    public static void Raise(object sender, TEventArgs args)
    {
        Event?.Invoke(sender, args);
    }
}