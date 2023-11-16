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
    public async Task<ActionResult<IEnumerable<UserLeaderboardResponseDto>>> GetUsersLeaderboard()
    {
        return Ok(await _leaderboardService.GetUsersLeaderboardAsync());
    }
}