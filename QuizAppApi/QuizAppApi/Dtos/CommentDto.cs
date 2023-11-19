namespace QuizAppApi.Dtos;

public class CommentDto
{
    public string Content { get; set; }
    public string? Username { get; set; }
    public DateTime Date { get; set; }
}