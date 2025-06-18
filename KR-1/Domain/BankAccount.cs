using KR_1.Domain.Visitors;

namespace KR_1.Domain;

public class BankAccount : IEntity, IExportable
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public decimal Balance { get; private set; }

    public BankAccount(Guid id, string name, decimal balance)
    {
        Id = id;
        Name = name;
        Balance = balance;
    }

    public void AdjustBalance(decimal amount) => Balance += amount;
    public void SetBalance(decimal balance) => Balance = balance;

    public void Accept(IExportVisitor visitor) => visitor.Visit(this);
}