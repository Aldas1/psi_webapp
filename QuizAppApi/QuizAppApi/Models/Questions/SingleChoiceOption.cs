namespace QuizAppApi.Models.Questions;

public class SingleChoiceOption
{
    public string Name { get; set; }

    public virtual int SingleChoiceQuestionId { get; set; }
    public virtual SingleChoiceQuestion SingleChoiceQuestion { get; set; } = null!;
}