namespace QuizAppApi.Events;

public class AnswerSubmittedEventArgs : EventArgs
{
    public string? Username { get; set; }
    public double Score { get; set; }
    public int QuizId { get; set; }
}