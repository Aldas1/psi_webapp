using NUnit.Framework;
using QuizAppApi.Enums;
using QuizAppApi.Utils;
using QuizAppApi;

namespace Tests;

[TestFixture]
public class QuestionTypeConverterTests
{
    [Test]
    public void ToString_ReturnsCorrectStringForMultipleChoiceQuestion()
    {
        var type = QuestionType.MultipleChoiceQuestion;

        var result = QuestionTypeConverter.ToString(type);

        Assert.AreEqual(Config.QuestionTypeValues.MultipleChoiceQuestionApiString, result);
    }

    [Test]
    public void ToString_ReturnsCorrectStringForOpenTextQuestion()
    {
        var type = QuestionType.OpenTextQuestion;

        var result = QuestionTypeConverter.ToString(type);

        Assert.AreEqual(Config.QuestionTypeValues.OpenTextQuestionApiString, result);
    }

    [Test]
    public void ToReadableString_ReturnsCorrectStringForSingleChoiceQuestion()
    {
        var type = QuestionType.SingleChoiceQuestion;

        var result = QuestionTypeConverter.ToReadableString(type);

        Assert.AreEqual(Config.ReadableQuestionTypeValues.SingleChoiceQuestionApiString, result);
    }

    [Test]
    public void ToReadableString_ReturnsCorrectStringForOpenTextQuestion()
    {
        var type = QuestionType.OpenTextQuestion;

        var result = QuestionTypeConverter.ToReadableString(type);

        Assert.AreEqual(Config.ReadableQuestionTypeValues.OpenTextQuestionApiString, result);
    }

    [Test]
    public void FromString_ReturnsCorrectEnumForSingleChoiceQuestion()
    {
        var typeString = Config.QuestionTypeValues.SingleChoiceQuestionApiString;

        var result = QuestionTypeConverter.FromString(typeString);

        Assert.AreEqual(QuestionType.SingleChoiceQuestion, result);
    }

    [Test]
    public void FromString_ReturnsUnknownEnumForInvalidString()
    {
        var invalidTypeString = "InvalidType";

        var result = QuestionTypeConverter.FromString(invalidTypeString);

        Assert.AreEqual(QuestionType.Unknown, result);
    }
}