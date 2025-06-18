using KR_1.Domain;
using KR_1.Repositories;
using KR_1.Factories;

namespace KR_1.Facades;

public class OperationFacade
{
    private readonly IRepository<Operation> _repository;
    private readonly BankAccountFacade _bankAccountFacade;
    private readonly CategoryFacade _categoryFacade;

    public OperationFacade(IRepository<Operation> repository, BankAccountFacade bankAccountFacade, CategoryFacade categoryFacade)
    {
        _repository = repository;
        _bankAccountFacade = bankAccountFacade;
        _categoryFacade = categoryFacade;
    }

    public Operation CreateOperation(TransactionType type, BankAccount account, decimal amount, DateTime date, string description, Category category)
    {
        var operation = DomainFactory.CreateOperation(type, account, amount, date, description, category);
        _repository.Add(operation);
        return operation;
    }

    public void DeleteOperation(Guid id)
    {
        _repository.Delete(id);
    }

    public IEnumerable<Operation> GetAllOperations()
    {
        return _repository.GetAll();
    }
}