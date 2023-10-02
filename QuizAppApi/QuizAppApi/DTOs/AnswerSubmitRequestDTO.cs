namespace QuizAppApi.DTOs
{
    public class AnswerSubmitRequestDTO
    {
        public int QuestionId { get; set; }
        public string? OptionName { get; set; }
        // TODO: Add props for other question types
    }
}
