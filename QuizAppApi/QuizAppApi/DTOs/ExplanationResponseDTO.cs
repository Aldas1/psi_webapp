namespace QuizAppApi.DTOs
{
    public class ExplanationResponseDTO
    {
        public int QuestionId { get; set; }
        public string Explanation { get; set; } = string.Empty;
    }
}