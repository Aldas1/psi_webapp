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

        if (_cache[key].Any() && _cache[key].First().GetType() != typeof(TEntity))
        {
            throw new TypeMismatchException(typeof(TEntity), _cache[key].First().GetType(), "add");
        }

        if (entity != null) _cache[key].Add(entity);
    }

    public IEnumerable<TEntity> Retrieve<TEntity>(string key)
    {
        if (!_cache.ContainsKey(key)) return new List<TEntity>();

        if (_cache[key].Any() && _cache[key].First().GetType() != typeof(TEntity))
        {
            throw new TypeMismatchException(typeof(TEntity), _cache[key].First().GetType(), "retrieve");
        }

        return _cache[key].Cast<TEntity>();
    }

    public void Clear(string key)
    {
        _cache.Remove(key);
    }

    public object Lock { get; } = new object();
}