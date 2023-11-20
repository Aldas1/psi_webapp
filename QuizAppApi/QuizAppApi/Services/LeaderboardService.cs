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

    public async Task<IEnumerable<UserLeaderboardResponseDto>> GetUsersLeaderboardAsync()
    {
        var user = await _userRepository.GetUsersAsync();
        return user.Select(user =>
        {
            return new UserLeaderboardResponseDto
            {
                Username = user.Username,
                AverageScore = user.TotalScore / (user.NumberOfSubmissions > 0 ? user.NumberOfSubmissions : 1),
                NumberOfSubmissions = user.NumberOfSubmissions
            };
        }).OrderByDescending(user => user.AverageScore).ThenByDescending(user => user.NumberOfSubmissions);
    }
}