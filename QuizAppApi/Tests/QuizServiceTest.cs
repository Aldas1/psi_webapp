using Moq;
using NUnit.Framework;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class QuizServiceTests
{
    private IQuizService? _quizService;
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
        _quizService = new QuizService(
            _mockQuizRepository.Object,
            _mockSingleChoiceQuestionDtoConverterService.Object,
            _mockMultipleChoiceQuestionDtoConverterService.Object,
            _mockOpenTextQuestionDtoConverterService.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _mockQuizRepository = null;
        _quizService = null;
    }

    [Test]
    public async Task CreateQuiz_ReturnsCorrectResponse()
    {
        var request = new QuizManipulationRequestDto
        {
            Name = "Test Quiz",
            Questions = new List<QuizManipulationQuestionRequestDto>
            {
                new QuizManipulationQuestionRequestDto
                {
                    QuestionText = "What is the capital of France?",
                    QuestionType = "singleChoiceQuestion",
                    QuestionParameters = new QuestionParametersDto
                    {
                        Options = new List<string> { "Paris", "London", "Berlin", "Madrid" },
                        CorrectOptionIndex = 0
                    }
                }
            }
        };
        var expectedResponse = new QuizManipulationResponseDto { Status = "success", Id = 1 };

        _mockQuizRepository.Setup(repo => repo.AddQuizAsync(It.IsAny<Quiz>())).ReturnsAsync(new Quiz { Id = 1 });
        _mockSingleChoiceQuestionDtoConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDto>())).Returns(new SingleChoiceQuestion());

        var result = await _quizService.CreateQuizAsync(request);

        Assert.AreEqual(expectedResponse.Status, result.Status);
        Assert.AreEqual(expectedResponse.Id, result.Id);
        _mockQuizRepository.Verify(repo => repo.AddQuizAsync(It.IsAny<Quiz>()), Times.Once);
    }

    [Test]
    public async Task GetQuizzes_ReturnsCorrectQuizzes()
    {
        var quizzes = new List<Quiz>
        {
            new Quiz { Id = 1, Name = "Quiz 1"},
            new Quiz { Id = 2, Name = "Quiz 2"}
        };

        _mockQuizRepository.Setup(repo => repo.GetQuizzesAsync()).ReturnsAsync(quizzes);

        var result = await _quizService.GetQuizzesAsync();

        Assert.AreEqual(quizzes.Count, result.Count());
        Assert.IsTrue(result.Any(q => q.Name == "Quiz 1"));
        Assert.IsTrue(result.Any(q => q.Name == "Quiz 2"));
    }

    [Test]
    public async Task DeleteQuiz_DeletesCorrectQuiz()
    {
        int quizIdToDelete = 1;
        var quizToDelete = new Quiz { Id = quizIdToDelete, Name = "Quiz that will soon be *poof*" };

        _mockQuizRepository.Setup(repo => repo.GetQuizByIdAsync(quizIdToDelete)).ReturnsAsync(quizToDelete);

        var result = await _quizService.DeleteQuizAsync(quizIdToDelete);

        Assert.IsTrue(result);
        _mockQuizRepository.Verify(repo => repo.GetQuizByIdAsync(quizIdToDelete), Times.Once);
        _mockQuizRepository.Verify(repo => repo.DeleteQuizAsync(quizIdToDelete), Times.Once);
    }

    [Test]
    public async Task GetQuiz_ReturnsProperQuiz()
    {
        int quizId = 10;
        var quiz = new Quiz { Id = quizId, Name = "Cool name" };
        _mockQuizRepository.Setup(repo => repo.GetQuizByIdAsync(quizId)).ReturnsAsync(quiz);

        var result = await _quizService.GetQuizAsync(quizId);

        Assert.AreEqual(quiz.Name, result.Name);
        Assert.AreEqual(quiz.Id, result.Id);
        _mockQuizRepository.Verify(repo => repo.GetQuizByIdAsync(quizId), Times.Once);
    }

    [Test]
    public async Task UpdateQuiz_SuccessfulUpdate()
    {
        var existingQuizId = 1;
        var existingQuiz = new Quiz { Id = existingQuizId, Name = "Old Name" };

        var newQuizData = new QuizManipulationRequestDto
        {
            Name = "New Name",
            Questions = new List<QuizManipulationQuestionRequestDto>
            {
                new QuizManipulationQuestionRequestDto
                {
                    QuestionText = "What is 2 + 2?",
                    QuestionType = "singleChoiceQuestion",
                    QuestionParameters = new QuestionParametersDto
                    {
                        Options = new List<string> { "3", "4", "5" },
                        CorrectOptionIndex = 1
                    }
                },
            }
        };

        _mockQuizRepository.Setup(repo => repo.GetQuizByIdAsync(existingQuizId)).ReturnsAsync(existingQuiz);
        _mockQuizRepository.Setup(repo => repo.SaveAsync()).Verifiable();
        _mockSingleChoiceQuestionDtoConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDto>())).Returns(new SingleChoiceQuestion());

        var result = await _quizService.UpdateQuizAsync(existingQuizId, newQuizData);

        Assert.AreEqual("success", result.Status);
        Assert.AreEqual(existingQuizId, result.Id);
        _mockQuizRepository.Verify(repo => repo.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateQuiz_QuizNotFound()
    {
        var nonExistentQuizId = 100;
        var editRequest = new QuizManipulationRequestDto
        {
        };

        _mockQuizRepository.Setup(repo => repo.GetQuizByIdAsync(nonExistentQuizId)).ReturnsAsync((Quiz)null);
        _mockSingleChoiceQuestionDtoConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDto>())).Returns(new SingleChoiceQuestion());

        var result = await _quizService.UpdateQuizAsync(nonExistentQuizId, editRequest);

        Assert.AreEqual("Quiz not found", result.Status);
    }

    [Test]
    public async Task UpdateQuiz_DataConversionException()
    {
        var existingQuizId = 1;
        var existingQuiz = new Quiz { Id = existingQuizId, Name = "Old Name" };

        var editRequest = new QuizManipulationRequestDto
        {
            Name = "New Name",
            Questions = new List<QuizManipulationQuestionRequestDto>
            {
                new QuizManipulationQuestionRequestDto
                {
                    QuestionText = null

                },

            }
        };

        _mockQuizRepository.Setup(repo => repo.GetQuizByIdAsync(existingQuizId)).ReturnsAsync(existingQuiz);
        _mockSingleChoiceQuestionDtoConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDto>())).Returns(new SingleChoiceQuestion());

        var result = await _quizService.UpdateQuizAsync(existingQuizId, editRequest);

        Assert.AreEqual("Failed to create a question", result.Status);
    }

    [Test]
    public async Task CanUserEditQuizAsync_UserCanEdit_ReturnsTrue()
    {
        var user = new User("TestUser", "Password");
        var quizId = 1;

        _mockQuizRepository.Setup(repo => repo.GetQuizByIdAsync(quizId)).ReturnsAsync(new Quiz { Id = quizId, Username = "TestUser" });

        var result = await _quizService.CanUserEditQuizAsync(user, quizId);

        Assert.IsTrue(result);
        _mockQuizRepository.Verify(repo => repo.GetQuizByIdAsync(quizId), Times.Once);
    }
}