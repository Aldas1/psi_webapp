namespace QuizAppApi.Interfaces;

public interface ICacheRepository
{
    void Add<TEntity>(string key, TEntity entity);
    IEnumerable<TEntity> Retrieve<TEntity>(string key);
    void Clear(string key);
    public object Lock { get;  }
}