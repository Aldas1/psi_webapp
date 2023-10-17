namespace QuizAppApi.DTOs
{
    public class ChatGptResponseDTO
    {
        public int QuestionId { get; set; }
        public string Explanation { get; set; } = string.Empty;
    }
}