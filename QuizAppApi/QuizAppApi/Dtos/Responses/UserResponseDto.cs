namespace QuizAppApi.Dtos;

public class UserResponseDto
{
    public string Username { get; set; }
    public double TotalScore { get; set; }
    public int NumberOfSubmissions { get; set; }
}