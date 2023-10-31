using Microsoft.AspNetCore.Mvc;
using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("quizzes")]
public class QuestionController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuestionController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpGet("{id}/questions")]
    public ActionResult<IEnumerable<QuestionResponseDTO>> GetQuestions(int id)
    {
        var questions = _quizService.GetQuestions(id);
        if (questions == null)
        {
            return NotFound();
        }
        return Ok(questions);
    }

    [HttpPost("{id}/submit")]
    public async Task<ActionResult<AnswerSubmitResponseDTO>> SubmitAnswers(int id, [FromBody] List<AnswerSubmitRequestDTO> request)
    {
        return await _quizService.SubmitAnswers(id, request);
    }
}