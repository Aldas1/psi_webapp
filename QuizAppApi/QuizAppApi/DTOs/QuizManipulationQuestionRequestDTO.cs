namespace QuizAppApi.Dtos;
public class QuizManipulationQuestionRequestDto
{
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public QuestionParametersDto QuestionParameters { get; set; } = new QuestionParametersDto();
}
