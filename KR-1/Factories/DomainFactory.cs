using KR_1.Domain;

namespace KR_1.Factories;

public static class DomainFactory
{
    public static BankAccount CreateBankAccount(string name, decimal initialBalance)
    {
        return new BankAccount(Guid.NewGuid(), name, initialBalance);
    }

    public static Category CreateCategory(string name, TransactionType type)
    {
        return new Category(Guid.NewGuid(), type, name);
    }

    public static Operation CreateOperation(TransactionType type, BankAccount account, decimal amount, DateTime date, string description, Category category)
    {
        if (amount < 0)
            throw new ArgumentException("Сумма операции не может быть отрицательной");

        if (type == TransactionType.Income)
            account.AdjustBalance(amount);
        else if (type == TransactionType.Expense)
            account.AdjustBalance(-amount);

        return new Operation(Guid.NewGuid(), type, account.Id, amount, date, description, category.Id);
    }
}