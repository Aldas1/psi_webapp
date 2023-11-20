namespace QuizAppApi.Exceptions;

public class TypeMismatchException : Exception
{
    public Type AttemptedType { get; }
    public Type ExistingType { get; }

    public TypeMismatchException(Type attemptedType, Type existingType)
        : base($"Attempted to add an object of type {attemptedType} to a key that already contains objects of type {existingType}.")
    {
        AttemptedType = attemptedType;
        ExistingType = existingType;
    }
}