namespace QuizAppApi.DTOs
{
    public class QuizCreationRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<QuizCreationQuestionRequestDTO> Questions { get; set; } = new List<QuizCreationQuestionRequestDTO>();
    }
}
