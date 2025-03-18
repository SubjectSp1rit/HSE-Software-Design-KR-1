using KR_1.Domain;
using KR_1.Repositories;
using KR_1.Factories;

namespace KR_1.Facades;

public class BankAccountFacade
{
    private readonly IRepository<BankAccount> _repository;

    public BankAccountFacade(IRepository<BankAccount> repository)
    {
        _repository = repository;
    }

    public BankAccount CreateBankAccount(string name, decimal initialBalance)
    {
        var account = DomainFactory.CreateBankAccount(name, initialBalance);
        _repository.Add(account);
        return account;
    }

    public void UpdateBankAccount(BankAccount account)
    {
        _repository.Update(account);
    }

    public void DeleteBankAccount(Guid id)
    {
        _repository.Delete(id);
    }

    public IEnumerable<BankAccount> GetAllBankAccounts()
    {
        return _repository.GetAll();
    }
}