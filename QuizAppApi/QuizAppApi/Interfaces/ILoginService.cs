namespace QuizAppApi.Interfaces;

public interface ILoginService
{
    string? Login(string username, string password);
}