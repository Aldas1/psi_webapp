using Moq;
using NUnit.Framework;
using QuizAppApi.DTOs;
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
    private Mock<IQuestionDTOConverterService<SingleChoiceQuestion>>? _mockSingleChoiceQuestionDTOConverterService;
    private Mock<IQuestionDTOConverterService<MultipleChoiceQuestion>>? _mockMultipleChoiceQuestionDTOConverterService;
    private Mock<IQuestionDTOConverterService<OpenTextQuestion>>? _mockOpenTextQuestionDTOConverterService;

    [SetUp]
    public void Setup()
    {
        _mockQuizRepository = new Mock<IQuizRepository>();
        _mockSingleChoiceQuestionDTOConverterService = new Mock<IQuestionDTOConverterService<SingleChoiceQuestion>>();
        _mockMultipleChoiceQuestionDTOConverterService = new Mock<IQuestionDTOConverterService<MultipleChoiceQuestion>>();
        _mockOpenTextQuestionDTOConverterService = new Mock<IQuestionDTOConverterService<OpenTextQuestion>>();

        _questionService = new QuestionService(
            _mockQuizRepository.Object,
            _mockSingleChoiceQuestionDTOConverterService.Object,
            _mockMultipleChoiceQuestionDTOConverterService.Object,
            _mockOpenTextQuestionDTOConverterService.Object
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

        //_mockQuizRepository.Setup(repo => repo.AddQuiz(It.IsAny<Quiz>())).;

        var expectedQuestions = new List<QuestionResponseDTO>
        {
            new QuestionResponseDTO
            {
                Id = 0,
                QuestionText = "The one and only, question 1",
                QuestionType = QuestionTypeConverter.ToString(QuestionType.SingleChoiceQuestion),
                QuestionParameters = new QuestionParametersDTO
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

        _mockQuizRepository.Setup(repo => repo.GetQuizById(0)).Returns(mockQuiz);

        _mockSingleChoiceQuestionDTOConverterService.Setup(service => service.GenerateParameters(It.IsAny<SingleChoiceQuestion>()))
            .Returns(new QuestionParametersDTO
            {
                Options = new List<string>
                    {
                        "Option numero uno",
                        "Option correcto"
                    },
                CorrectOptionIndex = 1
            });

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

        _mockQuizRepository.Verify(repo => repo.GetQuizById(0), Times.Once);
    }
}
