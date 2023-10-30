using NUnit.Framework;
using QuizAppApi.DTOs;
using QuizAppApi.Exceptions;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;
using System.Linq;

namespace Tests;

[TestFixture]
public class OpenTextQuestionDTOConverterServiceTests
{
    private SingleChoiceQuestionDTOConverterService _singleChoiceConverter;
    private MultipleChoiceQuestionDTOConverterService _multipleChoiceConverter;
    private OpenTextQuestionDTOConverterService _openTextConverter;

    [SetUp]
    public void Setup()
    {
        _singleChoiceConverter = new SingleChoiceQuestionDTOConverterService();
        _multipleChoiceConverter = new MultipleChoiceQuestionDTOConverterService();
        _openTextConverter = new OpenTextQuestionDTOConverterService();
    }

    [Test]
    public void CreateFromParameters_ReturnsOpenTextQuestion()
    {
        // Arrange
        var questionDTO = new QuestionParametersDTO
        {
            CorrectText = "ValidAnswer?"
        };

        // Act
        var result = _openTextConverter.CreateFromParameters(questionDTO);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OpenTextQuestion>(result);
        Assert.AreEqual("ValidAnswer?", result.CorrectAnswer);
    }

    [Test]
    public void CreateFromParameters_ThrowsDTOConversionException()
    {
        // Arrange
        var questionDTO = new QuestionParametersDTO
        {
            CorrectText = "#InvalidAnswer"
        };

        // Act & Assert
        Assert.Throws<DTOConversionException>(() => _openTextConverter.CreateFromParameters(questionDTO));
    }

    [Test]
    public void GenerateParameters_ReturnsQuestionParametersDTO()
    {
        // Arrange
        var question = new OpenTextQuestion
        {
            CorrectAnswer = "ValidAnswer?"
        };

        // Act
        var result = _openTextConverter.GenerateParameters(question);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<QuestionParametersDTO>(result);
        Assert.AreEqual("ValidAnswer?", result.CorrectText);
    }
}