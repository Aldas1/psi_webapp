namespace QuizAppApi.Dtos
{
    public class QuizManipulationRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<QuizManipulationQuestionRequestDto> Questions { get; set; } = new List<QuizManipulationQuestionRequestDto>();
    }
}
