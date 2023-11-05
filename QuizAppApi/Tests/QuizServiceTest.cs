using Moq;
using NUnit.Framework;
using QuizAppApi.DTOs;
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
    private Mock<IQuestionDTOConverterService<SingleChoiceQuestion>>? _mockSingleChoiceQuestionDTOConverterService;
    private Mock<IQuestionDTOConverterService<MultipleChoiceQuestion>>? _mockMultipleChoiceQuestionDTOConverterService;
    private Mock<IQuestionDTOConverterService<OpenTextQuestion>>? _mockOpenTextQuestionDTOConverterService;
    private Mock<IAnswerCheckerService>? _mockAnswerCheckerService;

    [SetUp]
    public void Setup()
    {
        _mockQuizRepository = new Mock<IQuizRepository>();
        _mockChatGptService = new Mock<IExplanationService>();
        _mockSingleChoiceQuestionDTOConverterService = new Mock<IQuestionDTOConverterService<SingleChoiceQuestion>>();
        _mockMultipleChoiceQuestionDTOConverterService = new Mock<IQuestionDTOConverterService<MultipleChoiceQuestion>>();
        _mockOpenTextQuestionDTOConverterService = new Mock<IQuestionDTOConverterService<OpenTextQuestion>>();
        _mockAnswerCheckerService = new Mock<IAnswerCheckerService>();
        _quizService = new QuizService(
            _mockQuizRepository.Object,
            _mockChatGptService.Object,
            _mockSingleChoiceQuestionDTOConverterService.Object,
            _mockMultipleChoiceQuestionDTOConverterService.Object,
            _mockOpenTextQuestionDTOConverterService.Object,
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
        var request = new QuizManipulationRequestDTO
        {
            Name = "Test Quiz",
            Questions = new List<QuizManipulationQuestionRequestDTO>
            {
                new QuizManipulationQuestionRequestDTO
                {
                    QuestionText = "What is the capital of France?",
                    QuestionType = "singleChoiceQuestion",
                    QuestionParameters = new QuestionParametersDTO
                    {
                        Options = new List<string> { "Paris", "London", "Berlin", "Madrid" },
                        CorrectOptionIndex = 0
                    }
                }
            }
        };
        var expectedResponse = new QuizManipulationResponseDTO { Status = "success", Id = 1 };

        _mockQuizRepository.Setup(repo => repo.AddQuiz(It.IsAny<Quiz>())).Returns(new Quiz { Id = 1 });
        _mockSingleChoiceQuestionDTOConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDTO>())).Returns(new SingleChoiceQuestion());

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
        var answerRequest = new List<AnswerSubmitRequestDTO>
        {
            new AnswerSubmitRequestDTO { QuestionId = 1, OptionName = "Paris" }
        };
        var expectedResponse = new AnswerSubmitResponseDTO { Status = "success" };

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
        var answerRequest = new List<AnswerSubmitRequestDTO>
        {
            new AnswerSubmitRequestDTO
            {
                QuestionId = 1,
                OptionNames = new List<string> { "Option1", "Option2" }
            }
        };

        var expectedResponse = new AnswerSubmitResponseDTO { Status = "success" };

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
        var answerRequest = new List<AnswerSubmitRequestDTO>
        {
            new AnswerSubmitRequestDTO
            {
                QuestionId = 1,
                AnswerText = "Your answer goes here"
            }
        };

        var expectedResponse = new AnswerSubmitResponseDTO { Status = "success" };

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
        int quizIdToDelete = 1;
        var quizToDelete = new Quiz { Id = quizIdToDelete, Name = "Quiz that will soon be *poof*" };

        _mockQuizRepository.Setup(repo => repo.GetQuizById(quizIdToDelete)).Returns(quizToDelete);

        var result = _quizService.DeleteQuiz(quizIdToDelete);

        Assert.IsTrue(result);
        _mockQuizRepository.Verify(repo => repo.GetQuizById(quizIdToDelete), Times.Once);
        _mockQuizRepository.Verify(repo => repo.DeleteQuiz(quizIdToDelete), Times.Once);
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

        var newQuizData = new QuizManipulationRequestDTO
        {
            Name = "New Name",
            Questions = new List<QuizManipulationQuestionRequestDTO>
            {
                new QuizManipulationQuestionRequestDTO
                {
                    QuestionText = "What is 2 + 2?",
                    QuestionType = "singleChoiceQuestion",
                    QuestionParameters = new QuestionParametersDTO
                    {
                        Options = new List<string> { "3", "4", "5" },
                        CorrectOptionIndex = 1
                    }
                },
            }
        };

        _mockQuizRepository.Setup(repo => repo.GetQuizById(existingQuizId)).Returns(existingQuiz);
        _mockQuizRepository.Setup(repo => repo.Save()).Verifiable();
        _mockSingleChoiceQuestionDTOConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDTO>())).Returns(new SingleChoiceQuestion());

        var result = _quizService.UpdateQuiz(existingQuizId, newQuizData);

        Assert.AreEqual("success", result.Status);
        Assert.AreEqual(existingQuizId, result.Id);
        _mockQuizRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [Test]
    public void UpdateQuiz_QuizNotFound()
    {
        var nonExistentQuizId = 100;
        var editRequest = new QuizManipulationRequestDTO
        {
        };

        _mockQuizRepository.Setup(repo => repo.GetQuizById(nonExistentQuizId)).Returns((Quiz)null);
        _mockSingleChoiceQuestionDTOConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDTO>())).Returns(new SingleChoiceQuestion());

        var result = _quizService.UpdateQuiz(nonExistentQuizId, editRequest);

        Assert.AreEqual("Quiz not found", result.Status);
    }

    [Test]
    public void UpdateQuiz_DataConversionException()
    {
        var existingQuizId = 1;
        var existingQuiz = new Quiz { Id = existingQuizId, Name = "Old Name" };

        var editRequest = new QuizManipulationRequestDTO
        {
            Name = "New Name",
            Questions = new List<QuizManipulationQuestionRequestDTO>
            {
                new QuizManipulationQuestionRequestDTO
                {
                    QuestionText = null

                },

            }
        };

        _mockQuizRepository.Setup(repo => repo.GetQuizById(existingQuizId)).Returns(existingQuiz);
        _mockSingleChoiceQuestionDTOConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDTO>())).Returns(new SingleChoiceQuestion());

        var result = _quizService.UpdateQuiz(existingQuizId, editRequest);

        Assert.AreEqual("Failed to create a question", result.Status);
    }
}