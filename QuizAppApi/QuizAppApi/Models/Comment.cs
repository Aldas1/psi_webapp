using System.ComponentModel.DataAnnotations.Schema;

namespace QuizAppApi.Models;

public class Comment
{
    public int CommentId { get; set; }
    public string Content { get; set; }
    public string? Username { get; set; }
    public DateTime Date { get; set; }
    [NotMapped] public bool Stored { get; set; }

    public int QuizId { get; set; }
}