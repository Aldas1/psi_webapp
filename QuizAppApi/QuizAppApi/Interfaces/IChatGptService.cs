namespace QuizAppApi.Interfaces
{
    public interface IChatGptService
    {
        Task<string> GenerateExplanationAsync(string questionText, string chosenOption);
    }
}