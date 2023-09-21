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

    }
}
