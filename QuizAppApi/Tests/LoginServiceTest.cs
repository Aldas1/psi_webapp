using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Services;
using BC = BCrypt.Net.BCrypt;

namespace Tests;

[TestFixture]
public class LoginServiceTest
{
    private ILoginService? _loginService;
    private Mock<IUserRepository>? _mockUserRepository;
    private Mock<IConfiguration>? _mockConfiguration;

    [SetUp]
    public void Setup()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockConfiguration = new Mock<IConfiguration>();
        _loginService = new LoginService(_mockUserRepository.Object, _mockConfiguration.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _mockUserRepository = null;
        _mockConfiguration = null;
        _loginService = null;
    }

    [Test]
    public void Login_HandlesUnauthorizedUser()
    {
        var password = "secure password";
        var user = new User("testUser", BC.HashPassword(password));
        _mockUserRepository.Setup(repo => repo.GetUser(user.Username)).Returns(user);

        var result = _loginService.Login(user.Username, "wrong password");

        Assert.IsNull(result);
        _mockUserRepository.Verify(repo => repo.GetUser(user.Username), Times.Once);
    }

    [Test]
    public void Login_GeneratesToken()
    {
        var password = "secure password";
        var user = new User("testUser", BC.HashPassword(password));
        _mockUserRepository.Setup(repo => repo.GetUser(user.Username)).Returns(user);
        _mockConfiguration.Setup(conf => conf["JWT_SECRET"]).Returns("sekjf;lkjfkjalkdjf;f   fadf dfad f dfalkdjf");

        var result = _loginService.Login(user.Username, password);

        Assert.IsNotNull(result);
        _mockUserRepository.Verify(repo => repo.GetUser(user.Username), Times.Once);
    }
}