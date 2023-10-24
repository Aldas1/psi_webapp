
using Moq;
using NUnit.Framework;
using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests
{
    [TestFixture]
    public class QuizServiceTests
    {
        private IQuizService? _quizService;
        private Mock<IQuizRepository>? _mockQuizRepository;
        private Mock<IQuestionDTOConverterService<SingleChoiceQuestion>>? _mockSingleChoiceQuestionDTOConverterService;
        private Mock<IQuestionDTOConverterService<MultipleChoiceQuestion>>? _mockMultipleChoiceQuestionDTOConverterService;
        private Mock<IQuestionDTOConverterService<OpenTextQuestion>>? _mockOpenTextQuestionDTOConverterService;
        private Mock<IAnswerCheckerService>? _mockAnswerCheckerService;
        private IAnswerCheckerService? _answerCheckerService;


        [SetUp]
        public void Setup()
        {
            _mockQuizRepository = new Mock<IQuizRepository>();
            _mockSingleChoiceQuestionDTOConverterService = new Mock<IQuestionDTOConverterService<SingleChoiceQuestion>>();
            _mockMultipleChoiceQuestionDTOConverterService = new Mock<IQuestionDTOConverterService<MultipleChoiceQuestion>>();
            _mockOpenTextQuestionDTOConverterService = new Mock<IQuestionDTOConverterService<OpenTextQuestion>>();
            _mockAnswerCheckerService = new Mock<IAnswerCheckerService>();

            _quizService = new QuizService(
                _mockQuizRepository.Object,
                _mockSingleChoiceQuestionDTOConverterService.Object,
                _mockMultipleChoiceQuestionDTOConverterService.Object,
                _mockOpenTextQuestionDTOConverterService.Object,
                _mockAnswerCheckerService.Object);

            _answerCheckerService = new AnswerCheckerService();
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
            var request = new QuizCreationRequestDTO
            {
                Name = "Test Quiz",
                Questions = new List<QuizCreationQuestionRequestDTO>
                {
                    new QuizCreationQuestionRequestDTO
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
            var expectedResponse = new QuizCreationResponseDTO { Status = "success", Id = 1 };

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
        public void SubmitAnswers_ReturnsErrorForNonexistentQuiz()
        {
            // Arrange
            var answerRequest = new List<AnswerSubmitRequestDTO>
            {
                new AnswerSubmitRequestDTO { QuestionId = 1, OptionName = "Paris" }
            };
            var expectedResponse = new AnswerSubmitResponseDTO { Status = "failed" };

            // Mock repository setup
            _mockQuizRepository.Setup(repo => repo.GetQuizById(It.IsAny<int>())).Returns((Quiz)null);

            // Act
            var result = _quizService.SubmitAnswers(1, answerRequest);

            // Assert
            Assert.AreEqual(expectedResponse.Status, result.Status);
            _mockQuizRepository.Verify(repo => repo.GetQuizById(It.IsAny<int>()), Times.Once);
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

        [Test]
        public void CheckAnswers()
        {
            // Arrange
            var singleChoiceQuestion = new SingleChoiceQuestion
            {
                Options = new List<Option>
                {
                    new Option { Name = "Paris", Correct = true },
                    new Option { Name = "London", Correct = false },
                    new Option { Name = "Berlin", Correct = false }
                }
            };

            var option1 = new Option { Name = "Paris", Correct = true };
            var option2 = new Option { Name = "London", Correct = false };
            var option3 = new Option { Name = "Berlin", Correct = false };

            var multipleChoiceQuestion = new MultipleChoiceQuestion
            {
                Options = new List<Option> { option1, option2, option3 }
            };

            var openTextQuestion = new OpenTextQuestion
            {
                CorrectAnswer = "Paris"
            };

            var answerCheckerService = _answerCheckerService;

            // Act and Assert

            Assert.IsTrue(answerCheckerService.CheckSingleChoiceAnswer(singleChoiceQuestion, "Paris"));
            Assert.IsFalse(answerCheckerService.CheckSingleChoiceAnswer(singleChoiceQuestion, "London"));

            var correctMultipleChoiceAnswer = new List<Option>
            {
                new Option { Name = "London", Correct = false },
                new Option { Name = "Berlin", Correct = false },
                new Option { Name = "Paris", Correct = true }
            };

            Assert.IsTrue(answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion, correctMultipleChoiceAnswer));

            var incorrectMultipleChoiceAnswer = new List<Option>
            {
                new Option { Name = "London", Correct = false },
                new Option { Name = "Berlin", Correct = false }
            };
            Assert.IsFalse(answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion, incorrectMultipleChoiceAnswer));

            Assert.IsTrue(answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "Paris"));
            Assert.IsFalse(answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "London"));
        }


    }
}
