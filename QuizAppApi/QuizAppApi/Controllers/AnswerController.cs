using Microsoft.AspNetCore.Mvc;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("answers")]
public class AnswerController : ControllerBase
{
    private readonly IAnswerService _answerService;

    public AnswerController(IAnswerService answerService)
    {
        _answerService = answerService;
    }

    [HttpPost("{id}/submit")]
    [RequireHttps]
    public ActionResult<AnswerSubmitResponseDto> SubmitQuizAnswers(int id, [FromBody] List<AnswerSubmitRequestDto> request)
    {
        return Ok(_answerService.SubmitAnswers(id, request, (string?)HttpContext.Items["UserName"]));
    }
}