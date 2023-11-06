namespace QuizAppApi.Dtos;

public class QuestionResponseDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = String.Empty;
    public string QuestionType { get; set; } = String.Empty;
    public QuestionParametersDto QuestionParameters { get; set; } = new QuestionParametersDto();
}