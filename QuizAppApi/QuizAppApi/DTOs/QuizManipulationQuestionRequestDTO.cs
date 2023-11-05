namespace QuizAppApi.DTOs;
public class QuizManipulationQuestionRequestDTO
{
    public string QuestionText { get; set; }
    public string QuestionType { get; set; }
    public QuestionParametersDTO QuestionParameters { get; set; }
}
