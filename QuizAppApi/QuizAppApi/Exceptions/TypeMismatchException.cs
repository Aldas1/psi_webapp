namespace QuizAppApi.Exceptions;

public class TypeMismatchException : Exception
{
    public Type AttemptedType { get; }
    public Type ExistingType { get; }

    public TypeMismatchException(Type attemptedType, Type existingType)
        : base($"Attempted to access an object of type {attemptedType} with a key that contains objects of type {existingType}.")
    {
        AttemptedType = attemptedType;
        ExistingType = existingType;
    }
}