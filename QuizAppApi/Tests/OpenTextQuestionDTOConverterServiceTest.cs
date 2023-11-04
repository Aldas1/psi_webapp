using NUnit.Framework;
using QuizAppApi.DTOs;
using QuizAppApi.Exceptions;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class OpenTextQuestionDTOConverterServiceTests
{
    private OpenTextQuestionDTOConverterService _openTextConverter;
        
    [SetUp]
    public void Setup()
    {
        _openTextConverter = new OpenTextQuestionDTOConverterService();
    }

    [Test]
    public void CreateFromParameters_ReturnsOpenTextQuestion()
    {
        string validAnswer = "ValidAnswer?";

        var questionDTO = new QuestionParametersDTO
        {
            CorrectText = validAnswer
        };

        var result = _openTextConverter.CreateFromParameters(questionDTO);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OpenTextQuestion>(result);
        Assert.AreEqual(validAnswer, result.CorrectAnswer);
    }

    [Test]
    public void GenerateParameters_ReturnsQuestionParametersDTO()
    {
        string validAnswer = "ValidAnswer?";

        var question = new OpenTextQuestion
        {
            CorrectAnswer = validAnswer
        };

        var result = _openTextConverter.GenerateParameters(question);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<QuestionParametersDTO>(result);
        Assert.AreEqual(validAnswer, result.CorrectText);
    }
}