using KR_1.Domain;
using KR_1.Data;

namespace KR_1.Repositories;

public class BankAccountRepository : IRepository<BankAccount>
{
    private readonly DataContext _context;

    public BankAccountRepository(DataContext context)
    {
        _context = context;
    }

    public void Add(BankAccount item)
    {
        _context.BankAccounts.Add(item);
    }

    public void Update(BankAccount item)
    {
        var index = _context.BankAccounts.FindIndex(x => x.Id == item.Id);
        if (index >= 0)
            _context.BankAccounts[index] = item;
    }

    public void Delete(Guid id)
    {
        _context.BankAccounts.RemoveAll(x => x.Id == id);
    }

    public BankAccount Get(Guid id)
    {
        return _context.BankAccounts.FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<BankAccount> GetAll()
    {
        return _context.BankAccounts;
    }
}