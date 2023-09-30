namespace QuizAppApi.DTOs
{
    public class QuizCreationQuestionRequestDTO
    {
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public QuestionParametersDTO QuestionParameters { get; set; }
    }
}
