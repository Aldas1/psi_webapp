namespace QuizAppApi.DTOs
{
    public class ExplanationDTO
    {
        public int QuestionId { get; set; }
        public bool Correct { get; set; }
        public string? Explanation { get; set; }
    }
}