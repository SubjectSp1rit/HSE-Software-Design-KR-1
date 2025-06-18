using KR_1.Domain;

namespace KR_1.Repositories;

public class RepositoryProxy<T> : IRepository<T> where T : IEntity
{
    private readonly IRepository<T> _innerRepository;
    private Dictionary<Guid, T> _cache;

    public RepositoryProxy(IRepository<T> innerRepository)
    {
        _innerRepository = innerRepository;
        _cache = _innerRepository.GetAll().ToDictionary(x => x.Id);
    }

    public void Add(T item)
    {
        _innerRepository.Add(item);
        _cache[item.Id] = item;
    }

    public void Update(T item)
    {
        _innerRepository.Update(item);
        _cache[item.Id] = item;
    }

    public void Delete(Guid id)
    {
        _innerRepository.Delete(id);
        _cache.Remove(id);
    }

    public T Get(Guid id)
    {
        if (_cache.TryGetValue(id, out var item))
            return item;
        item = _innerRepository.Get(id);
        if (item != null)
            _cache[item.Id] = item;
        return item;
    }

    public IEnumerable<T> GetAll()
    {
        return _cache.Values;
    }
}