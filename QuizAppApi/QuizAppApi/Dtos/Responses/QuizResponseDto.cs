namespace QuizAppApi.Dtos;

public class QuizResponseDto
{
    public string Name { get; set; }
    public int Id { get; set; }
    public int NumberOfSubmitters { get; set; }
    public string? Owner { get; set; }
}