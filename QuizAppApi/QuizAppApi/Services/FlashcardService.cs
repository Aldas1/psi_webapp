using QuizAppApi.Dtos.Flashcards;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Flashcards;

namespace QuizAppApi.Services;

public class FlashcardService : IFlashcardService
{
    private readonly IFlashcardRepository _repository;

    public FlashcardService(IFlashcardRepository repository)
    {
        _repository = repository;
    }

    public async Task<FlashcardDto> CreateAsync(int collectionId, FlashcardDto flashcardDto)
    {
        return ToDto(await _repository.CreateAsync(new Flashcard
        {
            Question = flashcardDto.Question,
            Answer = flashcardDto.Answer,
            FlashcardCollectionId = collectionId
        }));
    }

    public async Task<IEnumerable<FlashcardDto>> GetAsync(int collectionId)
    {
        return (await _repository.GetByCollectionAsync(collectionId)).Select(ToDto);
    }

    private static FlashcardDto ToDto(Flashcard flashcard)
    {
        return new FlashcardDto
        {
            Id = flashcard.Id,
            Question = flashcard.Question,
            Answer = flashcard.Answer
        };
    }
}