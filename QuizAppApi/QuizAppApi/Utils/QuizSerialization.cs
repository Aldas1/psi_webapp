using Newtonsoft.Json;
using QuizAppApi.Models;

namespace QuizAppApi.Utils
{
    public static class QuizSerialization
    {
        private static JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Converters = new JsonConverter[] { new QuestionJsonConverter() }
        };

        public static string SerializeQuiz(Quiz quiz)
        {
            var json = JsonConvert.SerializeObject(quiz, _serializerSettings);
            return json;
        }

        public static Quiz? DeserializeQuiz(string quizStr)
        {
            var quiz = JsonConvert.DeserializeObject<Quiz>(quizStr, _serializerSettings);
            return quiz;
        }
        
        public static Quiz CloneQuiz(Quiz quiz)
        {
            return (Quiz)DeserializeQuiz(SerializeQuiz(quiz));
        }
    }
}