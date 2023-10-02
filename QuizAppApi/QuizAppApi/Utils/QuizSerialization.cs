using Newtonsoft.Json;
using QuizAppApi.Models;

namespace QuizAppApi.Utils
{
    public static class QuizSerialization
    {
        public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Converters = new JsonConverter[] { new QuestionJsonConverter() }
        };

        public static string SerializeQuiz(Quiz quiz)
        {
            var json = JsonConvert.SerializeObject(quiz, SerializerSettings);
            return json;
        }

        public static Quiz? DeserializeQuiz(string quizStr)
        {
            var quiz = JsonConvert.DeserializeObject<Quiz>(quizStr, SerializerSettings);
            return quiz;
        }
        
        public static Quiz CloneQuiz(Quiz quiz)
        {
            return (Quiz)DeserializeQuiz(SerializeQuiz(quiz));
        }
        
        
    }
}