using QuizAppApi.Extensions;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;

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
            Quiz newQuiz = QuizSerialization.CloneQuiz(quiz);
            newQuiz.Id = NextId();
            foreach (var question in newQuiz.Questions)
            {
                question.Id = NextId();
            }
            _quizzes.Add(newQuiz);
            UpdateDataFile();
            return QuizSerialization.CloneQuiz(newQuiz);
        }

        public Quiz? GetQuizById(int id)
        {
            var quiz = _quizzes.FirstOrDefault(q => q.Id == id);
            if (quiz == null)
            {
                return null;
            }
            return QuizSerialization.CloneQuiz(quiz);
        }

        public IEnumerable<Quiz> GetQuizzes()
        {
            return (IEnumerable<Quiz>)_quizzes.Clone();
        }

        public Quiz? UpdateQuiz(int id, Quiz quiz)
        {
            var foundQuiz = _quizzes.FirstOrDefault(q => q.Id == id);
            if (foundQuiz == null)
            {
                return null;
            }

            _quizzes.Remove(foundQuiz);
            var newQuiz = QuizSerialization.CloneQuiz(quiz);
            newQuiz.Id = id;
            // TODO: handle question id
            // It is highly likely that we should do somewhat complicated question matching or create new id's
            // In the future, we should add QuestionRepository to handle such tasks.
            _quizzes.Add(newQuiz);
            UpdateDataFile();
            return QuizSerialization.CloneQuiz(newQuiz);
        }
    }
}
