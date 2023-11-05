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
        var questionDTO = new QuestionParametersDTO
        {
            CorrectText = "ValidAnswer?"
        };

        var result = _openTextConverter.CreateFromParameters(questionDTO);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OpenTextQuestion>(result);
        Assert.AreEqual(questionDTO.CorrectText, result.CorrectAnswer);
    }

    [Test]
    public void GenerateParameters_ReturnsQuestionParametersDTO()
    {
        var question = new OpenTextQuestion
        {
            CorrectAnswer = "ValidAnswer?"
        };

        var result = _openTextConverter.GenerateParameters(question);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<QuestionParametersDTO>(result);
        Assert.AreEqual(question.CorrectAnswer, result.CorrectText);
    }
}