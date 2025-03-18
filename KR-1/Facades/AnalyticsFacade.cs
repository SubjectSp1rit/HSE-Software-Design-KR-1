using KR_1.Domain;
using KR_1.Repositories;

namespace KR_1.Facades;

public class AnalyticsFacade
{
    private readonly IRepository<Operation> _operationRepository;
    private readonly IRepository<Category> _categoryRepository;

    public AnalyticsFacade(IRepository<Operation> operationRepository, IRepository<Category> categoryRepository)
    {
        _operationRepository = operationRepository;
        _categoryRepository = categoryRepository;
    }

    public decimal CalculateIncomeExpenseDifference(DateTime from, DateTime to)
    {
        var operations = _operationRepository.GetAll().Where(op => op.Date >= from && op.Date <= to);
        decimal totalIncome = operations.Where(op => op.Type == TransactionType.Income).Sum(op => op.Amount);
        decimal totalExpense = operations.Where(op => op.Type == TransactionType.Expense).Sum(op => op.Amount);
        return totalIncome - totalExpense;
    }

    public IEnumerable<IGrouping<Guid, Operation>> GroupOperationsByCategory()
    {
        return _operationRepository.GetAll().GroupBy(op => op.CategoryId);
    }
}