using Microsoft.AspNetCore.Mvc;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [RequireHttps]
    public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] UserRequestDto request)
    {
        var user = await _userService.CreateUserAsync(request);
        if (user == null) return Conflict();
        return CreatedAtAction(nameof(GetUser), new {
            username = request.Username
        }, user);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserResponseDto>> GetUser(string username)
    {
        var user = await _userService.GetUserAsync(username);
        if (user == null) return NotFound();
        return Ok(user);
    }
}