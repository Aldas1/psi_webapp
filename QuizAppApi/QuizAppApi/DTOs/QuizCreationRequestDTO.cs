namespace QuizAppApi.DTOs
{
    public class QuizCreationRequestDTO
    {
        public string Name { get; set; }
        public ICollection<QuizCreationQuestionRequestDTO> Questions { get; set; }
    }
}
