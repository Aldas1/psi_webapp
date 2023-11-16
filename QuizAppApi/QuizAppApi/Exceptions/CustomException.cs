namespace QuizAppApi.Exceptions;

public class CustomException : Exception
{
    public string ErrorCode { get; }

    public CustomException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}