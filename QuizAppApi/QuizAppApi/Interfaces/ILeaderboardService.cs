using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;

public interface ILeaderboardService
{
    public IEnumerable<UserLeaderboardResponseDto> GetUsersLeaderboard();
}