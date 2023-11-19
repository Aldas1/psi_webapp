namespace QuizAppApi.Models;

public class Quiz
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfSubmitters { get; set; }
        
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}