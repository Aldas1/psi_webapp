using QuizAppApi.Models.Flashcards;

namespace QuizAppApi.Interfaces;

public interface IFlashcardRepository
{
    Task<Flashcard> CreateAsync(Flashcard flashcard);
    Task<Flashcard?> GetByIdAsync(int id);
    Task<IEnumerable<Flashcard>> GetAsync();
    Task<IEnumerable<Flashcard>> GetByCollectionAsync(int collectionId);
}