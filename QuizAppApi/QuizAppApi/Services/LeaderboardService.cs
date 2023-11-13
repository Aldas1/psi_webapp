using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly IUserRepository _userRepository;

    public LeaderboardService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IEnumerable<UserLeaderboardResponseDto> GetUsersLeaderboard()
    {
        return _userRepository.GetUsers().Select(user =>
        {
            return new UserLeaderboardResponseDto
            {
                Username = user.Username,
                AverageScore = (double)user.TotalScore / (user.NumberOfSubmissions > 0 ? user.NumberOfSubmissions : 1),
                NumberOfSubmissions = user.NumberOfSubmissions
            };
        }).OrderByDescending(user => user.AverageScore).ThenByDescending(user => user.NumberOfSubmissions);
    }
}