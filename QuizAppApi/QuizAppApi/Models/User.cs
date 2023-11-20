using System.ComponentModel.DataAnnotations;

namespace QuizAppApi.Models;

public class User
{
    [Key]
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public double TotalScore { get; set; }
    public int NumberOfSubmissions { get; set; }

    public User(string username, string passwordHash)
    {
        Username = username;
        PasswordHash = passwordHash;
    }
}