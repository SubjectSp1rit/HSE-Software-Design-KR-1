using KR_1.Domain.Visitors;
using KR_1.Domain;

namespace KR_1_Tests.Mock_objects;

internal class TestVisitor : IExportVisitor
{
    public bool BankAccountVisited { get; private set; }
    public bool CategoryVisited { get; private set; }
    public bool OperationVisited { get; private set; }
    public void Visit(BankAccount account) => BankAccountVisited = true;
    public void Visit(Category category) => CategoryVisited = true;
    public void Visit(Operation operation) => OperationVisited = true;
}