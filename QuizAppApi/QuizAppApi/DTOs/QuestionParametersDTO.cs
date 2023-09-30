namespace QuizAppApi.DTOs
{
    public class QuestionParametersDTO
    {
        public List<string>? Options { get; set; }
        public int? CorrectOptionIndex { get; set; }
        // TODO: Add props for future question types
    }
}