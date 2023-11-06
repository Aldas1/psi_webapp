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
    public ActionResult<UserResponseDto> CreateUser([FromBody] UserRequestDto request)
    {
        var user = _userService.CreateUser(request);
        if (user == null) return Conflict();
        return CreatedAtAction(nameof(GetUser), new {
            username = request.Username
        }, user);
    }

    [HttpGet("{username}")]
    public ActionResult<UserResponseDto> GetUser(string username)
    {
        var user = _userService.GetUser(username);
        if (user == null) return NotFound();
        return Ok(user);
    }
}