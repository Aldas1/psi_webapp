using Microsoft.AspNetCore.Mvc;
using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("questions")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet("{id}/questions")]
    public ActionResult<IEnumerable<QuestionResponseDTO>> GetQuestions(int id)
    {
        var questions = _questionService.GetQuestions(id);
        if (questions == null)
        {
            return NotFound();
        }
        return Ok(questions);
    }
}