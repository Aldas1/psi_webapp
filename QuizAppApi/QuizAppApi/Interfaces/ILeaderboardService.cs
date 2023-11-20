using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;

public interface ILeaderboardService
{
    public Task<IEnumerable<UserLeaderboardResponseDto>> GetUsersLeaderboardAsync();
}