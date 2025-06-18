using KR_1.Domain;
using KR_1.Commands;
using KR_1.Repositories;
using KR_1.Facades;
using KR_1.Data;
using System.Diagnostics;
using KR_1_Tests.Mock_objects;

namespace KR_1_Tests;

public class CommandTests
{
    [Fact]
    public void CreateBankAccountCommand_ExecutesAndCreatesAccount()
    {
        // Arrange
        var context = new DataContext();
        var repo = new RepositoryProxy<BankAccount>(new BankAccountRepository(context));
        var facade = new BankAccountFacade(repo);
        var command = new CreateBankAccountCommand(facade, "Cmd Test", 500m);

        // Act
        command.Execute();

        // Assert
        var accounts = facade.GetAllBankAccounts().ToList();
        Assert.Single(accounts);
        Assert.Equal("Cmd Test", accounts[0].Name);
    }
    
    [Fact]
    public void CreateCategoryCommand_ExecutesAndCreatesCategory()
    {
        // Arrange
        var context = new DataContext();
        var repo = new RepositoryProxy<Category>(new CategoryRepository(context));
        var facade = new CategoryFacade(repo);
        var command = new CreateCategoryCommand(facade, "Cmd Cat", TransactionType.Expense);

        // Act
        command.Execute();

        // Assert
        var categories = facade.GetAllCategories().ToList();
        Assert.Single(categories);
        Assert.Equal("Cmd Cat", categories[0].Name);
    }
    
    [Fact]
    public void CreateOperationCommand_ExecutesAndAdjustsBalance()
    {
        // Arrange
        var context = new DataContext();
        var bankRepo = new RepositoryProxy<BankAccount>(new BankAccountRepository(context));
        var catRepo = new RepositoryProxy<Category>(new CategoryRepository(context));
        var opRepo = new RepositoryProxy<Operation>(new OperationRepository(context));

        var bankFacade = new BankAccountFacade(bankRepo);
        var catFacade = new CategoryFacade(catRepo);
        var opFacade = new OperationFacade(opRepo, bankFacade, catFacade);

        var account = bankFacade.CreateBankAccount("Cmd Op", 100m);
        var category = catFacade.CreateCategory("Cmd Cat", TransactionType.Income);
        var command = new CreateOperationCommand(opFacade, bankFacade, catFacade, account.Id, category.Id, TransactionType.Income, 50m, DateTime.Now, "Cmd Operation");

        // Act
        command.Execute();

        // Assert
        Assert.Equal(150m, account.Balance);
        var operations = opFacade.GetAllOperations().ToList();
        Assert.Single(operations);
    }
    
    [Fact]
    public void TimingCommandDecorator_WrapsCommandWithoutError()
    {
        // Arrange
        var command = new TestCommand();
        var timedCommand = new TimingCommandDecorator(command);
        var sw = Stopwatch.StartNew();

        // Act
        timedCommand.Execute();
        sw.Stop();

        // Assert
        Assert.True(command.Executed);
        Assert.True(sw.ElapsedMilliseconds >= 0);
    }
}