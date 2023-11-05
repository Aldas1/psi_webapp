namespace QuizAppApi.Dtos.Requests;

public class QuizManipulationQuestionRequestDto
{
    public string QuestionText { get; set; }
    public string QuestionType { get; set; }
    public QuestionParametersDto QuestionParameters { get; set; }
}
