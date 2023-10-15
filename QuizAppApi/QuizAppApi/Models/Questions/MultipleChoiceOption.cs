namespace QuizAppApi.Models.Questions;

public class MultipleChoiceOption
{
    public string Name { get; set; }
    public bool Correct { get; set; }

    public virtual int MultipleChoiceQuestionId { get; set; }
    public virtual MultipleChoiceQuestion MultipleChoiceQuestion { get; set; }
}