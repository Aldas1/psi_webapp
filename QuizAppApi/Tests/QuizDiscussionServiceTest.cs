using NUnit.Framework;
using Moq;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Dtos;
using QuizAppApi.Exceptions;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class QuizDiscussionServiceTests
{
    private Mock<ICacheRepository> _cacheRepositoryMock;
    private Mock<IExplanationService> _explanationServiceMock;
    private IQuizDiscussionService _quizDiscussionService;

    [SetUp]
    public void SetUp()
    {
        _cacheRepositoryMock = new Mock<ICacheRepository>();
        _explanationServiceMock = new Mock<IExplanationService>();
        _cacheRepositoryMock.SetupGet(c => c.Lock).Returns(new object());
        _quizDiscussionService = new QuizDiscussionService(_cacheRepositoryMock.Object, _explanationServiceMock.Object);
    }

    [Test]
    public void SaveMessageAsync_ReturnsCommentDto()
    {
        var quizId = 1;
        var username = "user";
        var content = "Message";

        _cacheRepositoryMock.Setup(c => c.Add(It.IsAny<string>(), It.IsAny<Comment>()));

        var result = _quizDiscussionService.SaveMessageAsync(quizId, username, content, false).Result;

        _cacheRepositoryMock.Verify(c => c.Add("comments", It.Is<Comment>(comment =>
            comment.QuizId == quizId &&
            comment.Username == username &&
            comment.Content == "Message")), Times.Once);

        Assert.IsInstanceOf<CommentDto>(result);
        Assert.AreEqual(content, result.Content);
        Assert.AreEqual(username, result.Username);
    }

    [Test]
    public void SaveMessageAsyncExplanation_ReturnsCommentDto()
    {
        var quizId = 1;
        var username = "user";
        var content = "Message";

        _explanationServiceMock.Setup(e => e.GenerateCommentExplanationAsync(It.IsAny<string>()))
            .ReturnsAsync("Message");

        _cacheRepositoryMock.Setup(c => c.Add(It.IsAny<string>(), It.IsAny<Comment>()));

        var result = _quizDiscussionService.SaveMessageAsync(quizId, username, content, true).Result;

        _cacheRepositoryMock.Verify(c => c.Add("comments", It.Is<Comment>(comment =>
            comment.QuizId == quizId &&
            comment.Username == username &&
            comment.Content == "Message")), Times.Once);

        _explanationServiceMock.Verify(e => e.GenerateCommentExplanationAsync(content), Times.Once);

        Assert.IsInstanceOf<CommentDto>(result);
        Assert.AreEqual("Message", result.Content);
        Assert.AreEqual(username, result.Username);
    }

    [Test]
    public void GetRecentComments_ReturnsListOfCommentDto()
    {
        var quizId = 1;

        _cacheRepositoryMock.Setup(c => c.Retrieve<Comment>(It.IsAny<string>()))
            .Returns(new List<Comment> { new Comment() });

        var result = _quizDiscussionService.GetRecentComments(quizId);

        Assert.IsInstanceOf<IEnumerable<CommentDto>>(result);
    }

    [Test]
    public void GetRecentComments_TypeMismatchException()
    {
        var quizId = 1;

        _cacheRepositoryMock.Setup(c => c.Retrieve<Comment>(It.IsAny<string>()))
            .Throws(new TypeMismatchException(typeof(Comment), typeof(string), "Type mismatch"));

        _cacheRepositoryMock.Setup(c => c.Clear(It.IsAny<string>()));

        Assert.Throws<InternalException>(() => _quizDiscussionService.GetRecentComments(quizId));
        _cacheRepositoryMock.Verify(c => c.Clear(It.IsAny<string>()), Times.Exactly(3));
    }
}