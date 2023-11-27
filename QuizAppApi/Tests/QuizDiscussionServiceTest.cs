using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [SetUp]
    public void SetUp()
    {
        _cacheRepositoryMock = new Mock<ICacheRepository>();
        _explanationServiceMock = new Mock<IExplanationService>();
        _cacheRepositoryMock.SetupGet(c => c.Lock).Returns(new object());
    }

    [Test]
    public void SaveMessageAsync_ReturnsCommentDto()
    {
        var quizDiscussionService = new QuizDiscussionService(_cacheRepositoryMock.Object, _explanationServiceMock.Object);
        var quizId = 1;
        var username = "user";
        var content = "Message";

        _explanationServiceMock.Setup(e => e.GenerateCommentExplanationAsync(It.IsAny<string>()))
            .ReturnsAsync("Explanation");

        _cacheRepositoryMock.Setup(c => c.Add(It.IsAny<string>(), It.IsAny<Comment>()));

        var result = quizDiscussionService.SaveMessageAsync(quizId, username, content, false).Result;

        Assert.IsInstanceOf<CommentDto>(result);
        Assert.AreEqual(content, result.Content);
        Assert.AreEqual(username, result.Username);
    }

    [Test]
    public void SaveMessageAsyncExplanation_ReturnsCommentDto()
    {
        var quizDiscussionService = new QuizDiscussionService(_cacheRepositoryMock.Object, _explanationServiceMock.Object);
        var quizId = 1;
        var username = "user";
        var content = "Message";

        _explanationServiceMock.Setup(e => e.GenerateCommentExplanationAsync(It.IsAny<string>()))
            .ReturnsAsync("Explanation");

        _cacheRepositoryMock.Setup(c => c.Add(It.IsAny<string>(), It.IsAny<Comment>()));

        var result = quizDiscussionService.SaveMessageAsync(quizId, username, content, true).Result;

        Assert.IsInstanceOf<CommentDto>(result);
        Assert.AreEqual("Explanation", result.Content);
        Assert.AreEqual(username, result.Username);
    }

    [Test]
    public void GetRecentComments_ReturnsListOfCommentDto()
    {
        var quizDiscussionService = new QuizDiscussionService(_cacheRepositoryMock.Object, _explanationServiceMock.Object);
        var quizId = 1;

        _cacheRepositoryMock.Setup(c => c.Retrieve<Comment>(It.IsAny<string>()))
            .Returns(new List<Comment> { new Comment() });

        var result = quizDiscussionService.GetRecentComments(quizId);

        Assert.IsInstanceOf<IEnumerable<CommentDto>>(result);
    }

    [Test]
    public void GetRecentComments_TypeMismatchException()
    {
        var quizDiscussionService = new QuizDiscussionService(_cacheRepositoryMock.Object, _explanationServiceMock.Object);
        var quizId = 1;

        _cacheRepositoryMock.Setup(c => c.Retrieve<Comment>(It.IsAny<string>()))
            .Throws(new TypeMismatchException(typeof(Comment), typeof(string), "Type mismatch"));

        _cacheRepositoryMock.Setup(c => c.Clear(It.IsAny<string>()));

        Assert.Throws<InternalException>(() => quizDiscussionService.GetRecentComments(quizId));
        _cacheRepositoryMock.Verify(c => c.Clear(It.IsAny<string>()), Times.Exactly(3));
    }
}