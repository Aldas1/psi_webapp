namespace QuizAppApi.Models.Questions
{
    public class SingleChoiceQuizAnswer : QuizAnswer
    {
        public int OptionIndex { get; set; }
        public override bool IsCorrect(QuizAnswer answer)
        {
            return (answer is SingleChoiceQuizAnswer ans) && ans.OptionIndex == OptionIndex;
        }

        public SingleChoiceQuizAnswer(int optionIndex)
        {
            OptionIndex = optionIndex;
        }
    }
}
