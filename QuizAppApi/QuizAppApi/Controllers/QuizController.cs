using Microsoft.AspNetCore.Mvc;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("quizzes")]
public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpPost]
    public ActionResult<QuizManipulationResponseDto> CreateQuiz([FromBody] QuizManipulationRequestDto request)
    {
        return _quizService.CreateQuiz(request);
    }

    [HttpPut("{id}")]
    public ActionResult<QuizManipulationResponseDto> UpdateQuiz(int id, [FromBody] QuizManipulationRequestDto updateRequest)
    {
        return _quizService.UpdateQuiz(id, updateRequest);
    }

    [HttpGet]
    public ActionResult<IEnumerable<QuizResponseDto>> GetQuizzes()
    {
        var quizzes = _quizService.GetQuizzes();
        if (quizzes == null)
        {
            return NotFound();
        }
        return Ok(quizzes);
    }

    [HttpGet("{id}")]
    public ActionResult<QuizResponseDto> GetQuiz(int id)
    {
        var quiz = _quizService.GetQuiz(id);
        if (quiz == null)
        {
            return NotFound();
        }

        return Ok(quiz);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteQuiz(int id)
    {
        bool response = _quizService.DeleteQuiz(id);
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