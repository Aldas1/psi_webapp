using Moq;
using NUnit.Framework;
using QuizAppApi.Dtos;
using QuizAppApi.Dtos.Responses;
using QuizAppApi.Dtos.Requests;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class QuizServiceTests
{
    private IQuizService? _quizService;
    private Mock<IExplanationService>? _mockChatGptService;
    private Mock<IQuizRepository>? _mockQuizRepository;
    private Mock<IQuestionDtoConverterService<SingleChoiceQuestion>>? _mockSingleChoiceQuestionDtoConverterService;
    private Mock<IQuestionDtoConverterService<MultipleChoiceQuestion>>? _mockMultipleChoiceQuestionDtoConverterService;
    private Mock<IQuestionDtoConverterService<OpenTextQuestion>>? _mockOpenTextQuestionDtoConverterService;
    private Mock<IAnswerCheckerService>? _mockAnswerCheckerService;

    [SetUp]
    public void Setup()
    {
        _mockQuizRepository = new Mock<IQuizRepository>();
        _mockChatGptService = new Mock<IExplanationService>();
        _mockSingleChoiceQuestionDtoConverterService = new Mock<IQuestionDtoConverterService<SingleChoiceQuestion>>();
        _mockMultipleChoiceQuestionDtoConverterService = new Mock<IQuestionDtoConverterService<MultipleChoiceQuestion>>();
        _mockOpenTextQuestionDtoConverterService = new Mock<IQuestionDtoConverterService<OpenTextQuestion>>();
        _mockAnswerCheckerService = new Mock<IAnswerCheckerService>();
        _quizService = new QuizService(
            _mockQuizRepository.Object,
            _mockChatGptService.Object,
            _mockSingleChoiceQuestionDtoConverterService.Object,
            _mockMultipleChoiceQuestionDtoConverterService.Object,
            _mockOpenTextQuestionDtoConverterService.Object,
            _mockAnswerCheckerService.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _mockQuizRepository = null;
        _quizService = null;
    }

    [Test]
    public void CreateQuiz_ReturnsCorrectResponse()
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

        _mockQuizRepository.Setup(repo => repo.AddQuiz(It.IsAny<Quiz>())).Returns(new Quiz { Id = 1 });
        _mockSingleChoiceQuestionDtoConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDto>())).Returns(new SingleChoiceQuestion());

        var result = _quizService.CreateQuiz(request);

        Assert.AreEqual(expectedResponse.Status, result.Status);
        Assert.AreEqual(expectedResponse.Id, result.Id);
        _mockQuizRepository.Verify(repo => repo.AddQuiz(It.IsAny<Quiz>()), Times.Once);
    }

    [Test]
    public void GetQuiz_ReturnsCorrectQuiz()
    {
        var createdQuiz = new Quiz { Id = 1, Name = "Test Quiz" };
        _mockQuizRepository.Setup(repo => repo.AddQuiz(It.IsAny<Quiz>())).Returns(createdQuiz);

        var expectedQuizzes = new List<Quiz> { createdQuiz };
        _mockQuizRepository.Setup(repo => repo.GetQuizzes()).Returns(expectedQuizzes);

        var result = _quizService.GetQuizzes().FirstOrDefault(q => q.Id == createdQuiz.Id);

        Assert.IsNotNull(createdQuiz);
        Assert.IsNotNull(result);
        _mockQuizRepository.Verify(repo => repo.GetQuizzes(), Times.Once);
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

        _mockQuizRepository.Setup(repo => repo.GetQuizById(It.IsAny<int>())).Returns(fakeQuiz);

        var result = await _quizService.SubmitAnswers(1, answerRequest);
        _mockAnswerCheckerService.Verify(mock => mock.CheckSingleChoiceAnswer(It.IsAny<SingleChoiceQuestion>(), It.IsAny<string>()), Times.Once);

        Assert.AreEqual(expectedResponse.Status, result.Status);
        _mockQuizRepository.Verify(repo => repo.GetQuizById(It.IsAny<int>()), Times.Once);
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

        _mockQuizRepository.Setup(repo => repo.GetQuizById(It.IsAny<int>())).Returns(fakeQuiz);

        var result = await _quizService.SubmitAnswers(1, answerRequest);
        _mockAnswerCheckerService.Verify(mock => mock.CheckMultipleChoiceAnswer(It.IsAny<MultipleChoiceQuestion>(), It.IsAny<List<Option>>()), Times.Once);

        Assert.AreEqual(expectedResponse.Status, result.Status);
        _mockQuizRepository.Verify(repo => repo.GetQuizById(It.IsAny<int>()), Times.Once);
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

        _mockQuizRepository.Setup(repo => repo.GetQuizById(It.IsAny<int>())).Returns(fakeQuiz);

        var result = await _quizService.SubmitAnswers(1, answerRequest);
        _mockAnswerCheckerService.Verify(mock => mock.CheckOpenTextAnswer(It.IsAny<OpenTextQuestion>(), It.IsAny<string>(), false, true), Times.Once);

        Assert.AreEqual(expectedResponse.Status, result.Status);
        _mockQuizRepository.Verify(repo => repo.GetQuizById(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public void GetQuizzes_ReturnsCorrectQuizzes()
    {
        var quizzes = new List<Quiz>
        {
            new Quiz { Id = 1, Name = "Quiz 1"},
            new Quiz { Id = 2, Name = "Quiz 2"}
        };

        _mockQuizRepository.Setup(repo => repo.GetQuizzes()).Returns(quizzes);

        var result = _quizService.GetQuizzes();

        Assert.AreEqual(quizzes.Count, result.Count());
        Assert.IsTrue(result.Any(q => q.Name == "Quiz 1"));
        Assert.IsTrue(result.Any(q => q.Name == "Quiz 2"));
    }

    [Test]
    public void DeleteQuiz_DeletesCorrectQuiz()
    {
        int quizIDtoDelete = 1;
        var quizToDelete = new Quiz { Id = quizIDtoDelete, Name = "Quiz that will soon be *poof*" };

        _mockQuizRepository.Setup(repo => repo.GetQuizById(quizIDtoDelete)).Returns(quizToDelete);

        var result = _quizService.DeleteQuiz(quizIDtoDelete);

        Assert.IsTrue(result);
        _mockQuizRepository.Verify(repo => repo.GetQuizById(quizIDtoDelete), Times.Once);
        _mockQuizRepository.Verify(repo => repo.DeleteQuiz(quizIDtoDelete), Times.Once);
    }

    [Test]
    public void GetQuiz_ReturnsProperQuiz()
    {
        int quizId = 10;
        var quiz = new Quiz { Id = quizId, Name = "Cool name" };
        _mockQuizRepository.Setup(repo => repo.GetQuizById(quizId)).Returns(quiz);

        var result = _quizService.GetQuiz(quizId);

        Assert.AreEqual(quiz.Name, result.Name);
        Assert.AreEqual(quiz.Id, result.Id);
        _mockQuizRepository.Verify(repo => repo.GetQuizById(quizId), Times.Once);
    }

    [Test]
    public void UpdateQuiz_SuccessfulUpdate()
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

        _mockQuizRepository.Setup(repo => repo.GetQuizById(existingQuizId)).Returns(existingQuiz);
        _mockQuizRepository.Setup(repo => repo.Save()).Verifiable();
        _mockSingleChoiceQuestionDtoConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDto>())).Returns(new SingleChoiceQuestion());

        var result = _quizService.UpdateQuiz(existingQuizId, newQuizData);

        Assert.AreEqual("success", result.Status);
        Assert.AreEqual(existingQuizId, result.Id);
        _mockQuizRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [Test]
    public void UpdateQuiz_QuizNotFound()
    {
        var nonExistentQuizId = 100;
        var editRequest = new QuizManipulationRequestDto
        {
        };

        _mockQuizRepository.Setup(repo => repo.GetQuizById(nonExistentQuizId)).Returns((Quiz)null);
        _mockSingleChoiceQuestionDtoConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDto>())).Returns(new SingleChoiceQuestion());

        var result = _quizService.UpdateQuiz(nonExistentQuizId, editRequest);

        Assert.AreEqual("Quiz not found", result.Status);
    }

    [Test]
    public void UpdateQuiz_DataConversionException()
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

        _mockQuizRepository.Setup(repo => repo.GetQuizById(existingQuizId)).Returns(existingQuiz);
        _mockSingleChoiceQuestionDtoConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDto>())).Returns(new SingleChoiceQuestion());

        var result = _quizService.UpdateQuiz(existingQuizId, editRequest);

        Assert.AreEqual("Failed to create a question", result.Status);
    }
}