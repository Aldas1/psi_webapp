using Microsoft.EntityFrameworkCore;
using QuizAppApi.Data;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Flashcards;

namespace QuizAppApi.Repositories;

public class FlashcardRepository : IFlashcardRepository
{
    private readonly QuizContext _context;

    public FlashcardRepository(QuizContext context)
    {
        _context = context;
    }

    public async Task<Flashcard> CreateAsync(Flashcard flashcard)
    {
        var e = (await _context.Flashcards.AddAsync(flashcard)).Entity;
        await _context.SaveChangesAsync();
        return e;
    }

    public async Task<Flashcard?> GetByIdAsync(int id)
    {
        return await _context.Flashcards.FindAsync(id);
    }

    public async Task<IEnumerable<Flashcard>> GetAsync()
    {
        return await _context.Flashcards.ToListAsync();
    }

    public async Task<IEnumerable<Flashcard>> GetByCollectionAsync(int collectionId)
    {
        return await _context.Flashcards.Where(f => f.FlashcardCollectionId == collectionId).ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Flashcards.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}