using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using QuizAppApi.Controllers;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;

namespace Tests;

[TestFixture]
public class QuizControllerTests
{
    private Mock<IQuizService> _quizServiceMock;
    private QuizController _controller;

    [SetUp]
    public void Setup()
    {
        _quizServiceMock = new Mock<IQuizService>();
        _controller = new QuizController(_quizServiceMock.Object);
    }

    [Test]
    public async Task GetQuizzes_ReturnsOkResultWithQuizzes()
    {
        _quizServiceMock.Setup(x => x.GetQuizzesAsync())
            .ReturnsAsync(new List<QuizResponseDto> { new QuizResponseDto() });

        var result = await _controller.GetQuizzes();

        Assert.IsInstanceOf<ActionResult<IEnumerable<QuizResponseDto>>>(result);
        Assert.IsInstanceOf<OkObjectResult>(result.Result);

        var quizzes = (result.Result as OkObjectResult)?.Value as IEnumerable<QuizResponseDto>;
        Assert.IsNotNull(quizzes);
        Assert.AreEqual(1, quizzes.Count());
    }

    [Test]
    public async Task GetQuiz_WithValidId_ReturnsOkResultWithQuiz()
    {
        _quizServiceMock.Setup(x => x.GetQuizAsync(It.IsAny<int>()))
            .ReturnsAsync(new QuizResponseDto());

        var result = await _controller.GetQuiz(1);

        Assert.IsInstanceOf<ActionResult<QuizResponseDto>>(result);
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
    }

    [Test]
    public async Task DeleteQuiz_WhenUserCanEdit_ReturnsOkResult()
    {
        _quizServiceMock.Setup(x => x.CanUserEditQuizAsync(It.IsAny<User>(), It.IsAny<int>()))
            .ReturnsAsync(true);
        _quizServiceMock.Setup(x => x.DeleteQuizAsync(It.IsAny<int>()))
            .ReturnsAsync(true);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                Items = { ["User"] = new User("sampleUsername", "sampleRole") }
            }
        };

        var result = await _controller.DeleteQuiz(1);

        Assert.IsInstanceOf<ActionResult>(result);
        Assert.IsInstanceOf<OkResult>(result);
    }
}