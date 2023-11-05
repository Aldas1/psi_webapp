namespace QuizAppApi.Exceptions;

public class DtoConversionException : Exception
{
    public DtoConversionException(string? message) : base(message)
    {
    }
}