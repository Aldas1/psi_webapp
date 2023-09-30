namespace QuizAppApi.DTOs
{
    public class QuestionResponseDTO
    {
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public QuestionParametersDTO QuestionParameters { get; set; }
    }
}
