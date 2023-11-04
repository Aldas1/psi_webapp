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
        string option1 = "Option1";
        string option2 = "Option2";
        string option3 = "Option3";

        var questionDTO = new QuestionParametersDTO
        {
            Options = new List<string> { option1, option2, option3 },
            CorrectOptionIndex = 0
        };

        var result = _singleChoiceConverter.CreateFromParameters(questionDTO);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<SingleChoiceQuestion>(result);
        Assert.AreEqual(3, result.Options.Count);
        Assert.IsTrue(result.Options.Any(option => option.Name == option1));
        Assert.IsTrue(result.Options.Any(option => option.Name == option2));
        Assert.IsTrue(result.Options.Any(option => option.Name == option3));
        Assert.IsTrue(result.Options.Any(option => option.Correct));
    }

    [Test]
    public void CreateFromParameters_ThrowsDTOConversionException()
    {
        string option1 = "Option1";
        string option2 = "Option2";
        string option3 = "Option3";

        var questionDTO = new QuestionParametersDTO
        {
            Options = new List<string> { option1, option2, option3 },
            CorrectOptionIndex = 3
        };

        Assert.Throws<DTOConversionException>(() => _singleChoiceConverter.CreateFromParameters(questionDTO));
    }

    [Test]
    public void GenerateParameters_ReturnsQuestionParametersDTO()
    {
        string option1 = "Option1";
        string option2 = "Option2";

        var question = new SingleChoiceQuestion
        {
            Options = new List<Option>
            {
                new Option { Name = option1, Correct = true },
                new Option { Name = option2, Correct = false },
            }
        };

        var result = _singleChoiceConverter.GenerateParameters(question);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<QuestionParametersDTO>(result);
        Assert.AreEqual(2, result.Options.Count);
        Assert.IsTrue(result.Options.Any(option => option == option1));
        Assert.IsTrue(result.Options.Any(option => option == option2));
        Assert.AreEqual(0, result.CorrectOptionIndex);
    }
}