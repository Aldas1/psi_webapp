namespace QuizAppApi.Dtos;

public class UserResponseDto
{
    public string Username { get; set; }
    public int TotalScore { get; set; }
    public int NumberOfSubmissions { get; set; }
}