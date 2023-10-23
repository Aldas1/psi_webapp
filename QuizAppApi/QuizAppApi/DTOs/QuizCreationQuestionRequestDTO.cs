namespace QuizAppApi.DTOs;

public class QuizCreationQuestionRequestDTO
{
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public QuestionParametersDTO QuestionParameters { get; set; } = new QuestionParametersDTO();
}