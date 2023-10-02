namespace QuizAppApi.Models;

public class InMemoryQuizRepositoryLocalStorage
{
    public int NextId { get; set; }
    public ICollection<Quiz> Quizzes { get; set; }
}