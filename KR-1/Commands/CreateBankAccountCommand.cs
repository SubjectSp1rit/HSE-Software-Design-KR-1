using KR_1.Facades;

namespace KR_1.Commands;

public class CreateBankAccountCommand : ICommand
{
    private readonly BankAccountFacade _bankAccountFacade;
    private readonly string _name;
    private readonly decimal _initialBalance;

    public CreateBankAccountCommand(BankAccountFacade bankAccountFacade, string name, decimal initialBalance)
    {
        _bankAccountFacade = bankAccountFacade;
        _name = name;
        _initialBalance = initialBalance;
    }

    public void Execute()
    {
        var account = _bankAccountFacade.CreateBankAccount(_name, _initialBalance);
        Console.WriteLine($"Создан счет {account.Name} с балансом {account.Balance}");
    }
}