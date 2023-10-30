using NUnit.Framework;
using QuizAppApi.DTOs;
using QuizAppApi.Exceptions;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;
using System.Linq;

namespace Tests;

[TestFixture]
public class SingleChoiceQuestionDTOConverterServiceTests
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
    public void CreateFromParameters_ReturnsSingleChoiceQuestion()
    {
        // Arrange
        var questionDTO = new QuestionParametersDTO
        {
            Options = new List<string> { "Option1", "Option2", "Option3" },
            CorrectOptionIndex = 0
        };

        // Act
        var result = _singleChoiceConverter.CreateFromParameters(questionDTO);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<SingleChoiceQuestion>(result);
        Assert.AreEqual(3, result.Options.Count);
        Assert.IsTrue(result.Options.All(option => option.Name == "Option1" || option.Name == "Option2" || option.Name == "Option3"));
        Assert.IsTrue(result.Options.Any(option => option.Correct));
    }

    [Test]
    public void CreateFromParameters_ThrowsDTOConversionException()
    {
        // Arrange
        var questionDTO = new QuestionParametersDTO
        {
            Options = new List<string> { "Option1", "Option2", "Option3" },
            CorrectOptionIndex = 3
        };

        // Act & Assert
        Assert.Throws<DTOConversionException>(() => _singleChoiceConverter.CreateFromParameters(questionDTO));
    }

    [Test]
    public void GenerateParameters_ReturnsQuestionParametersDTO()
    {
        // Arrange
        var question = new SingleChoiceQuestion
        {
            Options = new List<Option>
            {
                new Option { Name = "Option1", Correct = true },
                new Option { Name = "Option2", Correct = false },
            }
        };

        // Act
        var result = _singleChoiceConverter.GenerateParameters(question);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<QuestionParametersDTO>(result);
        Assert.AreEqual(2, result.Options.Count);
        Assert.IsTrue(result.Options.All(option => option == "Option1" || option == "Option2"));
        Assert.AreEqual(0, result.CorrectOptionIndex);
    }
}