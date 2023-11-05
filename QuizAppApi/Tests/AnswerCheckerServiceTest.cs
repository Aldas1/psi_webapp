using NUnit.Framework;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;
using System.Linq;

namespace Tests;

[TestFixture]
public class AnswerCheckerServiceTests
{
    private IAnswerCheckerService? _answerCheckerService;

    [SetUp]
    public void Setup()
    {
        _answerCheckerService = new AnswerCheckerService();
    }

    [TearDown]
    public void TearDown()
    {
        _answerCheckerService = null;
    }

    [Test]
    public void CheckSingleChoiceAnswer_ValidatingAnswer()
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

        Assert.IsTrue(_answerCheckerService.CheckSingleChoiceAnswer(singleChoiceQuestion, singleChoiceQuestion.Options.ToList().Find(option => option.Correct == true).Name));
        Assert.IsFalse(_answerCheckerService.CheckSingleChoiceAnswer(singleChoiceQuestion, singleChoiceQuestion.Options.ToList().Find(option => option.Correct == false).Name));
    }

    [Test]
    public void CheckMultipleChoiceAnswer_ValidatingCorrectAnswer()
    {
        var option1 = new Option { Name = "Paris", Correct = true };
        var option2 = new Option { Name = "Berlin", Correct = true };
        var option3 = new Option { Name = "London", Correct = false };

        var multipleChoiceQuestion = new MultipleChoiceQuestion
        {
            Options = new List<Option> { option1, option2, option3 }
        };

        var correctMultipleChoiceAnswer = new List<Option>
        {
            new Option { Name = "London", Correct = false },
            new Option { Name = "Berlin", Correct = true },
            new Option { Name = "Paris", Correct = true }
        };

        Assert.IsTrue(_answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion, correctMultipleChoiceAnswer));
    }

    [Test]
    public void CheckMultipleChoiceAnswer_ValidatingIncorrectAnswer()
    {
        var option1 = new Option { Name = "Paris", Correct = true };
        var option2 = new Option { Name = "Berlin", Correct = false };
        var option3 = new Option { Name = "London", Correct = true };

        var multipleChoiceQuestion = new MultipleChoiceQuestion
        {
            Options = new List<Option> { option1, option2, option3 }
        };

        var incorrectMultipleChoiceAnswer = new List<Option>
        {
            new Option { Name = "London", Correct = false },
            new Option { Name = "Berlin", Correct = false },
            new Option { Name = "Paris", Correct = false }
        };

        Assert.IsFalse(_answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion, incorrectMultipleChoiceAnswer));
    }

    [Test]
    public void CheckOpenTextAnswer_ValidatingAnswer()
    {
        var openTextQuestion = new OpenTextQuestion
        {
            CorrectAnswer = "Paris"
        };

        Assert.IsTrue(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, openTextQuestion.CorrectAnswer));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "London"));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, null));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, ""));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "Par"));
        Assert.IsTrue(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "paris", useLowercaseComparison: true));
        Assert.IsTrue(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "   Paris   ", trimWhitespace: true));
    }
}