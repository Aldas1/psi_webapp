namespace QuizAppApi.DTOs;

public class UserResponseDTO
{
    public string Username { get; set; }
    public int TotalScore { get; set; }
    public int NumberOfSubmissions { get; set; }
}