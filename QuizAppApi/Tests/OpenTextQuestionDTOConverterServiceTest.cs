using NUnit.Framework;
using QuizAppApi.Dtos;
using QuizAppApi.Exceptions;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class OpenTextQuestionDtoConverterServiceTests
{
    private OpenTextQuestionDtoConverterService _openTextConverter;
        
    [SetUp]
    public void Setup()
    {
        _openTextConverter = new OpenTextQuestionDtoConverterService();
    }

    [Test]
    public void CreateFromParameters_ReturnsOpenTextQuestion()
    {
        var questionDto = new QuestionParametersDto
        {
            CorrectText = "ValidAnswer?"
        };

        var result = _openTextConverter.CreateFromParameters(questionDto);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OpenTextQuestion>(result);
        Assert.AreEqual(questionDto.CorrectText, result.CorrectAnswer);
    }

    [Test]
    public void GenerateParameters_ReturnsQuestionParametersDto()
    {
        var question = new OpenTextQuestion
        {
            CorrectAnswer = "ValidAnswer?"
        };

        var result = _openTextConverter.GenerateParameters(question);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<QuestionParametersDto>(result);
        Assert.AreEqual(question.CorrectAnswer, result.CorrectText);
    }
}