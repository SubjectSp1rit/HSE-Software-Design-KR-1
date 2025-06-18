using KR_1.Domain.Visitors;

namespace KR_1.Domain;

public class Category : IEntity, IExportable
{
    public Guid Id { get; private set; }
    public TransactionType Type { get; private set; }
    public string Name { get; private set; }

    public Category(Guid id, TransactionType type, string name)
    {
        Id = id;
        Type = type;
        Name = name;
    }

    public void Accept(IExportVisitor visitor)
    {
        visitor.Visit(this);
    }
}