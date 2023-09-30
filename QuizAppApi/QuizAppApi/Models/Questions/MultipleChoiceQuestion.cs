namespace QuizAppApi.Models.Questions
{
    public class MultipleChoiceQuestion : Question
    {
        ICollection<Option> Options { get; set; }
        ICollection<Option> CorrectOptionIndexes { get; set; }

        public override string Type { get => "multipleChoiceQuestion"; }
    }
}
