using Microsoft.AspNetCore.Mvc;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("/leaderboard")]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    [HttpGet("users")]
    public ActionResult<IEnumerable<UserLeaderboardResponseDto>> GetUsersLeaderboard()
    {
        return Ok(_leaderboardService.GetUsersLeaderboard());
    }
}