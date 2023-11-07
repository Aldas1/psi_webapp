using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Services;

public class LeaderboardService : ILeaderboardService
{
    public IEnumerable<UserLeaderboardResponseDto> GetUsersLeaderboard()
    {
        throw new NotImplementedException();
    }
}