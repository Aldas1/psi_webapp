using Microsoft.AspNetCore.Mvc;
using QuizAppApi.Dtos.Flashcards;
using QuizAppApi.Interfaces;

namespace QuizAppApi.Controllers;

[ApiController]
[Route("/flashcard-collections/{collectionId}/flashcards")]
public class FlashcardController : ControllerBase
{
    private readonly IFlashcardService _flashcardService;
    private readonly IFlashcardCollectionService _flashcardCollectionService;

    public FlashcardController(IFlashcardService flashcardService, IFlashcardCollectionService flashcardCollectionService)
    {
        _flashcardService = flashcardService;
        _flashcardCollectionService = flashcardCollectionService;
    }

    [HttpPost]
    public async Task<ActionResult<FlashcardDto>> Create(int collectionId, [FromBody] FlashcardDto flashcardDto)
    {
        if ((await _flashcardCollectionService.GetByIdAsync(collectionId)) == null) return NotFound();
        return CreatedAtAction(nameof(Get), new {CollectionId = collectionId}, await _flashcardService.CreateAsync(collectionId, flashcardDto));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FlashcardDto>>> Get(int collectionId)
    {
        return Ok(await _flashcardService.GetAsync(collectionId));
    }

    [HttpPut("/flashcards/{id}")]
    public async Task<ActionResult<FlashcardCollectionDto>> Update(int id, [FromBody] FlashcardDto flashcardDto)
    {
        var res = await _flashcardService.UpdateAsync(id, flashcardDto);
        if (res == null) return NotFound();
        return Ok(res);
    }

    [HttpDelete("/flashcards/{id}")]    
    public async Task<IActionResult> Delete(int id)
    {
        await _flashcardService.DeleteAsync(id);
        return NoContent();
    }
}