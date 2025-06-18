namespace KR_1.Domain.Visitors;

public class CSVExportVisitor : IExportVisitor
{
    public void Visit(BankAccount account)
    {
        Console.WriteLine($"Экспорт в файл CSV (BankAccount): {account.Id}, {account.Name}, {account.Balance}");
    }

    public void Visit(Category category)
    {
        Console.WriteLine($"Экспорт в файл CSV (Category): {category.Id}, {category.Name}, {category.Type}");
    }

    public void Visit(Operation operation)
    {
        Console.WriteLine($"Экспорт в файл CSV (Operation): {operation.Id}, {operation.Type}, {operation.Amount}, {operation.Date}, {operation.Description}");
    }
}