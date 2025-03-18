using KR_1.Repositories;
using KR_1.Data;
using KR_1.Domain;
using KR_1.Facades;
using KR_1.Factories;

namespace KR_1_Tests;

public class FacadeTests
{
    private DataContext GetTestDataContext() => new DataContext();
    
    [Fact]
    public void BankAccountFacade_CreateAndRetrieve_WorksCorrectly()
    {
        // Arrange
        var context = GetTestDataContext();
        var repo = new RepositoryProxy<BankAccount>(new BankAccountRepository(context));
        var facade = new BankAccountFacade(repo);

        // Act
        var account = facade.CreateBankAccount("Facade Test", 300m);
        var accounts = facade.GetAllBankAccounts().ToList();

        // Assert
        Assert.Single(accounts);
        Assert.Equal("Facade Test", accounts[0].Name);
    }
    
    [Fact]
    public void CategoryFacade_CreateAndRetrieve_WorksCorrectly()
    {
        // Arrange
        var context = GetTestDataContext();
        var repo = new RepositoryProxy<Category>(new CategoryRepository(context));
        var facade = new CategoryFacade(repo);

        // Act
        var category = facade.CreateCategory("Test Cat", TransactionType.Income);
        var categories = facade.GetAllCategories().ToList();

        // Assert
        Assert.Single(categories);
        Assert.Equal("Test Cat", categories[0].Name);
    }
    
    [Fact]
    public void OperationFacade_CreateOperation_AdjustsBalance()
    {
        // Arrange
        var context = GetTestDataContext();
        var bankRepo = new RepositoryProxy<BankAccount>(new BankAccountRepository(context));
        var catRepo = new RepositoryProxy<Category>(new CategoryRepository(context));
        var opRepo = new RepositoryProxy<Operation>(new OperationRepository(context));

        var bankFacade = new BankAccountFacade(bankRepo);
        var catFacade = new CategoryFacade(catRepo);
        var opFacade = new OperationFacade(opRepo, bankFacade, catFacade);

        var account = bankFacade.CreateBankAccount("Op Test", 100m);
        var category = catFacade.CreateCategory("Salary", TransactionType.Income);
            
        // Act
        var operation = opFacade.CreateOperation(TransactionType.Income, account, 50m, DateTime.Now, "Salary Payment", category);
            
        // Assert
        Assert.Equal(150m, account.Balance);
    }
    
    [Fact]
    public void AnalyticsFacade_CalculatesIncomeExpenseDifference()
    {
        // Arrange
        var context = GetTestDataContext();
        var opRepo = new RepositoryProxy<Operation>(new OperationRepository(context));
        var catRepo = new RepositoryProxy<Category>(new CategoryRepository(context));
        var analyticsFacade = new AnalyticsFacade(opRepo, catRepo);
        var now = DateTime.Now;

        opRepo.Add(new Operation(Guid.NewGuid(), TransactionType.Income, Guid.NewGuid(), 200m, now, "Income", Guid.NewGuid()));
        opRepo.Add(new Operation(Guid.NewGuid(), TransactionType.Expense, Guid.NewGuid(), 50m, now, "Expense", Guid.NewGuid()));

        // Act
        var diff = analyticsFacade.CalculateIncomeExpenseDifference(now.AddMinutes(-1), now.AddMinutes(1));

        // Assert
        Assert.Equal(150m, diff);
    }
    
    [Fact]
    public void DataManagementFacade_RecalculatesBalanceCorrectly()
    {
        // Arrange
        var context = GetTestDataContext();
        var bankRepo = new RepositoryProxy<BankAccount>(new BankAccountRepository(context));
        var opRepo = new RepositoryProxy<Operation>(new OperationRepository(context));
        var dataManagementFacade = new DataManagementFacade(bankRepo, opRepo);
        var account = new BankAccount(Guid.NewGuid(), "DM Test", 0m);
        bankRepo.Add(account);
        opRepo.Add(new Operation(Guid.NewGuid(), TransactionType.Income, account.Id, 100m, DateTime.Now, "Income", Guid.NewGuid()));
        opRepo.Add(new Operation(Guid.NewGuid(), TransactionType.Expense, account.Id, 30m, DateTime.Now, "Expense", Guid.NewGuid()));

        // Act
        dataManagementFacade.RecalculateBalance(account.Id);

        // Assert
        Assert.Equal(70m, account.Balance);
    }
    
    [Fact]
    public void AnalyticsFacade_GroupOperationsByCategory_ReturnsCorrectGrouping()
    {
        // Arrange
        var context = new DataContext();
        var opRepo = new RepositoryProxy<Operation>(new OperationRepository(context));
        var catRepo = new RepositoryProxy<Category>(new CategoryRepository(context));
        var analyticsFacade = new AnalyticsFacade(opRepo, catRepo);
        
        var cat1 = new Category(Guid.NewGuid(), TransactionType.Expense, "Food");
        var cat2 = new Category(Guid.NewGuid(), TransactionType.Expense, "Transport");
        
        var op1 = new Operation(Guid.NewGuid(), TransactionType.Expense, Guid.NewGuid(), 10m, DateTime.Now, "Lunch", cat1.Id);
        var op2 = new Operation(Guid.NewGuid(), TransactionType.Expense, Guid.NewGuid(), 15m, DateTime.Now, "Dinner", cat1.Id);
        var op3 = new Operation(Guid.NewGuid(), TransactionType.Expense, Guid.NewGuid(), 5m, DateTime.Now, "Bus", cat2.Id);

        opRepo.Add(op1);
        opRepo.Add(op2);
        opRepo.Add(op3);

        // Act
        var groups = analyticsFacade.GroupOperationsByCategory().ToList();

        // Assert
        Assert.Equal(2, groups.Count);
        var group1 = groups.FirstOrDefault(g => g.Key == cat1.Id);
        var group2 = groups.FirstOrDefault(g => g.Key == cat2.Id);
        Assert.NotNull(group1);
        Assert.NotNull(group2);
        Assert.Equal(2, group1.Count());
        Assert.Single(group2);
    }
    
    [Fact]
    public void OperationFacade_DeleteOperation_RemovesOperation()
    {
        // Arrange
        var context = new DataContext();
        var bankRepo = new RepositoryProxy<BankAccount>(new BankAccountRepository(context));
        var catRepo = new RepositoryProxy<Category>(new CategoryRepository(context));
        var opRepo = new RepositoryProxy<Operation>(new OperationRepository(context));

        var bankFacade = new BankAccountFacade(bankRepo);
        var catFacade = new CategoryFacade(catRepo);
        var opFacade = new OperationFacade(opRepo, bankFacade, catFacade);

        var account = bankFacade.CreateBankAccount("Test Delete", 100m);
        var category = catFacade.CreateCategory("Test Cat", TransactionType.Income);
        var operation = opFacade.CreateOperation(TransactionType.Income, account, 50m, DateTime.Now, "Test Operation", category);
        
        Assert.Single(opFacade.GetAllOperations());

        // Act
        opFacade.DeleteOperation(operation.Id);

        // Assert
        Assert.Empty(opFacade.GetAllOperations());
    }
    
    [Fact]
    public void BankAccountFacade_DeleteBankAccount_RemovesAccount()
    {
        // Arrange
        var context = new DataContext();
        var repo = new RepositoryProxy<BankAccount>(new BankAccountRepository(context));
        var facade = new BankAccountFacade(repo);

        var account = facade.CreateBankAccount("ToDelete", 200m);
        Assert.Single(facade.GetAllBankAccounts());

        // Act
        facade.DeleteBankAccount(account.Id);

        // Assert
        Assert.Empty(facade.GetAllBankAccounts());
    }
    
    [Fact]
    public void CategoryFacade_DeleteCategory_RemovesCategory()
    {
        // Arrange
        var context = new DataContext();
        var repo = new RepositoryProxy<Category>(new CategoryRepository(context));
        var facade = new CategoryFacade(repo);

        var category = facade.CreateCategory("ToDelete", TransactionType.Expense);
        Assert.Single(facade.GetAllCategories());

        // Act
        facade.DeleteCategory(category.Id);

        // Assert
        Assert.Empty(facade.GetAllCategories());
    }
    
    [Fact]
    public void CategoryFacade_UpdateCategory_ChangesCategoryData()
    {
        // Arrange
        var context = new DataContext();
        var repo = new RepositoryProxy<Category>(new CategoryRepository(context));
        var facade = new CategoryFacade(repo);

        var category = facade.CreateCategory("OldName", TransactionType.Income);
        // Act
        var updatedCategory = new Category(category.Id, category.Type, "NewName");
        facade.UpdateCategory(updatedCategory);

        // Assert
        var fetched = facade.GetAllCategories().FirstOrDefault(c => c.Id == category.Id);
        Assert.NotNull(fetched);
        Assert.Equal("NewName", fetched.Name);
    }
    
    [Fact]
    public void BankAccountFacade_UpdateBankAccount_UpdatesAccountProperly()
    {
        // Arrange
        var context = new DataContext();
        var repo = new RepositoryProxy<BankAccount>(new BankAccountRepository(context));
        var facade = new BankAccountFacade(repo);

        var account = facade.CreateBankAccount("InitialName", 300m);
        // Act
        account.Name = "UpdatedName";
        facade.UpdateBankAccount(account);

        // Assert
        var fetched = facade.GetAllBankAccounts().FirstOrDefault(a => a.Id == account.Id);
        Assert.NotNull(fetched);
        Assert.Equal("UpdatedName", fetched.Name);
    }
    
    [Fact]
    public void CreateBankAccount_ReturnsUniqueAccounts()
    {
        // Arrange & Act
        var account1 = DomainFactory.CreateBankAccount("Account1", 100m);
        var account2 = DomainFactory.CreateBankAccount("Account2", 200m);

        // Assert
        Assert.NotEqual(account1.Id, account2.Id);
        Assert.Equal("Account1", account1.Name);
        Assert.Equal("Account2", account2.Name);
    }
    
    [Fact]
    public void CreateCategory_ReturnsUniqueCategories()
    {
        // Arrange & Act
        var category1 = DomainFactory.CreateCategory("Category1", TransactionType.Income);
        var category2 = DomainFactory.CreateCategory("Category2", TransactionType.Expense);

        // Assert
        Assert.NotEqual(category1.Id, category2.Id);
        Assert.Equal("Category1", category1.Name);
        Assert.Equal(TransactionType.Income, category1.Type);
        Assert.Equal("Category2", category2.Name);
        Assert.Equal(TransactionType.Expense, category2.Type);
    }
}