namespace QuizAppApi.Exceptions;

public class TypeMismatchException : Exception
{
    public Type AttemptedType { get; }
    public Type ExistingType { get; }

    public TypeMismatchException(Type attemptedType, Type existingType, string message)
        : base($"Type mismatch: requires {attemptedType} but got {existingType}. {message}")
    {
        AttemptedType = attemptedType;
        ExistingType = existingType;
    }
}