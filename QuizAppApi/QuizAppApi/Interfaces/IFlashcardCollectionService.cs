using QuizAppApi.Dtos.Flashcards;

namespace QuizAppApi.Interfaces;

public interface IFlashcardCollectionService
{
    Task<FlashcardCollectionDto> CreateAsync(FlashcardCollectionDto collectionDto);
    Task<IEnumerable<FlashcardCollectionDto>> CreateFromQuizzesAsync(IEnumerable<int> quizIDs);
    Task<FlashcardCollectionDto?> GetByIdAsync(int id);
    Task<IEnumerable<FlashcardCollectionDto>> GetAsync();
    Task DeleteAsync(int id);
}