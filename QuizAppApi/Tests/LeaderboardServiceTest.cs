using NUnit.Framework;
using Moq;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Dtos;
using QuizAppApi.Services;

namespace Tests
{
    [TestFixture]
    public class LeaderboardServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private LeaderboardService _leaderboardService;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _leaderboardService = new LeaderboardService(_userRepositoryMock.Object);
        }

        [Test]
        public async Task GetUsersLeaderboardAsync_ReturnsLeaderboard()
        {
        string User1 = "User1";
        string User2 = "User2";
        string User3 = "User3";

        var users = new List<User>
        {
            new User(User1, "Password1") { TotalScore = 100, NumberOfSubmissions = 5 },
            new User(User2, "Password2") { TotalScore = 80, NumberOfSubmissions = 4 },
            new User(User3, "Password3") { TotalScore = 120, NumberOfSubmissions = 3 },
        };

        _userRepositoryMock.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(users);

        var result = await _leaderboardService.GetUsersLeaderboardAsync();

        Assert.IsInstanceOf<IEnumerable<UserLeaderboardResponseDto>>(result);
        var resultList = result.ToList();
        Assert.AreEqual(users.Count, resultList.Count);

        Assert.IsTrue(resultList.Any(user => user.Username == User3));
        Assert.IsTrue(resultList.Any(user => user.Username == User1));
        Assert.IsTrue(resultList.Any(user => user.Username == User2));
        }

        [Test]
        public async Task GetUsersLeaderboardAsync_EmptyList_ReturnsEmptyLeaderboard()
        {
            _userRepositoryMock.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(new List<User>());

            var result = await _leaderboardService.GetUsersLeaderboardAsync();

            Assert.IsEmpty(result);
        }
    }
}
