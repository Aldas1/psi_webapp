using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace QuizAppApi
{
    public class QuizStorage : List<Quiz>
    {
        private static readonly QuizStorage _instance = new();
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
            this.Add(citiesQuiz);
        }

    }
}
