namespace QuizAppApi.Utils;

public delegate void AnswerSubmittedEventHandler(object sender, AnswerSubmittedEventArgs args);

public class EventPublisher
{
    public event AnswerSubmittedEventHandler AnswerSubmitted;

    public void RaiseAnswerSubmittedEvent(object sender, AnswerSubmittedEventArgs e)
    {
        AnswerSubmitted?.Invoke(sender, e);
    }
}