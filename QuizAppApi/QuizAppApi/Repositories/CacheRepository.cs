using QuizAppApi.Interfaces;

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

        if (entity != null) _cache[key].Add(entity);
    }

    public IEnumerable<TEntity> Retrieve<TEntity>(string key)
    {
        if (!_cache.ContainsKey(key)) return new List<TEntity>();
        return _cache[key].Cast<TEntity>();
    }

    public void Clear(string key)
    {
        _cache.Remove(key);
    }

    public object Lock { get; } = new object();
}