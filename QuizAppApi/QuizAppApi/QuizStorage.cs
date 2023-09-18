using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using System.Collections;

namespace QuizAppApi
{
    public class QuizStorage : IEnumerable<Quiz>
    {
        private static readonly QuizStorage _instance = new();
        private readonly List<Quiz> _quizzes = new();
        public static QuizStorage Instance
        {
            get
            {
                return _instance;
            }
        }
        private QuizStorage()
        {
            Quiz citiesQuiz = new Quiz("Cities and Countries", 2);

            // populate with sample data
            // TODO: Use db
            citiesQuiz.Questions.Add(
                new SingleChoiceQuizQuestion("What is the capital of Lithuania?", new List<string>() { "Kaunas", "Vilnius", "Jonava" }, new SingleChoiceQuizAnswer(1))
            );
            citiesQuiz.Questions.Add(
                new SingleChoiceQuizQuestion("What is the capital of Spain?", new List<string>() { "Madrid", "Vilnius", "Jonava" }, new SingleChoiceQuizAnswer(0))
            );
            citiesQuiz.Questions.Add(
                new SingleChoiceQuizQuestion("Do all countries have capitals?", new List<string>() { "Yes", "No" }, new SingleChoiceQuizAnswer(0))
            );
            _quizzes.Add(citiesQuiz);
        }

        public void Add(Quiz quiz)
        {
            _quizzes.Add(quiz);
        }

        public void Remove(Quiz quiz)
        {
            _quizzes.Remove(quiz);
        }

        public void Clear()
        {
            _quizzes.Clear();
        }

        public IEnumerator<Quiz> GetEnumerator()
        {
            return _quizzes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
