namespace QuizAppApi.Utils;

public class AnswerSubmittedEventArgs : EventArgs
{
    public int QuizId { get; set; }
}