using Microsoft.AspNetCore.Mvc;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("/login")]
[RequireHttps]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    public async Task<ActionResult<TokenResponseDto>> Login([FromBody] UserRequestDto request)
    {
        var token = await _loginService.LoginAsync(request.Username, request.Password);
        if (token == null) return Unauthorized();
        return Ok(new TokenResponseDto { Token = token });
    }
}