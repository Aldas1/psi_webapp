namespace QuizAppApi.Dtos
{
    public class ExplanationDto
    {
        public int QuestionId { get; set; }
        public bool Correct { get; set; }
        public string? Explanation { get; set; } = string.Empty;
    }
}