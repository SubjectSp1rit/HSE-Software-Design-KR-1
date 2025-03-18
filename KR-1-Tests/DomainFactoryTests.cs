using KR_1.Factories;
using KR_1.Domain;

namespace KR_1_Tests;

public class DomainFactoryTests
{
    [Fact]
    public void CreateBankAccount_ReturnsValidAccount()
    {
        // Act
        var account = DomainFactory.CreateBankAccount("Test Account", 100m);
            
        // Assert
        Assert.NotEqual(Guid.Empty, account.Id);
        Assert.Equal("Test Account", account.Name);
        Assert.Equal(100m, account.Balance);
    }
    
    [Fact]
    public void CreateCategory_ReturnsValidCategory()
    {
        // Act
        var category = DomainFactory.CreateCategory("Test Category", TransactionType.Income);
            
        // Assert
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.Equal("Test Category", category.Name);
        Assert.Equal(TransactionType.Income, category.Type);
    }
    
    [Fact]
    public void CreateOperation_AdjustsAccountBalanceCorrectly()
    {
        // Arrange
        var account = DomainFactory.CreateBankAccount("Test Account", 100m);
        var category = DomainFactory.CreateCategory("Income", TransactionType.Income);
            
        // Act
        var operation = DomainFactory.CreateOperation(TransactionType.Income, account, 50m, DateTime.Now, "Income", category);
            
        // Assert
        Assert.Equal(150m, account.Balance);
    }
    
    [Fact]
    public void CreateOperation_WithNegativeAmount_ThrowsException()
    {
        // Arrange
        var account = DomainFactory.CreateBankAccount("Test Account", 100m);
        var category = DomainFactory.CreateCategory("Expense", TransactionType.Expense);
            
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        DomainFactory.CreateOperation(TransactionType.Expense, account, -30m, DateTime.Now, "Expense", category));
    }
}