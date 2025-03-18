using KR_1.Domain;
using KR_1.Data;

namespace KR_1.Repositories;

public class OperationRepository : IRepository<Operation>
{
    private readonly DataContext _context;

    public OperationRepository(DataContext context)
    {
        _context = context;
    }

    public void Add(Operation item)
    {
        _context.Operations.Add(item);
    }

    public void Update(Operation item)
    {
        var index = _context.Operations.FindIndex(x => x.Id == item.Id);
        if (index >= 0)
            _context.Operations[index] = item;
    }

    public void Delete(Guid id)
    {
        _context.Operations.RemoveAll(x => x.Id == id);
    }

    public Operation Get(Guid id)
    {
        return _context.Operations.FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<Operation> GetAll()
    {
        return _context.Operations;
    }
}