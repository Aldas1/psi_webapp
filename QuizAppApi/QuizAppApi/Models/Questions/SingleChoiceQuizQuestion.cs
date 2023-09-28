using QuizAppApi.Controllers;
using System.Text.Json;

namespace QuizAppApi.Models.Questions
{
    public class SingleChoiceQuizQuestion : QuizQuestion
    {
        public List<string> Options { get; set; }
        public SingleChoiceQuizQuestion(string questionText, List<string> options, SingleChoiceQuizAnswer correctAnswer) : base(questionText, correctAnswer)
        {
            Options = options;
        }

        public override string GetQuestionType()
        {
            return "singleChoiceQuestion";
        }

        public override object GenerateApiParameters()
        {
            return new
            {
                Options = Options
            };
        }

        public static QuizQuestion CreateFromParameters(string questionText, Dictionary<string, JsonElement> questionParameters)
        {
            Dictionary<string, JsonElement> parameters = (Dictionary<string, JsonElement>)questionParameters["parameters"];
            // Getting answer options from parameters
            List<string> options = (List<string>)parameters["options"];
            int correctOptionIndex = Convert.ToInt32(parameters["correctOptionIndex"]);

            return new SingleChoiceQuizQuestion(questionText, options, new SingleChoiceQuizAnswer(correctOptionIndex));
        }
    }
}
