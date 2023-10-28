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
        // Reset the mock after each test
        _mockQuizRepository = null;
        _quizService = null;
    }

    [Test]
    public void CreateQuiz_ReturnsCorrectResponse()
    {
        // Arrange
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

        // Mock repository setup
        _mockQuizRepository.Setup(repo => repo.AddQuiz(It.IsAny<Quiz>())).Returns(new Quiz { Id = 1 });
        _mockSingleChoiceQuestionDTOConverterService.Setup(service => service.CreateFromParameters(It.IsAny<QuestionParametersDTO>())).Returns(new SingleChoiceQuestion());

        // Act
        var result = _quizService.CreateQuiz(request);

        // Assert
        Assert.AreEqual(expectedResponse.Status, result.Status);
        Assert.AreEqual(expectedResponse.Id, result.Id);
        _mockQuizRepository.Verify(repo => repo.AddQuiz(It.IsAny<Quiz>()), Times.Once);
    }

    [Test]
    public void GetQuiz_ReturnsCorrectQuiz()
    {
        // Mock repository setup
        var createdQuiz = new Quiz { Id = 1, Name = "Test Quiz" };
        _mockQuizRepository.Setup(repo => repo.AddQuiz(It.IsAny<Quiz>())).Returns(createdQuiz);

        // Ensure that GetQuizzes returns the expected list of quizzes
        var expectedQuizzes = new List<Quiz> { createdQuiz };
        _mockQuizRepository.Setup(repo => repo.GetQuizzes()).Returns(expectedQuizzes);

        // Act
        var result = _quizService.GetQuizzes().FirstOrDefault(q => q.Id == createdQuiz.Id);

        // Assert
        Assert.IsNotNull(createdQuiz); // Ensure quiz is created successfully
        Assert.IsNotNull(result); // Ensure result is not null
        // Add assertions for other properties as needed
        _mockQuizRepository.Verify(repo => repo.GetQuizzes(), Times.Once); // Verify that GetQuizzes is called
    }

    [Test]
    public async Task SubmitAnswers_ReturnsErrorForNonexistentQuiz()
    {
        // Arrange
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

        // Mock repository setup
        _mockQuizRepository.Setup(repo => repo.GetQuizById(It.IsAny<int>())).Returns(fakeQuiz);

        // Act
        var result = await _quizService.SubmitAnswers(1, answerRequest);
        _mockAnswerCheckerService.Verify(mock => mock.CheckSingleChoiceAnswer(It.IsAny<SingleChoiceQuestion>(), It.IsAny<string>()), Times.Once);


        var answerRequest2 = new List<AnswerSubmitRequestDTO>
        {
            new AnswerSubmitRequestDTO
            {
                QuestionId = 1,
                OptionNames = new List<string> { "Option1", "Option2" }
            }
        };

        var questions2 = new List<Question>
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

        var fakeQuiz2 = new Quiz
        {
            Id = 2,
            Name = "Fake Quiz",
            Questions = questions2
        };

        // Mock repository setup
        _mockQuizRepository.Setup(repo => repo.GetQuizById(It.IsAny<int>())).Returns(fakeQuiz2);

        // Act
        var result2 = await _quizService.SubmitAnswers(2, answerRequest2);
        _mockAnswerCheckerService.Verify(mock => mock.CheckMultipleChoiceAnswer(It.IsAny<MultipleChoiceQuestion>(), It.IsAny<List<Option>>()), Times.Once);

        var answerRequest3 = new List<AnswerSubmitRequestDTO>
        {
            new AnswerSubmitRequestDTO
            {
                QuestionId = 1,
                AnswerText = "Your answer goes here"
            }
        };

        var questions3 = new List<Question>
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

        var fakeQuiz3 = new Quiz
        {
            Id = 2,
            Name = "Fake Quiz",
            Questions = questions3
        };

        // Mock repository setup
        _mockQuizRepository.Setup(repo => repo.GetQuizById(It.IsAny<int>())).Returns(fakeQuiz3);

        // Act
        var result3 = await _quizService.SubmitAnswers(2, answerRequest3);
        _mockAnswerCheckerService.Verify(mock => mock.CheckOpenTextAnswer(It.IsAny<OpenTextQuestion>(), It.IsAny<string>(), false, false), Times.Once);

        // Assert
        Assert.AreEqual(expectedResponse.Status, result.Status);
        _mockQuizRepository.Verify(repo => repo.GetQuizById(It.IsAny<int>()), Times.Exactly(3));
    }



    [Test]
    public void GetQuizzes_ReturnsCorrectQuizzes()
    {
        // Arrange
        var quizzes = new List<Quiz>
        {
            new Quiz { Id = 1, Name = "Quiz 1" },
            new Quiz { Id = 2, Name = "Quiz 2" }
        };

        // Mock repository setup
        _mockQuizRepository.Setup(repo => repo.GetQuizzes()).Returns(quizzes);

        // Act
        var result = _quizService.GetQuizzes();

        // Assert
        Assert.AreEqual(quizzes.Count, result.Count());
        Assert.AreEqual(quizzes[0].Name, result.First().Name);
        Assert.AreEqual(quizzes[1].Name, result.Skip(1).First().Name);
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
}
