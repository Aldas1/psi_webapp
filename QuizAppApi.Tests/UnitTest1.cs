using NUnit.Framework;
using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Services;
using QuizAppApi.Models;
using System.Collections.Generic;
using System.Linq;
using QuizAppApi.Repositories;

namespace QuizAppApi.Tests
{
    [TestFixture]
    public class QuizServiceTests
    {
        private IQuizService _quizService;
        private IQuizRepository _quizRepository;

        [SetUp]
        public void Setup()
        {
            _quizRepository = new InMemoryQuizRepository();
            _quizService = new QuizService(_quizRepository);
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
            var expectedResponse = new QuizCreationResponseDTO { Status = "Success" };

            // Act
            var result = _quizService.CreateQuiz(request);

            // Assert
            Assert.AreEqual(expectedResponse.Status, result.Status);
            Assert.AreEqual(1, _quizRepository.GetQuizzes().Count());
            Assert.AreEqual(request.Name, _quizRepository.GetQuizById(0).Name);
            Assert.AreEqual(request.Questions.Count, _quizRepository.GetQuizById(0).Questions.Count);
            Assert.AreEqual(request.Questions.First().QuestionText, _quizRepository.GetQuizById(0).Questions.First().Text);
            // Assert.AreEqual(request.Questions.First().QuestionParameters.Options.Count, _quizRepository.GetQuizById(0).Questions.First().Options.Count);
            // Assert.AreEqual(request.Questions.First().QuestionParameters.CorrectOptionIndex, _quizRepository.GetQuizById(0).Questions.First().CorrectOptionIndex);
        }

        // [Test]
        // public void CreateQuiz_ReturnsErrorForInvalidRequest()
        // {
        //     // Arrange
        //     var request = new QuizCreationRequestDTO
        //     {
        //         Name = "Test Quiz",
        //         Questions = new List<QuizCreationQuestionRequestDTO>
        //         {
        //             new QuizCreationQuestionRequestDTO
        //             {
        //                 QuestionText = "What is the capital of France?",
        //                 QuestionType = "singleChoiceQuestion",
        //                 QuestionParameters = new QuestionParametersDTO
        //                 {
        //                     Options = new List<string> { "Paris", "London", "Berlin", "Madrid" },
        //                     CorrectOptionIndex = 4 // Invalid index
        //                 }
        //             }
        //         }
        //     };
        //     var expectedResponse = new QuizCreationResponseDTO { Status = "Correct option index out of Options list bounds" };

        //     // Act
        //     var result = _quizService.CreateQuiz(request);

        //     // Assert
        //     Assert.AreEqual(expectedResponse.Status, result.Status);
        //     Assert.AreEqual(0, _quizRepository.GetQuizzes().Count());
        // }

        // [Test]
        // public void GetQuiz_ReturnsCorrectQuiz()
        // {
        //     // Arrange
        //     var quiz = new Quiz
        //     {
        //         Name = "Test Quiz",
        //         Questions = new List<Question>
        //         {
        //             new SingleChoiceQuestion
        //             {
        //                 Text = "What is the capital of France?",
        //                 Options = new List<Option>
        //                 {
        //                     new Option { Name = "Paris" },
        //                     new Option { Name = "London" },
        //                     new Option { Name = "Berlin" },
        //                     new Option { Name = "Madrid" }
        //                 },
        //                 CorrectOptionIndex = 0
        //             }
        //         }
        //     };
        //     _quizRepository.AddQuiz(quiz);
        //
        //     // Act
        //     var result = _quizService.GetQuiz(quiz.Id);
        //
        //     // Assert
        //     Assert.AreEqual(quiz.Name, result.Name);
        //     Assert.AreEqual(quiz.Questions.Count, result.Questions.Count);
        //     Assert.AreEqual(quiz.Questions[0].Text, result.Questions[0].QuestionText);
        //     Assert.AreEqual(quiz.Questions[0].Options.Count, result.Questions[0].QuestionParameters.Options.Count);
        //     Assert.AreEqual(quiz.Questions[0].CorrectOption.Name, result.Questions[0].QuestionParameters.Options[quiz.Questions[0].CorrectOptionIndex]);
        // }
        //
        // [Test]
        // public void GetQuiz_ReturnsNullForNonexistentQuiz()
        // {
        //     // Arrange
        //
        //     // Act
        //     var result = _quizService.GetQuiz(1);
        //
        //     // Assert
        //     Assert.IsNull(result);
        // }
        //
        // [Test]
        // public void SubmitAnswers_ReturnsCorrectResponse()
        // {
        //     // Arrange
        //     var quiz = new Quiz
        //     {
        //         Name = "Test Quiz",
        //         Questions = new List<Question>
        //         {
        //             new SingleChoiceQuestion
        //             {
        //                 Text = "What is the capital of France?",
        //                 Options = new List<Option>
        //                 {
        //                     new Option { Name = "Paris" },
        //                     new Option { Name = "London" },
        //                     new Option { Name = "Berlin" },
        //                     new Option { Name = "Madrid" }
        //                 },
        //                 CorrectOptionIndex = 0
        //             }
        //         }
        //     };
        //     _quizRepository.AddQuiz(quiz);
        //     var request = new List<AnswerSubmitRequestDTO>
        //     {
        //         new AnswerSubmitRequestDTO { QuestionId = 1, OptionName = "Paris" }
        //     };
        //     var expectedResponse = new AnswerSubmitResponseDTO { CorrectlyAnswered = 1 };
        //
        //     // Act
        //     var result = _quizService.SubmitAnswers(quiz.Id, request);
        //
        //     // Assert
        //     Assert.AreEqual(expectedResponse.CorrectlyAnswered, result.CorrectlyAnswered);
        // }
        //
        // [Test]
        // public void SubmitAnswers_ReturnsErrorForNonexistentQuiz()
        // {
        //     // Arrange
        //     var request = new List<AnswerSubmitRequestDTO>
        //     {
        //         new AnswerSubmitRequestDTO { QuestionId = 1, OptionName = "Paris" }
        //     };
        //     var expectedResponse = new AnswerSubmitResponseDTO { ErrorMessage = "Quiz not found" };
        //
        //     // Act
        //     var result = _quizService.SubmitAnswers(1, request);
        //
        //     // Assert
        //     Assert.AreEqual(expectedResponse.ErrorMessage, result.ErrorMessage);
        // }
    }
}