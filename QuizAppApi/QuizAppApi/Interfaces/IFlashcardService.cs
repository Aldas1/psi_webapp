using QuizAppApi.Dtos.Flashcards;

namespace QuizAppApi.Interfaces;

public interface IFlashcardService
{
    Task<FlashcardDto> CreateAsync(int collectionId, FlashcardDto flashcardDto);
    Task<IEnumerable<FlashcardDto>> GetAsync(int collectionId);
    Task<FlashcardDto?> UpdateAsync(int id, FlashcardDto flashcardDto);
    Task DeleteAsync(int id);
}