namespace QuizAppApi.Events;

public class EventBase<TEventArgs> where TEventArgs : EventArgs
{
    public static event Func<object, TEventArgs, Task>? Event;

    public static void Raise(object sender, TEventArgs args)
    {
        Event?.Invoke(sender, args);
    }
}