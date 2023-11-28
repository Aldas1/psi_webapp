using Microsoft.AspNetCore.Mvc;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("[controller]")]
[RequireHttps]
public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpPost]
    public async Task<ActionResult<QuizManipulationResponseDto>> CreateQuiz([FromBody] QuizManipulationRequestDto request)
    {
        return await _quizService.CreateQuizAsync(request, (User?)HttpContext.Items["User"]);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<QuizManipulationResponseDto>> UpdateQuiz(int id, [FromBody] QuizManipulationRequestDto updateRequest)
    {
        if (!(await _quizService.CanUserEditQuizAsync((User?)HttpContext.Items["User"], id)))
        {
            return Forbid();
        }
        return await _quizService.UpdateQuizAsync(id, updateRequest);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizResponseDto>>> GetQuizzes()
    {
        var quizzes = await _quizService.GetQuizzesAsync();
        if (quizzes == null)
        {
            return NotFound();
        }
        return Ok(quizzes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QuizResponseDto>> GetQuiz(int id)
    {
        var quiz = await _quizService.GetQuizAsync(id);
        if (quiz == null)
        {
            return NotFound();
        }

        return Ok(quiz);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteQuiz(int id)
    {
        if (!(await _quizService.CanUserEditQuizAsync((User?)HttpContext.Items["User"], id)))
        {
            return Forbid();
        }
        bool response = await _quizService.DeleteQuizAsync(id);
        if (response)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }
}