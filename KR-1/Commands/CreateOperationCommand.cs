using KR_1.Facades;
using KR_1.Domain;

namespace KR_1.Commands;

public class CreateOperationCommand : ICommand
{
    private readonly OperationFacade _operationFacade;
    private readonly BankAccountFacade _bankAccountFacade;
    private readonly CategoryFacade _categoryFacade;
    private readonly Guid _bankAccountId;
    private readonly Guid _categoryId;
    private readonly TransactionType _type;
    private readonly decimal _amount;
    private readonly DateTime _date;
    private readonly string _description;

    public CreateOperationCommand(
        OperationFacade operationFacade,
        BankAccountFacade bankAccountFacade,
        CategoryFacade categoryFacade,
        Guid bankAccountId,
        Guid categoryId,
        TransactionType type,
        decimal amount,
        DateTime date,
        string description)
    {
        _operationFacade = operationFacade;
        _bankAccountFacade = bankAccountFacade;
        _categoryFacade = categoryFacade;
        _bankAccountId = bankAccountId;
        _categoryId = categoryId;
        _type = type;
        _amount = amount;
        _date = date;
        _description = description;
    }

    public void Execute()
    {
        var account = _bankAccountFacade.GetAllBankAccounts().FirstOrDefault(a => a.Id == _bankAccountId);
        var category = _categoryFacade.GetAllCategories().FirstOrDefault(c => c.Id == _categoryId);
        if (account == null || category == null)
        {
            Console.WriteLine("Счет или категория не найдены");
            return;
        }
        var operation = _operationFacade.CreateOperation(_type, account, _amount, _date, _description, category);
        Console.WriteLine($"Создана операция {operation.Type} на сумму {operation.Amount} от {operation.Date}");
    }
}