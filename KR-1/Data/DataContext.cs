using KR_1.Domain;

namespace KR_1.Data;

public class DataContext
{
    public List<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    public List<Category> Categories { get; set; } = new List<Category>();
    public List<Operation> Operations { get; set; } = new List<Operation>();
}