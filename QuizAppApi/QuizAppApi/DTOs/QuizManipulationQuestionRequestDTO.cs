namespace QuizAppApi.DTOs;
public class QuizManipulationQuestionRequestDTO
{
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public QuestionParametersDTO QuestionParameters { get; set; } = new QuestionParametersDTO();
}
