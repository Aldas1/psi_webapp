namespace QuizAppApi.Dtos.Responses;

public class AnswerSubmitResponseDto
{
    public int Score { get; set; }
    public int CorrectlyAnswered { get; set; } // number of correctly answered questions
    public string? Status { get; set; } // "passed" or "failed"
    public List<ExplanationDto> Explanations { get; set; }
}