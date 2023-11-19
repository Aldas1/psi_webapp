using NUnit.Framework;
using QuizAppApi.Dtos;
using QuizAppApi.Exceptions;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class SingleChoiceQuestionDtoConverterServiceTests
{
    private SingleChoiceQuestionDtoConverterService _singleChoiceConverter;

    [SetUp]
    public void Setup()
    {
        _singleChoiceConverter = new SingleChoiceQuestionDtoConverterService();
    }

    [Test]
    public void CreateFromParameters_ReturnsSingleChoiceQuestion()
    {
        var questionDto = new QuestionParametersDto
        {
            Options = new List<string> { "Option1", "Option2", "Option3" },
            CorrectOptionIndex = 0
        };

        var result = _singleChoiceConverter.CreateFromParameters(questionDto);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<SingleChoiceQuestion>(result);
        Assert.AreEqual(3, result.Options.Count);
        Assert.IsTrue(result.Options.Any(option => option.Name == "Option1"));
        Assert.IsTrue(result.Options.Any(option => option.Name == "Option2"));
        Assert.IsTrue(result.Options.Any(option => option.Name == "Option3"));
        Assert.IsTrue(result.Options.Any(option => option.Correct));
    }

    [Test]
    public void CreateFromParameters_ThrowsDtoConversionException()
    {
        var questionDto = new QuestionParametersDto
        {
            Options = new List<string> { "Option1", "Option2", "Option3" },
            CorrectOptionIndex = 3
        };

        Assert.Throws<DtoConversionException>(() => _singleChoiceConverter.CreateFromParameters(questionDto));
    }

    [Test]
    public void GenerateParameters_ReturnsQuestionParametersDto()
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

        var optionNames = result.Options.Select(option => option).ToHashSet();
        var expectedOptionNames = question.Options.Select(option => option.Name).ToHashSet();
        Assert.AreEqual(expectedOptionNames, optionNames);
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<QuestionParametersDto>(result);
        Assert.AreEqual(2, result.Options.Count);
        Assert.AreEqual(0, result.CorrectOptionIndex);
    }
}