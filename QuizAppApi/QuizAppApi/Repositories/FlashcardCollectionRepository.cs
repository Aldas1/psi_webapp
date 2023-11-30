using Microsoft.EntityFrameworkCore;
using QuizAppApi.Data;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Flashcards;

namespace QuizAppApi.Repositories;

public class FlashcardCollectionRepository : IFlashcardCollectionRepository
{
    private readonly QuizContext _context;

    public FlashcardCollectionRepository(QuizContext context)
    {
        _context = context;
    }

    public async Task<FlashcardCollection> CreateAsync(FlashcardCollection collection)
    {
        var e = (await _context.FlashcardCollections.AddAsync(collection)).Entity;
        await _context.SaveChangesAsync();
        return e;
    }

    public async Task<FlashcardCollection?> GetByIdAsync(int id)
    {
        return await _context.FlashcardCollections.FindAsync(id);
    }

    public async Task<IEnumerable<FlashcardCollection>> GetAsync()
    {
        return await _context.FlashcardCollections.ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.FlashcardCollections.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}