using NUnit.Framework;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class AnswerCheckerServiceTests
{
    private IAnswerCheckerService? _answerCheckerService;

    [SetUp]
    public void Setup()
    {
        // Setup checker
        _answerCheckerService = new AnswerCheckerService();
    }

    [TearDown]
    public void TearDown()
    {
        // Reset the checker after each test
        _answerCheckerService = null;
    }

    [Test]
    public void SingleChoiceAnswerChecker_Test()
    {
        
        var singleChoiceQuestion = new SingleChoiceQuestion
        {
            Options = new List<Option>
            {
                new Option { Name = "Paris", Correct = true },
                new Option { Name = "London", Correct = false },
                new Option { Name = "Berlin", Correct = false }
            }
        };

        // Act and Assert
        Assert.IsTrue(_answerCheckerService.CheckSingleChoiceAnswer(singleChoiceQuestion, "Paris"));
        Assert.IsFalse(_answerCheckerService.CheckSingleChoiceAnswer(singleChoiceQuestion, "London"));
    }

    [Test]
    public void MultipleChoiceAnswerChecker_Test()
    {
        // Arrange
        var option1 = new Option { Name = "Paris", Correct = true };
        var option2 = new Option { Name = "London", Correct = false };
        var option3 = new Option { Name = "Berlin", Correct = false };

        var multipleChoiceQuestion = new MultipleChoiceQuestion
        {
            Options = new List<Option> { option1, option2, option3 }
        };

        var correctMultipleChoiceAnswer = new List<Option>
        {
            new Option { Name = "London", Correct = false },
            new Option { Name = "Berlin", Correct = false },
            new Option { Name = "Paris", Correct = true }
        };

        Assert.IsTrue(_answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion, correctMultipleChoiceAnswer));

        var incorrectMultipleChoiceAnswer = new List<Option>
        {
            new Option { Name = "London", Correct = false },
            new Option { Name = "Berlin", Correct = false }
        };
        Assert.IsFalse(_answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion, incorrectMultipleChoiceAnswer));
    }

    [Test]
    public void OpenTextAnswerChecker_Test()
    {
        // Arrange
        var openTextQuestion = new OpenTextQuestion
        {
            CorrectAnswer = "Paris"
        };

        // Act and Assert
        Assert.IsTrue(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "Paris"));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "London"));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, null));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, ""));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "Par"));

        // Check useLowerCaseComparison and trimWhitespace optional parameters
        Assert.IsTrue(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "paris", useLowercaseComparison: true));
        Assert.IsTrue(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "   Paris   ", trimWhitespace: true));
    }
}