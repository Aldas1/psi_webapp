using Moq;
using NUnit.Framework;
using QuizAppApi.Dtos;
using QuizAppApi.Enums;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;
using QuizAppApi.Utils;

namespace Tests;

[TestFixture]
public class QuestionServiceTest
{
    private IQuestionService? _questionService;
    private Mock<IQuizRepository>? _mockQuizRepository;
    private Mock<IQuestionDtoConverterService<SingleChoiceQuestion>>? _mockSingleChoiceQuestionDtoConverterService;
    private Mock<IQuestionDtoConverterService<MultipleChoiceQuestion>>? _mockMultipleChoiceQuestionDtoConverterService;
    private Mock<IQuestionDtoConverterService<OpenTextQuestion>>? _mockOpenTextQuestionDtoConverterService;

    [SetUp]
    public void Setup()
    {
        _mockQuizRepository = new Mock<IQuizRepository>();
        _mockSingleChoiceQuestionDtoConverterService = new Mock<IQuestionDtoConverterService<SingleChoiceQuestion>>();
        _mockMultipleChoiceQuestionDtoConverterService = new Mock<IQuestionDtoConverterService<MultipleChoiceQuestion>>();
        _mockOpenTextQuestionDtoConverterService = new Mock<IQuestionDtoConverterService<OpenTextQuestion>>();

        _questionService = new QuestionService(
            _mockQuizRepository.Object,
            _mockSingleChoiceQuestionDtoConverterService.Object,
            _mockMultipleChoiceQuestionDtoConverterService.Object,
            _mockOpenTextQuestionDtoConverterService.Object
        );
    }

    [TearDown]
    public void TearDown()
    {
        _mockQuizRepository = null;
        _questionService = null;
    }

    [Test]
    public void GetQuestions_ReturnsCorrectQuestions()
    {
        var mockQuiz = new Quiz
        {
            Id = 0,
            Name = "Test quiz <3",
            Questions = new List<Question>
            {
                new SingleChoiceQuestion
                {
                    Id = 0,
                    Text = "The one and only, question 1",
                    Options = new List<Option>
                    {
                        new Option
                        {
                            Name = "Option numero uno",
                            Correct = false
                        },
                        new Option
                        {
                            Name = "Option correcto",
                            Correct = true
                        }
                    }
                }
            }
        };

        var expectedQuestions = new List<QuestionResponseDto>
        {
            new QuestionResponseDto
            {
                Id = 0,
                QuestionText = "The one and only, question 1",
                QuestionType = QuestionTypeConverter.ToString(QuestionType.SingleChoiceQuestion),
                QuestionParameters = new QuestionParametersDto
                {
                    Options = new List<string>
                    {
                        "Option numero uno",
                        "Option correcto"
                    },
                    CorrectOptionIndex = 1
                }
            }
        };

        _mockQuizRepository.Setup(repo => repo.GetQuizByIdAsync(0)).Returns(mockQuiz);

        _mockSingleChoiceQuestionDtoConverterService.Setup(service => service.GenerateParameters(It.IsAny<SingleChoiceQuestion>()))
            .Returns(expectedQuestions.ElementAt(0).QuestionParameters);

        var result = _questionService.GetQuestions(0);

        Assert.IsNotNull(expectedQuestions);
        Assert.IsNotNull(result);

        Assert.AreEqual(expectedQuestions.Count, result.Count());
        Assert.AreEqual(expectedQuestions.ElementAtOrDefault(0).Id, result.ElementAtOrDefault(0).Id);
        Assert.AreEqual(expectedQuestions.ElementAtOrDefault(0).QuestionText, result.ElementAtOrDefault(0).QuestionText);
        Assert.AreEqual(expectedQuestions.ElementAtOrDefault(0).QuestionType, result.ElementAtOrDefault(0).QuestionType);

        Assert.AreEqual(
            expectedQuestions.ElementAtOrDefault(0).QuestionParameters.Options.ElementAtOrDefault(0),
            result.ElementAtOrDefault(0).QuestionParameters.Options.ElementAtOrDefault(0));

        Assert.AreEqual(
            expectedQuestions.ElementAtOrDefault(0).QuestionParameters.Options.ElementAtOrDefault(1),
            result.ElementAtOrDefault(0).QuestionParameters.Options.ElementAtOrDefault(1));

        Assert.AreEqual(
            expectedQuestions.ElementAtOrDefault(0).QuestionParameters.CorrectOptionIndex,
            result.ElementAtOrDefault(0).QuestionParameters.CorrectOptionIndex);

        _mockQuizRepository.Verify(repo => repo.GetQuizByIdAsync(0), Times.Once);
    }
}
