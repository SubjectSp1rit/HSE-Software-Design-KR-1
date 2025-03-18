using KR_1.Domain.Visitors;
using KR_1.Domain;

namespace KR_1_Tests;

public class VisitorTests
{
    [Fact]
    public void CSVExportVisitor_WritesExpectedOutput()
    {
        // Arrange
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            var visitor = new CSVExportVisitor();
            var account = new BankAccount(Guid.NewGuid(), "Visitor Test", 100m);

            // Act
            account.Accept(visitor);
            var output = sw.ToString();

            // Assert
            Assert.Contains("Экспорт в файл CSV (BankAccount)", output);
        }
    }
    
    [Fact]
    public void YAMLExportVisitor_WritesExpectedOutput()
    {
        // Arrange
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            var visitor = new YAMLExportVisitor();
            var category = new Category(Guid.NewGuid(), TransactionType.Expense, "Visitor Cat");

            // Act
            category.Accept(visitor);
            var output = sw.ToString();

            // Assert
            Assert.Contains("Экспорт в файл YAML (Category)", output);
        }
    }
    
    [Fact]
    public void JSONExportVisitor_WritesExpectedOutput()
    {
        // Arrange
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            var visitor = new JSONExportVisitor();
            var operation = new Operation(Guid.NewGuid(), TransactionType.Expense, Guid.NewGuid(), 75m, DateTime.Now, "Visitor Op", Guid.NewGuid());

            // Act
            operation.Accept(visitor);
            var output = sw.ToString();

            // Assert
            Assert.Contains("Экспорт в файл JSON (Operation)", output);
        }
    }
}