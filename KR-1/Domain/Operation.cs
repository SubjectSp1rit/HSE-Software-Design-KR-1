using KR_1.Domain.Visitors;

namespace KR_1.Domain;

public class Operation : IEntity, IExportable
{
    public Guid Id { get; private set; }
    public TransactionType Type { get; private set; }
    public Guid BankAccountId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }
    public Guid CategoryId { get; private set; }

    public Operation(Guid id, TransactionType type, Guid bankAccountId, decimal amount, DateTime date, string description, Guid categoryId)
    {
        Id = id;
        Type = type;
        if (amount < 0)
            throw new ArgumentException("Сумма операции не может быть отрицательной");
        Amount = amount;
        BankAccountId = bankAccountId;
        Date = date;
        Description = description;
        CategoryId = categoryId;
    }

    public void Accept(IExportVisitor visitor)
    {
        visitor.Visit(this);
    }
}