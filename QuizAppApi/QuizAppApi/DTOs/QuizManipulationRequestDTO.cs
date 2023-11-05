namespace QuizAppApi.DTOs
{
    public class QuizManipulationRequestDTO
    {
        public string Name { get; set; }
        public ICollection<QuizManipulationQuestionRequestDTO> Questions { get; set; }
    }
}
