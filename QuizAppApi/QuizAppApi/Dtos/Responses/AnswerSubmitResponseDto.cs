namespace QuizAppApi.Dtos;

public class AnswerSubmitResponseDto
{
    public double Score { get; set; }
    public int CorrectlyAnswered { get; set; } // number of correctly answered questions
    public string? Status { get; set; } // "passed" or "failed"
}
