namespace QuizAppApi.Dtos;

public class AnswerSubmitQuestionStatusResponseDto
{
    public int QuestionId { get; set; }
    public bool Correct { get; set; }
}