namespace QuizAppApi.DTOs;

public class QuestionResponseDTO
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = String.Empty;
    public string QuestionType { get; set; } = String.Empty;
    public QuestionParametersDTO QuestionParameters { get; set; } = new QuestionParametersDTO();
}