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
        var option_name1 = "Paris";
        var option_name2 = "Berlin";
        var option_name3 = "London";

        var singleChoiceQuestion = new SingleChoiceQuestion
        {
            Options = new List<Option>
            {
                new Option { Name = option_name1, Correct = true },
                new Option { Name = option_name3, Correct = false },
                new Option { Name = option_name2, Correct = false }
            }
        };

        Assert.IsTrue(_answerCheckerService.CheckSingleChoiceAnswer(singleChoiceQuestion, option_name1));
        Assert.IsFalse(_answerCheckerService.CheckSingleChoiceAnswer(singleChoiceQuestion, option_name3));
    }

    [Test]
    public void CheckMultipleChoiceAnswer_ValidatingCorrectAnswer()
    {
        var option_name1 = "Paris";
        var option_name2 = "Berlin";
        var option_name3 = "London";

        var option1 = new Option { Name = option_name1, Correct = true };
        var option2 = new Option { Name = option_name2, Correct = false };
        var option3 = new Option { Name = option_name3, Correct = false };

        var multipleChoiceQuestion = new MultipleChoiceQuestion
        {
            Options = new List<Option> { option1, option2, option3 }
        };

        var correctMultipleChoiceAnswer = new List<Option>
        {
            new Option { Name = option_name3, Correct = false },
            new Option { Name = option_name2, Correct = false },
            new Option { Name = option_name1, Correct = true }
        };

        Assert.IsTrue(_answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion, correctMultipleChoiceAnswer));
    }

    [Test]
    public void CheckMultipleChoiceAnswer_ValidatingIncorrectAnswer()
    {
        var option_name1 = "Paris";
        var option_name2 = "Berlin";
        var option_name3 = "London";

        var option1 = new Option { Name = option_name1, Correct = true };
        var option2 = new Option { Name = option_name3, Correct = false };
        var option3 = new Option { Name = option_name2, Correct = false };

        var multipleChoiceQuestion = new MultipleChoiceQuestion
        {
            Options = new List<Option> { option1, option2, option3 }
        };

        var incorrectMultipleChoiceAnswer = new List<Option>
        {
            new Option { Name = option_name3, Correct = false },
            new Option { Name = option_name2, Correct = false }
        };

        Assert.IsFalse(_answerCheckerService.CheckMultipleChoiceAnswer(multipleChoiceQuestion, incorrectMultipleChoiceAnswer));
    }

    [Test]
    public void CheckOpenTextAnswer_ValidatingAnswer()
    {
        var option_name1 = "Paris";
        var option_name3 = "London";

        var openTextQuestion = new OpenTextQuestion
        {
            CorrectAnswer = option_name1
        };

        Assert.IsTrue(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, option_name1));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, option_name3));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, null));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, ""));
        Assert.IsFalse(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "Par"));
        Assert.IsTrue(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "paris", useLowercaseComparison: true));
        Assert.IsTrue(_answerCheckerService.CheckOpenTextAnswer(openTextQuestion, "   Paris   ", trimWhitespace: true));
    }
}