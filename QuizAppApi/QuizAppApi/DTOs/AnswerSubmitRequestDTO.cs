namespace QuizAppApi.DTOs
{
    public class AnswerSubmitRequestDTO
    {
        public int QuestionId { get; set; }
        public string? OptionName { get; set; }
        public List<string>? OptionNames { get; set; }
        public string? AnswerText { get; set; }
    }
}
