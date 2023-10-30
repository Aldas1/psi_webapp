using NUnit.Framework;
using QuizAppApi.DTOs;
using QuizAppApi.Exceptions;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class SingleChoiceQuestionDTOConverterServiceTests
{
    private SingleChoiceQuestionDTOConverterService _singleChoiceConverter;

    [SetUp]
    public void Setup()
    {
        _singleChoiceConverter = new SingleChoiceQuestionDTOConverterService();
    }

    [Test]
    public void CreateFromParameters_ReturnsSingleChoiceQuestion()
    {
        var questionDTO = new QuestionParametersDTO
        {
            Options = new List<string> { "Option1", "Option2", "Option3" },
            CorrectOptionIndex = 0
        };

        var result = _singleChoiceConverter.CreateFromParameters(questionDTO);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<SingleChoiceQuestion>(result);
        Assert.AreEqual(3, result.Options.Count);
        Assert.IsTrue(result.Options.Any(option => option.Name == "Option1"));
        Assert.IsTrue(result.Options.Any(option => option.Name == "Option2"));
        Assert.IsTrue(result.Options.Any(option => option.Name == "Option3"));
        Assert.IsTrue(result.Options.Any(option => option.Correct));
    }

    [Test]
    public void CreateFromParameters_ThrowsDTOConversionException()
    {
        var questionDTO = new QuestionParametersDTO
        {
            Options = new List<string> { "Option1", "Option2", "Option3" },
            CorrectOptionIndex = 3
        };

        Assert.Throws<DTOConversionException>(() => _singleChoiceConverter.CreateFromParameters(questionDTO));
    }

    [Test]
    public void GenerateParameters_ReturnsQuestionParametersDTO()
    {
        var question = new SingleChoiceQuestion
        {
            Options = new List<Option>
            {
                new Option { Name = "Option1", Correct = true },
                new Option { Name = "Option2", Correct = false },
            }
        };

        var result = _singleChoiceConverter.GenerateParameters(question);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<QuestionParametersDTO>(result);
        Assert.AreEqual(2, result.Options.Count);
        Assert.IsTrue(result.Options.Any(option => option == "Option1"));
        Assert.IsTrue(result.Options.Any(option => option == "Option2"));
        Assert.AreEqual(0, result.CorrectOptionIndex);
    }
}