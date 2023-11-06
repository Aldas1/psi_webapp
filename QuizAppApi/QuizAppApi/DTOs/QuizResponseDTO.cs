namespace QuizAppApi.Dtos;

public class QuizResponseDto
{
    public string Name { get; set; } = String.Empty;
    public int Id { get; set; }
    public int NumberOfSubmitters { get; set; }
}