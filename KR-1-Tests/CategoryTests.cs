using KR_1.Domain;
using KR_1_Tests.Mock_objects;

namespace KR_1_Tests;

public class CategoryTests
{
    [Fact]
    public void Category_StoresPropertiesCorrectly()
    {
        // Arrange & Act
        var category = new Category(Guid.NewGuid(), TransactionType.Expense, "Food");
            
        // Assert
        Assert.Equal("Food", category.Name);
        Assert.Equal(TransactionType.Expense, category.Type);
    }
    
    [Fact]
    public void Accept_CallsVisitorMethod()
    {
        // Arrange
        var category = new Category(Guid.NewGuid(), TransactionType.Income, "Salary");
        var visitor = new TestVisitor();
            
        // Act
        category.Accept(visitor);
            
        // Assert
        Assert.True(visitor.CategoryVisited);
    }
}