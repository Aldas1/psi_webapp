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
        _quizService = new QuizService(
            _mockQuizRepository.Object,
            _mockSingleChoiceQuestionDTOConverterService.Object,
            _mockMultipleChoiceQuestionDTOConverterService.Object,
            _mockOpenTextQuestionDTOConverterService.Object);
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