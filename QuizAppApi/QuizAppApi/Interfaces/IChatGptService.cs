namespace QuizAppApi.Interfaces
{
    public interface IChatGptService
    {
        string GenerateExplanation(string questionText, string chosenOption);
    }
}