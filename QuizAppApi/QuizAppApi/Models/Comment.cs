namespace QuizAppApi.Models;

public class Comment
{
    public int CommentId { get; set; }
    public string Content { get; set; }
    public string? Username { get; set; }
    public DateTime Date { get; set; }
}