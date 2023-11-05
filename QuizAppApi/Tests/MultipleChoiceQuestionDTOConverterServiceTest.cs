using NUnit.Framework;
using QuizAppApi.DTOs;
using QuizAppApi.Exceptions;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class MultipleChoiceQuestionDTOConverterServiceTests
{
    private MultipleChoiceQuestionDTOConverterService _multipleChoiceConverter;

    string optionName1 = "Option1";
    string optionName2 = "Option2";
    string optionName3 = "Option3";

    [SetUp]
    public void Setup()
    {
        _multipleChoiceConverter = new MultipleChoiceQuestionDTOConverterService();
    }

    [Test]
    public void CreateFromParameters_ReturnsMultipleChoiceQuestion()
    {
        var questionDTO = new QuestionParametersDTO
        {
            Options = new List<string> { optionName1, optionName2, optionName3 },
            CorrectOptionIndexes = new List<int> { 0, 2 }
        };

        var result = _multipleChoiceConverter.CreateFromParameters(questionDTO);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<MultipleChoiceQuestion>(result);
        Assert.AreEqual(3, result.Options.Count);
        Assert.IsTrue(result.Options.Any(option => option.Name == optionName1));
        Assert.IsTrue(result.Options.Any(option => option.Name == optionName2));
        Assert.IsTrue(result.Options.Any(option => option.Name == optionName3));
        Assert.IsTrue(result.Options.Where(option => option.Correct).Count() == 2);
    }

    [Test]
    public void CreateFromParameters_ThrowsDTOConversionException()
    {
        var questionDTO = new QuestionParametersDTO
        {
            Options = new List<string> { optionName1, optionName2, optionName3 },
            CorrectOptionIndexes = new List<int> { 0, 3 }
        };

        Assert.Throws<DTOConversionException>(() => _multipleChoiceConverter.CreateFromParameters(questionDTO));
    }

    [Test]
    public void GenerateParameters_ReturnsQuestionParametersDTO()
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
        Assert.IsInstanceOf<QuestionParametersDTO>(result);
        Assert.AreEqual(3, result.Options.Count);
        Assert.IsTrue(result.Options.Any(option => option == optionName1));
        Assert.IsTrue(result.Options.Any(option => option == optionName2));
        Assert.IsTrue(result.Options.Any(option => option == optionName3));
        Assert.IsTrue(result.CorrectOptionIndexes.Count() == 2);
    }
}