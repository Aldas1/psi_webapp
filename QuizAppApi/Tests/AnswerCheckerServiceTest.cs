using Moq;
using NUnit.Framework;
using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class AnswerCheckerTests
{
    private IAnswerCheckerService? _answerCheckerService;

    [Test]
    public void CheckAnswers()
    {
        // Arrange
        _answerCheckerService = new AnswerCheckerService();

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