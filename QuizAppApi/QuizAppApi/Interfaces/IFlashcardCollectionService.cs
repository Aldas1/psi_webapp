using QuizAppApi.Dtos.Flashcards;

namespace QuizAppApi.Interfaces;

public interface IFlashcardCollectionService
{
    Task<FlashcardCollectionDto> CreateAsync(FlashcardCollectionDto collectionDto);
    Task<FlashcardCollectionDto?> GetByIdAsync(int id);
    Task<IEnumerable<FlashcardCollectionDto>> GetAsync();
}