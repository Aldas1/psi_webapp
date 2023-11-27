using QuizAppApi.Dtos.Flashcards;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Flashcards;

namespace QuizAppApi.Services;

public class FlashcardCollectionService : IFlashcardCollectionService
{
    private readonly IFlashcardCollectionRepository _repository;

    public FlashcardCollectionService(IFlashcardCollectionRepository repository)
    {
        _repository = repository;
    }

    public async Task<FlashcardCollectionDto> CreateAsync(FlashcardCollectionDto collectionDto)
    {
        return ToDto(await _repository.CreateAsync(new FlashcardCollection { Name = collectionDto.Name }));
    }

    public async Task<FlashcardCollectionDto?> GetByIdAsync(int id)
    {
        var res = await _repository.GetByIdAsync(id);
        return res == null ? null : ToDto(res);
    }

    public async Task<IEnumerable<FlashcardCollectionDto>> GetAsync()
    {
        return (await _repository.GetAsync()).Select(ToDto);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static FlashcardCollectionDto ToDto(FlashcardCollection collection)
    {
        return new FlashcardCollectionDto
        {
            Id = collection.Id,
            Name = collection.Name
        };
    }
}