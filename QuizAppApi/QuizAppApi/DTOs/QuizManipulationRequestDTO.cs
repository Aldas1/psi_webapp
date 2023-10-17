namespace QuizAppApi.DTOs
{
    public class QuizManipulationRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<QuizManipulationQuestionRequestDTO> Questions { get; set; } = new List<QuizManipulationQuestionRequestDTO>();
    }
}
