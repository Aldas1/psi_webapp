namespace QuizAppApi.Models.Questions;

public class Option
{
    public string Name { get; set; }
    public bool Correct { get; set; }
        
    public virtual OptionQuestion Question { get; set; }
    public virtual int QuestionId { get; set; }
}