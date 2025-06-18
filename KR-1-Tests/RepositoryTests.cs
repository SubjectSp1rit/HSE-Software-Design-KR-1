using KR_1.Repositories;
using KR_1.Data;
using KR_1.Domain;

namespace KR_1_Tests;

public class RepositoryTests
{
    private DataContext GetTestDataContext() => new DataContext();
    
    [Fact]
    public void BankAccountRepository_AddGetUpdateDelete_WorksCorrectly()
    {
        // Arrange
        var context = GetTestDataContext();
        var repository = new BankAccountRepository(context);
        var account = new BankAccount(Guid.NewGuid(), "Test", 100m);

        // Act & Assert
        repository.Add(account);
        var fetched = repository.Get(account.Id);
        Assert.NotNull(fetched);
        Assert.Equal("Test", fetched.Name);

        account.Name = "Updated";
        repository.Update(account);
        fetched = repository.Get(account.Id);
        Assert.Equal("Updated", fetched.Name);

        repository.Delete(account.Id);
        fetched = repository.Get(account.Id);
        Assert.Null(fetched);
    }
    
    [Fact]
    public void DataContext_InitializesEmptyLists()
    {
        // Arrange & Act
        var context = new DataContext();

        // Assert
        Assert.NotNull(context.BankAccounts);
        Assert.NotNull(context.Categories);
        Assert.NotNull(context.Operations);
        Assert.Empty(context.BankAccounts);
        Assert.Empty(context.Categories);
        Assert.Empty(context.Operations);
    }
    
    [Fact]
    public void DataContext_AllowsAddingEntities()
    {
        // Arrange
        var context = new DataContext();
        var bankAccount = new BankAccount(Guid.NewGuid(), "Test Account", 0m);
        var category = new Category(Guid.NewGuid(), TransactionType.Income, "Test Category");
        var operation = new Operation(Guid.NewGuid(), TransactionType.Expense, Guid.NewGuid(), 100m, DateTime.Now, "Test Operation", Guid.NewGuid());

        // Act
        context.BankAccounts.Add(bankAccount);
        context.Categories.Add(category);
        context.Operations.Add(operation);

        // Assert
        Assert.Single(context.BankAccounts);
        Assert.Single(context.Categories);
        Assert.Single(context.Operations);
    }

    [Fact]
    public void CategoryRepository_AddGetUpdateDelete_WorksCorrectly()
    {
        // Arrange
        var context = GetTestDataContext();
        var repository = new CategoryRepository(context);
        var category = new Category(Guid.NewGuid(), TransactionType.Expense, "Food");

        // Act & Assert
        repository.Add(category);
        var fetched = repository.Get(category.Id);
        Assert.NotNull(fetched);
        Assert.Equal("Food", fetched.Name);
        
        var updatedCategory = new Category(category.Id, TransactionType.Expense, "Updated Food");
        repository.Update(updatedCategory);
        fetched = repository.Get(category.Id);
        Assert.Equal("Updated Food", fetched.Name);

        repository.Delete(category.Id);
        fetched = repository.Get(category.Id);
        Assert.Null(fetched);
    }

    [Fact]
    public void OperationRepository_AddGetUpdateDelete_WorksCorrectly()
    {
        // Arrange
        var context = GetTestDataContext();
        var repository = new OperationRepository(context);
        var operation = new Operation(Guid.NewGuid(), TransactionType.Income, Guid.NewGuid(), 100m, DateTime.Now, "Test", Guid.NewGuid());

        // Act & Assert
        repository.Add(operation);
        var fetched = repository.Get(operation.Id);
        Assert.NotNull(fetched);
        Assert.Equal(100m, fetched.Amount);

        var updatedOperation = new Operation(operation.Id, TransactionType.Income, operation.BankAccountId, 150m, operation.Date, "Test Updated", operation.CategoryId);
        repository.Update(updatedOperation);
        fetched = repository.Get(operation.Id);
        Assert.Equal(150m, fetched.Amount);

        repository.Delete(operation.Id);
        fetched = repository.Get(operation.Id);
        Assert.Null(fetched);
    }
    
    [Fact]
    public void RepositoryProxy_CachesDataProperly()
    {
        // Arrange
        var context = GetTestDataContext();
        var innerRepo = new BankAccountRepository(context);
        var proxy = new RepositoryProxy<BankAccount>(innerRepo);
        var account = new BankAccount(Guid.NewGuid(), "CacheTest", 200m);

        // Act & Assert
        proxy.Add(account);
        var fetched1 = proxy.Get(account.Id);
        Assert.Equal("CacheTest", fetched1.Name);
        
        account.Name = "Changed";
        innerRepo.Update(account);

        var fetched2 = proxy.Get(account.Id);
        
        Assert.Equal("Changed", fetched2.Name);
    }
}