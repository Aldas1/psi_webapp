namespace QuizAppApi.Events;

public class EventBase<TEventArgs> where TEventArgs : EventArgs
{
    public delegate Task EventDelegate(object  sender, TEventArgs args);

    public static event EventDelegate? Event;

    public static void Raise(object sender, TEventArgs args)
    {
        Event?.Invoke(sender, args);
    }
}