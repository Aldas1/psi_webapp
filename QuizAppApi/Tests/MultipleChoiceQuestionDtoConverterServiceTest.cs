using NUnit.Framework;
using QuizAppApi.Dtos;
using QuizAppApi.Exceptions;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class MultipleChoiceQuestionDtoConverterServiceTests
{
    private MultipleChoiceQuestionDtoConverterService _multipleChoiceConverter;

    string optionName1 = "Option1";
    string optionName2 = "Option2";
    string optionName3 = "Option3";

    [SetUp]
    public void Setup()
    {
        _multipleChoiceConverter = new MultipleChoiceQuestionDtoConverterService();
    }

    [Test]
    public void CreateFromParameters_ReturnsMultipleChoiceQuestion()
    {
        var questionDto = new QuestionParametersDto
        {
            Options = new List<string> { optionName1, optionName2, optionName3 },
            CorrectOptionIndexes = new List<int> { 0, 2 }
        };

        var result = _multipleChoiceConverter.CreateFromParameters(questionDto);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<MultipleChoiceQuestion>(result);
        Assert.AreEqual(3, result.Options.Count);
        Assert.IsTrue(result.Options.Any(option => option.Name == optionName1));
        Assert.IsTrue(result.Options.Any(option => option.Name == optionName2));
        Assert.IsTrue(result.Options.Any(option => option.Name == optionName3));
        Assert.IsTrue(result.Options.Where(option => option.Correct).Count() == 2);
    }

    [Test]
    public void CreateFromParameters_ThrowsDtoConversionException()
    {
        var questionDto = new QuestionParametersDto
        {
            Options = new List<string> { optionName1, optionName2, optionName3 },
            CorrectOptionIndexes = new List<int> { 0, 3 }
        };

        Assert.Throws<DtoConversionException>(() => _multipleChoiceConverter.CreateFromParameters(questionDto));
    }

    [Test]
    public void GenerateParameters_ReturnsQuestionParametersDto()
    {
        var question = new MultipleChoiceQuestion
        {
            Options = new List<Option>
            {
                new Option { Name = optionName1, Correct = true },
                new Option { Name = optionName2, Correct = false },
                new Option { Name = optionName3, Correct = true },
            }
        };

        var result = _multipleChoiceConverter.GenerateParameters(question);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<QuestionParametersDto>(result);
        Assert.AreEqual(3, result.Options.Count);
        Assert.IsTrue(result.Options.Any(option => option == optionName1));
        Assert.IsTrue(result.Options.Any(option => option == optionName2));
        Assert.IsTrue(result.Options.Any(option => option == optionName3));
        Assert.IsTrue(result.CorrectOptionIndexes.Count() == 2);
    }
}