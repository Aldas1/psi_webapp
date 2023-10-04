using NUnit.Framework;
using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Services;
using QuizAppApi.Repositories;

namespace Tests
{
    [TestFixture]
    public class QuizServiceTests
    {
        private IQuizService? _quizService;
        private IQuizRepository? _quizRepository;

        [SetUp]
        public void Setup()
        {
            _quizRepository = new InMemoryQuizRepository();
            _quizService = new QuizService(_quizRepository);
        }
        
        [TearDown]
        public void TearDown()
        {
            // Reset the repository after each test
            _quizRepository = null;
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
            var expectedResponse = new QuizCreationResponseDTO { Status = "success" };

            // Act
            var result = _quizService.CreateQuiz(request);
            
            // Assert
            Assert.AreEqual(expectedResponse.Status, result.Status);
            // Assert.AreEqual(1, _quizRepository.GetQuizzes());
            Assert.AreEqual(request.Name, _quizRepository.GetQuizById(0).Name);
            Assert.AreEqual(request.Questions.Count, _quizRepository.GetQuizById(0).Questions.Count);
            Assert.AreEqual(request.Questions.First().QuestionText, _quizRepository.GetQuizById(0).Questions.First().Text);
            // Assert.AreEqual(request.Questions.First().QuestionParameters.Options.Count, _quizRepository.GetQuizById(0).Questions.First().Options.Count);
            // Assert.AreEqual(request.Questions.First().QuestionParameters.CorrectOptionIndex, _quizRepository.GetQuizById(0).Questions.First().CorrectOptionIndex);
        }

        [Test]
        public void CreateQuiz_ReturnsErrorForInvalidRequest()
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
                            CorrectOptionIndex = 4 // Invalid index
                        }
                    }
                }
            };
            var expectedResponse = new QuizCreationResponseDTO { Status = "Correct option index out of Options list bounds" };

            // Act
            var result = _quizService.CreateQuiz(request);

            // Assert
            Assert.AreEqual(expectedResponse.Status, result.Status);
            Assert.AreEqual(0, _quizRepository.GetQuizzes().Count());
        }

        [Test]
        public void GetQuiz_ReturnsCorrectQuiz()
        {
            // Arrange
            var quizRequest = new QuizCreationRequestDTO
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
            var createdQuiz = _quizService.CreateQuiz(quizRequest);

            // Act
            var result = _quizRepository.GetQuizById(createdQuiz.Id ?? 0);

            // Assert
            Assert.AreEqual(quizRequest.Name, result.Name);
            Assert.AreEqual(quizRequest.Questions.Count, result.Questions.Count);
            Assert.AreEqual(quizRequest.Questions.First().QuestionText, result.Questions.First().Text);
            // Assert.AreEqual(quizRequest.Questions.First().QuestionParameters.Options.Count, result.Questions.First().QuestionParameters.Options.Count);
            // Assert.AreEqual(quizRequest.Questions.First().QuestionParameters.CorrectOptionIndex, result.Questions.First().QuestionParameters.CorrectOptionIndex);
        }

        [Test]
        public void GetQuiz_ReturnsNullForNonexistentQuiz()
        {
            // Arrange

            // Act
            var result = _quizRepository.GetQuizById(0);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void SubmitAnswers_ReturnsCorrectResponse()
        {
            // Arrange
            var quizRequest = new QuizCreationRequestDTO
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
            var createdQuiz = _quizService.CreateQuiz(quizRequest);

            var answerRequest = new List<AnswerSubmitRequestDTO>
            {
                new AnswerSubmitRequestDTO { QuestionId = 1, OptionName = "Paris" }
            };
            var expectedResponse = new AnswerSubmitResponseDTO { CorrectlyAnswered = 1 };

            // Act
            var result = _quizService.SubmitAnswers(createdQuiz.Id ?? 0, answerRequest);

            // Assert
            Assert.AreEqual(expectedResponse.CorrectlyAnswered, result.CorrectlyAnswered);
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

            // Act
            var result = _quizService.SubmitAnswers(1, answerRequest);

            // Assert
            Assert.AreEqual(expectedResponse.Status, result.Status);
        }
    }
}
