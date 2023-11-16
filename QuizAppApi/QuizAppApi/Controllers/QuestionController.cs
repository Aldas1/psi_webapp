﻿using Microsoft.AspNetCore.Mvc;
using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("quizzes/{id}/questions")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
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
}