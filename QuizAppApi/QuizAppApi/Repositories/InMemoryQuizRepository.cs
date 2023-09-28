using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using System.Text.Json;

namespace QuizAppApi.Repositories
{
    public class InMemoryQuizRepository : IQuizRepository
    {
        private readonly List<Quiz> _quizzes;
        private int _nextId = 0;

        private int NextId() => _nextId++;

        private void UpdateDataFile()
        {
            // Should write current state to a file
            return;
        }

        private List<Quiz> ReadFromDataFile()
        {
            // Should read data from Data/quizzes.json
            return new List<Quiz>();
        }

        public InMemoryQuizRepository()
        {
            _quizzes = ReadFromDataFile();
        }


        public Quiz? AddQuiz(Quiz quiz)
        {
            throw new NotImplementedException();
        }

        public Quiz? GetQuizById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Quiz> GetQuizzes()
        {
            throw new NotImplementedException();
        }

        public Quiz? UpdateQuiz(int id, Quiz quiz)
        {
            throw new NotImplementedException();
        }
    }
}
