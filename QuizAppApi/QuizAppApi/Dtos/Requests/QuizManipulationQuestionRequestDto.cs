namespace QuizAppApi.Dtos;
public class QuizManipulationQuestionRequestDto
{
    public string QuestionText { get; set; }
    public string QuestionType { get; set; }
    public QuestionParametersDto QuestionParameters { get; set; }
}
