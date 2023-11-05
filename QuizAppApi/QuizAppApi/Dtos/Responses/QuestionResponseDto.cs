namespace QuizAppApi.Dtos.Responses;

public class QuestionResponseDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public string QuestionType { get; set; }
    public QuestionParametersDto QuestionParameters { get; set; }
}