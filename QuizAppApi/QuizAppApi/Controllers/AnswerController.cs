using Microsoft.AspNetCore.Mvc;
using QuizAppApi.DTOs;
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
    public ActionResult<AnswerSubmitResponseDTO> SubmitQuizAnswers(int id, [FromBody] List<AnswerSubmitRequestDTO> request)
    {
        return Ok(_answerService.SubmitAnswers(id, request));
    }
}