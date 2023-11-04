using NUnit.Framework;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class AnswerCheckerServiceTests
{
    private IAnswerCheckerService? _answerCheckerService;

    string option1 = "Paris";
    string option2 = "Berlin";
    string option3 = "London";

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
                new Option { Name = option1, Correct = true },
                new Option { Name = option3, Correct = false },
                new Option { Name = option2, Correct = false }
            }
        };

        Assert.IsTrue(_answerCheckerService.CheckSingleChoiceAnswer(singleChoiceQuestion, option1));
        Assert.IsFalse(_answerCheckerService.CheckSingleChoiceAnswer(singleChoiceQuestion, option3));
    }

    [Test]
    public void CheckMultipleChoiceAnswer_ValidatingCorrectAnswer()
    {
        var option1 = new Option { Name = this.option1, Correct = true };
        var option2 = new Option { Name = this.option3, Correct = false };
        var option3 = new Option { Name = this.option2, Correct = false };

        var multipleChoiceQuestion = new MultipleChoiceQuestion
        {
            Options = new List<Option> { option1, option2, option3 }
        };

        var correctMultipleChoiceAnswer = new List<Option>
        {
            new Option { Name = this.option3, Correct = false },
            new Option { Name = this.option2, Correct = false },
            new Option { Name = this.option1, Correct = true }
        };

        Assert.IsTrue(_answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion, correctMultipleChoiceAnswer));

        var incorrectMultipleChoiceAnswer = new List<Option>
        {
            new Option { Name = this.option3, Correct = false },
            new Option { Name = this.option2, Correct = false }
        };

        Assert.IsFalse(_answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion, incorrectMultipleChoiceAnswer));
    }

    [Test]
    public void CheckMultipleChoiceAnswer_ValidatingIncorrectAnswer()
    {
        var option1 = new Option { Name = this.option1, Correct = true };
        var option2 = new Option { Name = this.option3, Correct = false };
        var option3 = new Option { Name = this.option2, Correct = false };

        var multipleChoiceQuestion = new MultipleChoiceQuestion
        {
            Options = new List<Option> { option1, option2, option3 }
        };

        var incorrectMultipleChoiceAnswer = new List<Option>
        {
            new Option { Name = this.option3, Correct = false },
            new Option { Name = this.option2, Correct = false }
        };

        Assert.IsFalse(_answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion, incorrectMultipleChoiceAnswer));
    }

    [Test]
    public void CheckOpenTextAnswer_ValidatingAnswer()
    {
        var openTextQuestion = new OpenTextQuestion
        {
            CorrectAnswer = option1
        };

        Assert.IsTrue(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "Paris"));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "London"));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, null));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, ""));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "Par"));
        Assert.IsTrue(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "paris", useLowercaseComparison: true));
        Assert.IsTrue(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "   Paris   ", trimWhitespace: true));
    }
}