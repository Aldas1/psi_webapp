using Newtonsoft.Json;
using QuizAppApi.Extensions;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Utils;

namespace QuizAppApi.Repositories
{
    public class InMemoryQuizRepository : IQuizRepository
    {
        private readonly List<Quiz> _quizzes;
        private int _nextId = 0;
        private static readonly string DataPath = "Data/data.json";

        private int NextId() => _nextId++;

        private void UpdateDataFile()
        {
            var data = new InMemoryQuizRepositoryLocalStorage { NextId = _nextId, Quizzes = _quizzes };
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DataPath));
                using var streamWriter = File.CreateText(DataPath);
                using var jsonTextWriter = new JsonTextWriter(streamWriter);
                var serializer = JsonSerializer.Create(QuizSerialization.SerializerSettings);
                serializer.Serialize(jsonTextWriter, data);
            }
            catch (IOException e)
            {
                Console.Error.WriteLineAsync("Failed writing a file");
            }
        }

        private List<Quiz> ReadFromDataFile()
        {
            try
            {
                using var streamReader = File.OpenText(DataPath);
                using var jsonTextReader = new JsonTextReader(streamReader);
                var serializer = JsonSerializer.Create(QuizSerialization.SerializerSettings);
                var data = serializer.Deserialize<InMemoryQuizRepositoryLocalStorage>(jsonTextReader);
                _nextId = data.NextId;
                return data.Quizzes.ToList();
            }
            catch (Exception e)
            {
                if (e is not (IOException or JsonException)) throw;
                Console.Error.WriteLineAsync($"Failure reading from file: {DataPath}");
                return new List<Quiz>();
            }
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

        public void DeleteQuiz(int id)
        {
            _quizzes.RemoveAt(id);
        }
    }
}
