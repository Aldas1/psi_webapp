using QuizAppApi.Enums;

namespace QuizAppApi.Models.Questions;

public abstract class OptionQuestion : Question
{
    public virtual ICollection<Option> Options { get; set; } = new List<Option>();
}