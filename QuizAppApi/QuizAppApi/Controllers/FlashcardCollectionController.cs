using Microsoft.AspNetCore.Mvc;
using QuizAppApi.Dtos.Flashcards;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("/flashcard-collections")]
public class FlashcardCollectionController : ControllerBase
{
    private readonly IFlashcardCollectionService _collectionService;

    public FlashcardCollectionController(IFlashcardCollectionService collectionService)
    {
        _collectionService = collectionService;
    }

    [HttpPost]
    public async Task<ActionResult<FlashcardCollectionDto>> Create([FromBody] FlashcardCollectionDto collectionDto)
    {
        var res = await _collectionService.CreateAsync(collectionDto);
        return CreatedAtAction(nameof(GetById), new { res.Id }, res);
    }

    [HttpPost("create-from-quiz/{quizId}")]
    public async Task<ActionResult<FlashcardCollectionDto>> CreateFromQuizzes(int quizId)
    {
        var createdCollection = await _collectionService.CreateFromQuizAsync(quizId);
        return CreatedAtAction(nameof(GetById), new { createdCollection.Id }, createdCollection);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FlashcardCollectionDto>> GetById(int id)
    {
        var collection = await _collectionService.GetByIdAsync(id);
        if (collection == null) return NotFound();
        return Ok(collection);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FlashcardCollectionDto>>> Get()
    {
        return Ok(await _collectionService.GetAsync());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _collectionService.DeleteAsync(id);
        return NoContent();
    }
}