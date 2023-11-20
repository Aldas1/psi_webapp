namespace QuizAppApi.Exceptions;

public class TypeMismatchException : Exception
{
    public Type AttemptedType { get; }
    public Type ExistingType { get; }

    public TypeMismatchException(Type attemptedType, Type existingType, string action, string location)
        : base($"Attempted to {action} an object of type {attemptedType} {location} a key that contains objects of type {existingType}.")
    {
        AttemptedType = attemptedType;
        ExistingType = existingType;
    }
}