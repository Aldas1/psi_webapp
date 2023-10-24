namespace QuizAppApi.Exceptions;

public class DTOConversionException : Exception
{
    public DTOConversionException(string? message) : base(message)
    {
    }
}