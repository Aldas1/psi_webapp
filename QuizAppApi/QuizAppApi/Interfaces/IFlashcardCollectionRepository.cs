using QuizAppApi.Models.Flashcards;

namespace QuizAppApi.Interfaces;

public interface IFlashcardCollectionRepository
{
    Task<FlashcardCollection> CreateAsync(FlashcardCollection collection);
    Task<FlashcardCollection?> GetByIdAsync(int id);
    Task<IEnumerable<FlashcardCollection>> GetAsync();
}