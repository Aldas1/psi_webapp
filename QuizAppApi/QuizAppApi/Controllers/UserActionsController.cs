using Microsoft.AspNetCore.Mvc;
using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("actions")]
public class UserActionsController : ControllerBase
{
    private readonly IUserActionsService _userActionsService;

    public UserActionsController(IUserActionsService userActionsService)
    {
        _userActionsService = userActionsService;
    }

    [HttpPost("submitAnswers/{id}")]
    [RequireHttps]
    public ActionResult<AnswerSubmitResponseDTO> SubmitQuizAnswers(int id, [FromBody] List<AnswerSubmitRequestDTO> request)
    {
        return Ok(_userActionsService.SubmitAnswers(id, request));
    }
}