using KR_1.Domain;
using KR_1.Repositories;

namespace KR_1.Facades;

public class DataManagementFacade
{
    private readonly IRepository<BankAccount> _bankAccountRepository;
    private readonly IRepository<Operation> _operationRepository;

    public DataManagementFacade(IRepository<BankAccount> bankAccountRepository, IRepository<Operation> operationRepository)
    {
        _bankAccountRepository = bankAccountRepository;
        _operationRepository = operationRepository;
    }

    public void RecalculateBalance(Guid bankAccountId)
    {
        var account = _bankAccountRepository.Get(bankAccountId);
        if (account == null)
        {
            Console.WriteLine("Счет не найден");
            return;
        }

        decimal balance = 0;
        foreach (var op in _operationRepository.GetAll().Where(o => o.BankAccountId == bankAccountId))
        {
            balance += op.Type == TransactionType.Income ? op.Amount : -op.Amount;
        }

        account.SetBalance(balance);
        _bankAccountRepository.Update(account);
        Console.WriteLine($"Новый баланс счета \"{account.Name}\": {account.Balance}");
    }
}