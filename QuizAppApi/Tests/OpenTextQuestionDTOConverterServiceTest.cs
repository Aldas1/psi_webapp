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

    string validAnswer = "ValidAnswer?";
        
    [SetUp]
    public void Setup()
    {
        _openTextConverter = new OpenTextQuestionDTOConverterService();
    }

    [Test]
    public void CreateFromParameters_ReturnsOpenTextQuestion()
    {
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