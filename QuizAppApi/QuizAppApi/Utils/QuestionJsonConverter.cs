using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            if (jo["Type"] != null && Enum.TryParse(jo["Type"].Value<string>(), out QuestionType questionType))
            {
                switch (questionType)
                {
                    case QuestionType.singleChoiceQuestion:
                        return jo.ToObject<SingleChoiceQuestion>();
                    case QuestionType.multipleChoiceQuestion:
                        return jo.ToObject<MultipleChoiceQuestion>();
                    case QuestionType.openTextQuestion:
                        return jo.ToObject<OpenTextQuestion>();
                    default:
                        return null;
                }
            }
            return null;
        }
    }
}