using Microsoft.AspNetCore.Mvc;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("quizzes/{id}/questions")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;
    private readonly IExplanationService _explanationService;
    
    public QuestionController(IQuestionService questionService, IExplanationService explanationService)
    {
        _questionService = questionService;
        _explanationService = explanationService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuestionResponseDto>>> GetQuestions(int id)
    {
        var questions = await _questionService.GetQuestionsAsync(id);
        if (questions == null)
        {
            return NotFound();
        }
        return Ok(questions);
    }
    
    [HttpGet("generate-explanation")]
    public async Task<ActionResult<string>> GenerateExplanation(int quizId, int? questionId)
    {
        try
        {
            var questions = await _questionService.GetQuestionsAsync(quizId);

            if (questions == null)
            {
                return NotFound("Quiz not found");
            }

            if (questionId.HasValue)
            {
                var question = questions.FirstOrDefault(q => q.Id == questionId.Value);

                if (question == null)
                {
                    return NotFound("Question not found");
                }

                var explanation = await _explanationService.GenerateExplanationAsync(question);

                if (string.IsNullOrEmpty(explanation))
                {
                    return StatusCode(500, "Failed to generate explanation");
                }

                return Ok(explanation);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred");
        }

        return BadRequest("Invalid request");
    }
}