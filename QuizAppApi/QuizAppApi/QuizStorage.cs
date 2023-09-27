using QuizAppApi.Models;
using QuizAppApi.Models.Questions;

namespace QuizAppApi
{
    public sealed class QuizStorage : List<Quiz>
    {
        private static readonly Lazy<QuizStorage> _instance = new(() => new QuizStorage());
        public static QuizStorage Instance
        {
            get => _instance.Value;
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
            this.Add(citiesQuiz);
        }

    }
}
