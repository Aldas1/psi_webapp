namespace QuizAppApi.DTOs;

public class QuestionResponseDTO
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public string QuestionType { get; set; }
    public QuestionParametersDTO QuestionParameters { get; set; } //= new QuestionParametersDTO();
}