using KR_1.Domain;
using KR_1_Tests.Mock_objects;

namespace KR_1_Tests;

public class BankAccountTests
{
    [Fact]
    public void AdjustBalance_AddsAmountCorrectly()
    {
        // Arrange
        var account = new BankAccount(Guid.NewGuid(), "Test Account", 100m);
            
        // Act
        account.AdjustBalance(50m);
            
        // Assert
        Assert.Equal(150m, account.Balance);
    }
    
    [Fact]
    public void SetBalance_OverridesBalanceCorrectly()
    {
        // Arrange
        var account = new BankAccount(Guid.NewGuid(), "Test Account", 100m);
            
        // Act
        account.SetBalance(200m);
            
        // Assert
        Assert.Equal(200m, account.Balance);
    }

    [Fact]
    public void Accept_CallsVisitorMethod()
    {
        // Arrange
        var account = new BankAccount(Guid.NewGuid(), "Test Account", 100m);
        var visitor = new TestVisitor();

        // Act
        account.Accept(visitor);

        // Assert
        Assert.True(visitor.BankAccountVisited);
    }
}