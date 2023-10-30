using NUnit.Framework;
using QuizAppApi.DTOs;
using QuizAppApi.Exceptions;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;
using System.Linq;

namespace Tests;

[TestFixture]
public class MultipleChoiceQuestionDTOConverterServiceTests
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
    public void CreateFromParameters_ReturnsMultipleChoiceQuestion()
    {
        // Arrange
        var questionDTO = new QuestionParametersDTO
        {
            Options = new List<string> { "Option1", "Option2", "Option3" },
            CorrectOptionIndexes = new List<int> { 0, 2 }
        };

        // Act
        var result = _multipleChoiceConverter.CreateFromParameters(questionDTO);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<MultipleChoiceQuestion>(result);
        Assert.AreEqual(3, result.Options.Count);
        Assert.IsTrue(result.Options.All(option => option.Name == "Option1" || option.Name == "Option2" || option.Name == "Option3"));
        Assert.IsTrue(result.Options.Where(option => option.Correct).Count() == 2);
    }

    [Test]
    public void CreateFromParameters_ThrowsDTOConversionException()
    {
        // Arrange
        var questionDTO = new QuestionParametersDTO
        {
            Options = new List<string> { "Option1", "Option2", "Option3" },
            CorrectOptionIndexes = new List<int> { 0, 3 } // Index 3 is out of bounds
        };

        // Act & Assert
        Assert.Throws<DTOConversionException>(() => _multipleChoiceConverter.CreateFromParameters(questionDTO));
    }

    [Test]
    public void GenerateParameters_ReturnsQuestionParametersDTO()
    {
        // Arrange
        var question = new MultipleChoiceQuestion
        {
            Options = new List<Option>
            {
                new Option { Name = "Option1", Correct = true },
                new Option { Name = "Option2", Correct = false },
                new Option { Name = "Option3", Correct = true },
            }
        };

        // Act
        var result = _multipleChoiceConverter.GenerateParameters(question);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<QuestionParametersDTO>(result);
        Assert.AreEqual(3, result.Options.Count);
        Assert.IsTrue(result.Options.All(option => option == "Option1" || option == "Option2" || option == "Option3"));
        Assert.IsTrue(result.CorrectOptionIndexes.Count() == 2);
    }
}