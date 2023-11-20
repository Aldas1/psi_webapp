using NUnit.Framework;
using QuizAppApi.Exceptions;
using System;

namespace QuizAppApi.Tests.Exceptions;

[TestFixture]
public class TypeMismatchExceptionTests
{
    [Test]
    public void Constructor_SetsAttemptedType()
    {
        Type attemptedType = typeof(string);
        Type existingType = typeof(int);

        TypeMismatchException exception = new TypeMismatchException(attemptedType, existingType);

        Assert.AreEqual(attemptedType, exception.AttemptedType);
    }

    [Test]
    public void Constructor_SetsExistingType()
    {
        Type attemptedType = typeof(string);
        Type existingType = typeof(int);

        TypeMismatchException exception = new TypeMismatchException(attemptedType, existingType);

        Assert.AreEqual(existingType, exception.ExistingType);
    }

    [Test]
    public void Constructor_SetsErrorMessage()
    {
        Type attemptedType = typeof(string);
        Type existingType = typeof(int);

        TypeMismatchException exception = new TypeMismatchException(attemptedType, existingType);

        string expectedErrorMessage = $"Attempted to access an object of type {attemptedType} with a key that contains objects of type {existingType}.";
        Assert.AreEqual(expectedErrorMessage, exception.Message);
    }
}