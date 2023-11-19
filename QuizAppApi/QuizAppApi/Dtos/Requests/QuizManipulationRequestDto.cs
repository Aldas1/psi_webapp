namespace QuizAppApi.Dtos;

public class QuizManipulationRequestDto
{
    public string Name { get; set; }
    public ICollection<QuizManipulationQuestionRequestDto> Questions { get; set; }
}
