using Moq;
using NUnit.Framework;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Services;

namespace Tests;

[TestFixture]
public class UserServiceTest
{
    private IUserService? _userService;
    private Mock<IUserRepository>? _mockUserRepository;

    [SetUp]
    public void Setup()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockUserRepository.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _mockUserRepository = null;
        _userService = null;
    }

    [Test]
    public void CreateUser_CreatesNewUser()
    {
        var request = new UserRequestDto { Username = "testUser", Password = "securePassword" };

        var response = _userService.CreateUser(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(response.Username, request.Username);
        Assert.AreEqual(response.TotalScore, 0);
        Assert.AreEqual(response.NumberOfSubmissions, 0);
        _mockUserRepository.Verify(repo => repo.AddUser(It.Is<User>(u => u.Username == request.Username)), Times.Once);
    }

    [Test]
    public void GetUser_ReturnUserIfItExists()
    {
        var user = new User("testUser", "pjfkjfopjaoiw");
        _mockUserRepository.Setup(repo => repo.GetUser(user.Username)).Returns(user);
        
        var response = _userService.GetUser(user.Username);
        
        Assert.IsNotNull(response);
        Assert.AreEqual(response.Username, user.Username);
        _mockUserRepository.Verify(repo => repo.GetUser(user.Username), Times.Once);
    }
    
    [Test]
    public void GetUser_ReturnNullIfNoUserIsFound()
    {
        var username = "non existent username";
        _mockUserRepository.Setup(repo => repo.GetUser(It.IsAny<String>())).Returns((User?)null);
        
        var response = _userService.GetUser(username);
        
        Assert.IsNull(response);
        _mockUserRepository.Verify(repo => repo.GetUser(username), Times.Once);
    }
}