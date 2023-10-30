namespace QuizAppApi.DTOs;

public class QuizResponseDTO
{
    public string Name { get; set; } = String.Empty;
    public int Id { get; set; }
    public int NumberOfSubmitters { get; set; }
}