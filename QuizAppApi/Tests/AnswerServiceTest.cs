using Moq;
using NUnit.Framework;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class AnswerServiceTest
{
    private IAnswerService _answerService = null!;
    private Mock<IQuizRepository> _mockQuizRepository = null!;
    private Mock<IAnswerCheckerService> _mockAnswerCheckerService = null!;

    [SetUp]
    public void Setup()
    {
        _mockQuizRepository = new Mock<IQuizRepository>();
        _mockAnswerCheckerService = new Mock<IAnswerCheckerService>();
        _answerService = new AnswerService(_mockQuizRepository.Object, _mockAnswerCheckerService.Object);
    }
    
    [Test]
    public async Task SubmitAnswers_ReturnsErrorForNonexistentQuiz_1()
    {
        var answerRequest = new List<AnswerSubmitRequestDto>
        {
            new AnswerSubmitRequestDto { QuestionId = 1, OptionName = "Paris" }
        };
        var expectedResponse = new AnswerSubmitResponseDto { Status = "success" };
    
        var questions = new List<Question>
        {
            new SingleChoiceQuestion
            {
                Id = 1,
            },
            new SingleChoiceQuestion
            {
                Id = 2,
            },
            new SingleChoiceQuestion
            {
                Id = 3,
            }
        };
    
        var fakeQuiz = new Quiz
        {
            Id = 1,
            Name = "Fake Quiz",
            Questions = questions
        };
    
        _mockQuizRepository.Setup(repo => repo.GetQuizByIdAsync(It.IsAny<int>())).ReturnsAsync(fakeQuiz);
    
        var result = await _answerService.SubmitAnswersAsync(1, answerRequest);
        _mockAnswerCheckerService.Verify(mock => mock.CheckSingleChoiceAnswer(It.IsAny<SingleChoiceQuestion>(), It.IsAny<string>()), Times.Once);
    
        Assert.AreEqual(expectedResponse.Status, result.Status);
        _mockQuizRepository.Verify(repo => repo.GetQuizByIdAsync(It.IsAny<int>()), Times.Once);
    }
    
    [Test]
    public async Task SubmitAnswers_ReturnsErrorForNonexistentQuiz_2()
    {
        var answerRequest = new List<AnswerSubmitRequestDto>
        {
            new AnswerSubmitRequestDto
            {
                QuestionId = 1,
                OptionNames = new List<string> { "Option1", "Option2" }
            }
        };
    
        var expectedResponse = new AnswerSubmitResponseDto { Status = "success" };
    
        var questions = new List<Question>
        {
            new MultipleChoiceQuestion
            {
                Id = 1,
            },
            new MultipleChoiceQuestion
            {
                Id = 2,
            },
            new MultipleChoiceQuestion
            {
                Id = 3,
            }
        };
    
        var fakeQuiz = new Quiz
        {
            Id = 2,
            Name = "Fake Quiz",
            Questions = questions
        };
    
        _mockQuizRepository.Setup(repo => repo.GetQuizByIdAsync(It.IsAny<int>())).ReturnsAsync(fakeQuiz);
    
        var result = await _answerService.SubmitAnswersAsync(1, answerRequest);
        _mockAnswerCheckerService.Verify(mock => mock.CheckMultipleChoiceAnswer(It.IsAny<MultipleChoiceQuestion>(), It.IsAny<List<Option>>()), Times.Once);
    
        Assert.AreEqual(expectedResponse.Status, result.Status);
        _mockQuizRepository.Verify(repo => repo.GetQuizByIdAsync(It.IsAny<int>()), Times.Once);
    }
    
    [Test]
    public async Task SubmitAnswers_ReturnsErrorForNonexistentQuiz_3()
    {
        var answerRequest = new List<AnswerSubmitRequestDto>
        {
            new AnswerSubmitRequestDto
            {
                QuestionId = 1,
                AnswerText = "Your answer goes here"
            }
        };
    
        var expectedResponse = new AnswerSubmitResponseDto { Status = "success" };
    
        var questions = new List<Question>
        {
            new OpenTextQuestion
            {
                Id = 1,
            },
            new OpenTextQuestion
            {
                Id = 2,
            },
            new OpenTextQuestion
            {
                Id = 3,
            }
        };
    
        var fakeQuiz = new Quiz
        {
            Id = 2,
            Name = "Fake Quiz",
            Questions = questions
        };
    
        _mockQuizRepository.Setup(repo => repo.GetQuizByIdAsync(It.IsAny<int>())).ReturnsAsync(fakeQuiz);
    
        var result = await _answerService.SubmitAnswersAsync(1, answerRequest);
        _mockAnswerCheckerService.Verify(mock => mock.CheckOpenTextAnswer(It.IsAny<OpenTextQuestion>(), It.IsAny<string>(), true, true), Times.Once);
    
        Assert.AreEqual(expectedResponse.Status, result.Status);
        _mockQuizRepository.Verify(repo => repo.GetQuizByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task SubmitAnswers_CalculatesCorrectlyAnswered()
    {
        var answerRequest = new List<AnswerSubmitRequestDto>
    {
        new AnswerSubmitRequestDto { QuestionId = 1, OptionName = "CorrectOption" },
        new AnswerSubmitRequestDto { QuestionId = 2, OptionName = "IncorrectOption" },
        new AnswerSubmitRequestDto { QuestionId = 3, OptionName = "CorrectOption" }
    };

        var questions = new List<Question>
    {
        new SingleChoiceQuestion
        {
            Id = 1,
            Options = new List<Option>
            {
                new Option { Name = "CorrectOption", Correct = true },
                new Option { Name = "IncorrectOption", Correct = false }
            }
        },
        new SingleChoiceQuestion
        {
            Id = 2,
            Options = new List<Option>
            {
                new Option { Name = "CorrectOption", Correct = true },
                new Option { Name = "IncorrectOption", Correct = false }
            }
        },
        new SingleChoiceQuestion
        {
            Id = 3,
            Options = new List<Option>
            {
                new Option { Name = "CorrectOption", Correct = true },
                new Option { Name = "IncorrectOption", Correct = false }
            }
        }
    };

        var fakeQuiz = new Quiz
        {
            Id = 1,
            Name = "Fake Quiz",
            Questions = questions
        };

        _mockQuizRepository.Setup(repo => repo.GetQuizByIdAsync(It.IsAny<int>())).ReturnsAsync(fakeQuiz);

        _mockAnswerCheckerService.Setup(mock => mock.CheckSingleChoiceAnswer(It.IsAny<SingleChoiceQuestion>(), It.IsAny<string>()))
            .Returns<SingleChoiceQuestion, string>((question, optionName) =>
                question.Options.Any(o => o.Name == optionName && o.Correct));

        var result = await _answerService.SubmitAnswersAsync(1, answerRequest, "fakeUsername");

        Assert.AreEqual(2, result.CorrectlyAnswered);
        _mockQuizRepository.Verify(repo => repo.GetQuizByIdAsync(It.IsAny<int>()), Times.Once);
        _mockAnswerCheckerService.Verify(mock => mock.CheckSingleChoiceAnswer(It.IsAny<SingleChoiceQuestion>(), It.IsAny<string>()), Times.Exactly(3));
    }
}