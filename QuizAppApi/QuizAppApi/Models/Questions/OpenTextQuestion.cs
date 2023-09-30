namespace QuizAppApi.Models.Questions
{
    public class OpenTextQuestion : Question
    {
        public string CorrectAnswer { get; set; }
        public override string Type { get => "openTextQuestion"; }
    }
}
