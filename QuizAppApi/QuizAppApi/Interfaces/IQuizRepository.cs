using QuizAppApi.Models;

namespace QuizAppApi.Interfaces
{
    public interface IQuizRepository
    {
        Quiz? AddQuiz(Quiz quiz);
        IEnumerable<Quiz> GetQuizzes();
        Quiz? GetQuizById(int id);
        Quiz? UpdateQuiz(int id, Quiz quiz);
        void DeleteQuiz(int id);
        void Save();
    }
}
