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

    string option1 = "Option1";
    string option2 = "Option2";
    string option3 = "Option3";

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
            Options = new List<string> { option1, option2, option3 },
            CorrectOptionIndexes = new List<int> { 0, 2 }
        };

        var result = _multipleChoiceConverter.CreateFromParameters(questionDTO);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<MultipleChoiceQuestion>(result);
        Assert.AreEqual(3, result.Options.Count);
        Assert.IsTrue(result.Options.Any(option => option.Name == option1));
        Assert.IsTrue(result.Options.Any(option => option.Name == option2));
        Assert.IsTrue(result.Options.Any(option => option.Name == option3));
        Assert.IsTrue(result.Options.Where(option => option.Correct).Count() == 2);
    }

    [Test]
    public void CreateFromParameters_ThrowsDTOConversionException()
    {
        var questionDTO = new QuestionParametersDTO
        {
            Options = new List<string> { option1, option2, option3 },
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
                new Option { Name = option1, Correct = true },
                new Option { Name = option2, Correct = false },
                new Option { Name = option3, Correct = true },
            }
        };

        var result = _multipleChoiceConverter.GenerateParameters(question);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<QuestionParametersDTO>(result);
        Assert.AreEqual(3, result.Options.Count);
        Assert.IsTrue(result.Options.Any(option => option == option1));
        Assert.IsTrue(result.Options.Any(option => option == option2));
        Assert.IsTrue(result.Options.Any(option => option == option3));
        Assert.IsTrue(result.CorrectOptionIndexes.Count() == 2);
    }
}