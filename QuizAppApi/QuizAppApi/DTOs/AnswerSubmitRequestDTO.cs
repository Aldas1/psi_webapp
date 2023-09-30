namespace QuizAppApi.DTOs
{
    public class AnswerSubmitRequestDTO
    {
        public int QuestionId { get; set; }
        public int? OptionIndex { get; set; }
        // TODO: Add props for other question types
    }
}
