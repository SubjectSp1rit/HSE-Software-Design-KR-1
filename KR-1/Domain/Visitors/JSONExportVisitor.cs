namespace KR_1.Domain.Visitors;

public class JSONExportVisitor : IExportVisitor
{
    public void Visit(BankAccount account)
    {
        Console.WriteLine($"Экспорт в файл JSON (BankAccount): {account.Id}, {account.Name}, {account.Balance}");
    }

    public void Visit(Category category)
    {
        Console.WriteLine($"Экспорт в файл JSON (Category): {category.Id}, {category.Name}, {category.Type}");
    }

    public void Visit(Operation operation)
    {
        Console.WriteLine($"Экспорт в файл JSON (Operation): {operation.Id}, {operation.Type}, {operation.Amount}, {operation.Date}, {operation.Description}");
    }
}