using KR_1.Domain;

namespace KR_1.Repositories;

public interface IRepository<T> where T : IEntity
{
    void Add(T item);
    void Update(T item);
    void Delete(Guid id);
    T Get(Guid id);
    IEnumerable<T> GetAll();
}