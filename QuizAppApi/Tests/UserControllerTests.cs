using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using QuizAppApi.Controllers;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;

namespace Tests;

[TestFixture]
public class UserControllerTests
{
    private Mock<IUserService> _userServiceMock;
    private UserController _controller;

    [SetUp]
    public void Setup()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UserController(_userServiceMock.Object);
    }

    [Test]
    public async Task CreateUser_ReturnsCreatedAtAction()
    {
        var userRequestDto = new UserRequestDto { Username = "testUser", Password = "TestPassword1" };
        var expectedResponse = new UserResponseDto
        {
            Username = userRequestDto.Username,
            TotalScore = 0,
            NumberOfSubmissions = 0
        };

        _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<UserRequestDto>()))
            .Returns(Task.FromResult(expectedResponse));

        var result = await _controller.CreateUser(userRequestDto);

        Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        var createdAtActionResult = (CreatedAtActionResult)result.Result;
        Assert.AreEqual("GetUser", createdAtActionResult.ActionName);
        Assert.AreEqual(userRequestDto.Username, createdAtActionResult.RouteValues["username"]);
        Assert.IsInstanceOf<UserResponseDto>(createdAtActionResult.Value);
        Assert.AreEqual(expectedResponse, createdAtActionResult.Value);
    }

    [Test]
    public async Task CreateUser_ReturnsConflictResult()
    {
        var existingUsername = "existingUser";
        var existingUserRequestDto = new UserRequestDto { Username = existingUsername, Password = "ExistingPassword1" };

        _userServiceMock.Setup(x => x.CreateUserAsync(existingUserRequestDto))
            .ReturnsAsync((UserResponseDto)null);

        var result = await _controller.CreateUser(existingUserRequestDto);

        Assert.IsInstanceOf<ConflictResult>(result.Result);
    }

    [Test]
    public async Task GetUser_ReturnsOk()
    {
        var existingUsername = "existingUser";
        var expectedResponse = new UserResponseDto
        {
            Username = existingUsername,
            TotalScore = 10,
            NumberOfSubmissions = 2
        };

        _userServiceMock.Setup(x => x.GetUserAsync(existingUsername))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.GetUser(existingUsername);

        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okObjectResult = (OkObjectResult)result.Result;
        Assert.IsInstanceOf<UserResponseDto>(okObjectResult.Value);
        Assert.AreEqual(expectedResponse, okObjectResult.Value);
    }

    [Test]
    public async Task GetUser_ReturnsNotFound()
    {
        var nonExistingUsername = "nonExistingUser";

        _userServiceMock.Setup(x => x.GetUserAsync(nonExistingUsername))
            .ReturnsAsync((UserResponseDto)null);

        var result = await _controller.GetUser(nonExistingUsername);

        Assert.IsInstanceOf<NotFoundResult>(result.Result);
    }
}