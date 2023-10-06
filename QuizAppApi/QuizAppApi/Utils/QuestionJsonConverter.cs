using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuizAppApi.Enums;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Utils
{
    public class QuestionJsonConverter : JsonConverter<Question>
    {
        public override void WriteJson(JsonWriter writer, Question? value, JsonSerializer serializer)
        {
            var jo = JObject.FromObject(value);
            jo.WriteTo(writer);
        }

        public override Question? ReadJson(JsonReader reader, Type objectType, Question? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            //TODO cia irgi pakeist kad teisingai matchintu
            if (jo["Type"] != null && Enum.TryParse(jo["Type"].Value<string>(), out QuestionType questionType))
            {
                switch (questionType)
                {
                    case QuestionType.SingleChoiceQuestion:
                        return jo.ToObject<SingleChoiceQuestion>();
                    case QuestionType.MultipleChoiceQuestion:
                        return jo.ToObject<MultipleChoiceQuestion>();
                    case QuestionType.OpenTextQuestion:
                        return jo.ToObject<OpenTextQuestion>();
                    default:
                        return null;
                }
            }
            return null;
        }
    }
}