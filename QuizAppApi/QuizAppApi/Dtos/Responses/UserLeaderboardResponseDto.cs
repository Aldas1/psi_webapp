namespace QuizAppApi.Dtos;

public class UserLeaderboardResponseDto
{
    public string Username { get; set; }
    public double AverageScore { get; set; }
    public int NumberOfSubmissions { get; set; }
}