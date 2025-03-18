using KR_1.Domain;
using KR_1_Tests.Mock_objects;

namespace KR_1_Tests;

public class OperationTests
{
    [Fact]
    public void Operation_StoresPropertiesCorrectly()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var now = DateTime.Now;
            
        // Act
        var operation = new Operation(Guid.NewGuid(), TransactionType.Income, accountId, 100m, now, "Test Operation", categoryId);
            
        // Assert
        Assert.Equal(100m, operation.Amount);
        Assert.Equal("Test Operation", operation.Description);
        Assert.Equal(accountId, operation.BankAccountId);
    }
    
    [Fact]
    public void Operation_WithNegativeAmount_ThrowsArgumentException()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
            
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        new Operation(Guid.NewGuid(), TransactionType.Expense, accountId, -50m, DateTime.Now, "Negative Test", categoryId));
    }
    
    [Fact]
    public void Accept_CallsVisitorMethod()
    {
        // Arrange
        var operation = new Operation(Guid.NewGuid(), TransactionType.Income, Guid.NewGuid(), 200m, DateTime.Now, "Op", Guid.NewGuid());
        var visitor = new TestVisitor();
            
        // Act
        operation.Accept(visitor);
            
        // Assert
        Assert.True(visitor.OperationVisited);
    }
}