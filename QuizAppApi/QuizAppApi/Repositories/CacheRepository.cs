using QuizAppApi.Interfaces;
using QuizAppApi.Exceptions;

namespace QuizAppApi.Repositories;

public class CacheRepository : ICacheRepository
{
    private readonly Dictionary<string, List<object>> _cache = new();

    public void Add<TEntity>(string key, TEntity entity)
    {
        if (!_cache.ContainsKey(key))
        {
            _cache[key] = new List<object>();
        }

        try 
        {
            if (entity != null) _cache[key].Add(entity);
        }
        catch (TypeMismatchException)
        {
            throw new TypeMismatchException(typeof(TEntity), _cache[key].First().GetType());
        }
        
    }

    public IEnumerable<TEntity> Retrieve<TEntity>(string key)
    {
        if (!_cache.ContainsKey(key)) return new List<TEntity>();

        try
        {
            return _cache[key].Cast<TEntity>();
        }
        catch (TypeMismatchException)
        {
            throw new TypeMismatchException(typeof(TEntity), _cache[key].First().GetType());
        }
        
    }

    public void Clear(string key)
    {
        _cache.Remove(key);
    }

    public object Lock { get; } = new object();
}