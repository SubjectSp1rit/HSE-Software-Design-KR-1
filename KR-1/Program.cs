using Microsoft.Extensions.DependencyInjection;
using KR_1.Data;
using KR_1.Repositories;
using KR_1.Facades;
using KR_1.Commands;
using KR_1.Importers;
using KR_1.Domain.Visitors;
using KR_1.Domain;
using KR_1.UI;

namespace KR_1;

class Program
{
    static void Main()
    {
        // DI
        var serviceCollection = new ServiceCollection();
        
        serviceCollection.AddSingleton<DataContext>();
        
        serviceCollection.AddTransient<BankAccountRepository>();
        serviceCollection.AddTransient<CategoryRepository>();
        serviceCollection.AddTransient<OperationRepository>();
        
        serviceCollection.AddSingleton<IRepository<BankAccount>>(sp =>
            new RepositoryProxy<BankAccount>(sp.GetRequiredService<BankAccountRepository>()));
        serviceCollection.AddSingleton<IRepository<Category>>(sp =>
            new RepositoryProxy<Category>(sp.GetRequiredService<CategoryRepository>()));
        serviceCollection.AddSingleton<IRepository<Operation>>(sp =>
            new RepositoryProxy<Operation>(sp.GetRequiredService<OperationRepository>()));
        
        serviceCollection.AddSingleton<BankAccountFacade>();
        serviceCollection.AddSingleton<CategoryFacade>();
        serviceCollection.AddSingleton<OperationFacade>();
        serviceCollection.AddSingleton<AnalyticsFacade>();
        serviceCollection.AddSingleton<DataManagementFacade>();
        
        serviceCollection.AddTransient<CSVImporter>();
        serviceCollection.AddTransient<YAMLImporter>();
        serviceCollection.AddTransient<JSONImporter>();
        
        serviceCollection.AddTransient<CSVExportVisitor>();
        serviceCollection.AddTransient<YAMLExportVisitor>();
        serviceCollection.AddTransient<JSONExportVisitor>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Начало работы (сначала получаем данные)
        var bankAccountFacade = serviceProvider.GetRequiredService<BankAccountFacade>();
        var categoryFacade = serviceProvider.GetRequiredService<CategoryFacade>();
        var operationFacade = serviceProvider.GetRequiredService<OperationFacade>();
        var analyticsFacade = serviceProvider.GetRequiredService<AnalyticsFacade>();
        var dataManagementFacade = serviceProvider.GetRequiredService<DataManagementFacade>();
        
        // стартовые данные
        bankAccountFacade.CreateBankAccount("Основной счет", 1000m);
        bankAccountFacade.CreateBankAccount("Накопительный счет", 500m);
        categoryFacade.CreateCategory("Зарплата", TransactionType.Income);
        categoryFacade.CreateCategory("Кафе", TransactionType.Expense);
        
        bool exit = false;
        while (!exit)
        {
            var menuOptions = new List<string>
            {
                "Создать счет",
                "Создать категорию",
                "Создать операцию",
                "Аналитика (разница доходов и расходов за период)",
                "Пересчитать баланс счета",
                "Экспорт данных",
                "Импорт данных",
                "Вывести все счета, категории, операции",
                "Выход"
            };

            var consoleMenu = new ConsoleMenu(menuOptions);
            int selected = consoleMenu.Show();

            Console.Clear();
            try
            {
                switch (selected)
                {
                    case 0:
                    {
                        Console.Write("Введите название счета: ");
                        string name = Console.ReadLine();
                        Console.Write("Введите начальный баланс: ");
                        decimal balance = decimal.Parse(Console.ReadLine());
                        ICommand createAccountCommand = new CreateBankAccountCommand(bankAccountFacade, name, balance);
                        ICommand timedCommand = new TimingCommandDecorator(createAccountCommand);
                        timedCommand.Execute();
                    }
                        break;
                    case 1:
                    {
                        Console.Write("Введите название категории: ");
                        string name = Console.ReadLine();
                        Console.Write("Введите тип (0 – Income, 1 – Expense): ");
                        TransactionType type = (TransactionType)int.Parse(Console.ReadLine());
                        ICommand createCategoryCommand = new CreateCategoryCommand(categoryFacade, name, type);
                        ICommand timedCommand = new TimingCommandDecorator(createCategoryCommand);
                        timedCommand.Execute();
                    }
                        break;
                    case 2:
                    {
                        Console.WriteLine("Доступные счета:");
                        foreach (var acc in bankAccountFacade.GetAllBankAccounts())
                            Console.WriteLine($"{acc.Id} – {acc.Name} (баланс {acc.Balance})");

                        Console.Write("Введите Id счета: ");
                        Guid accountId = Guid.Parse(Console.ReadLine());

                        Console.WriteLine("Доступные категории:");
                        foreach (var cat in categoryFacade.GetAllCategories())
                            Console.WriteLine($"{cat.Id} – {cat.Name} ({cat.Type})");

                        Console.Write("Введите Id категории: ");
                        Guid categoryId = Guid.Parse(Console.ReadLine());

                        Console.Write("Введите тип операции (0 – Income, 1 – Expense): ");
                        TransactionType opType = (TransactionType)int.Parse(Console.ReadLine());
                        Console.Write("Введите сумму операции: ");
                        decimal amount = decimal.Parse(Console.ReadLine());
                        Console.Write("Введите описание операции: ");
                        string description = Console.ReadLine();
                        DateTime date = DateTime.Now;

                        ICommand createOperationCommand = new CreateOperationCommand(operationFacade, bankAccountFacade,
                            categoryFacade,
                            accountId, categoryId, opType, amount, date, description);
                        ICommand timedCommand = new TimingCommandDecorator(createOperationCommand);
                        timedCommand.Execute();
                    }
                        break;
                    case 3:
                    {
                        Console.Write("Введите дату начала периода (yyyy-MM-dd): ");
                        DateTime from = DateTime.Parse(Console.ReadLine());
                        Console.Write("Введите дату окончания периода (yyyy-MM-dd): ");
                        DateTime to = DateTime.Parse(Console.ReadLine());
                        var diff = analyticsFacade.CalculateIncomeExpenseDifference(from, to);
                        Console.WriteLine($"Разница доходов и расходов за период: {diff}");
                    }
                        break;
                    case 4:
                    {
                        Console.WriteLine("Доступные счета:");
                        foreach (var acc in bankAccountFacade.GetAllBankAccounts())
                            Console.WriteLine($"{acc.Id} – {acc.Name} (баланс {acc.Balance})");
                        Console.Write("Введите Id счета для пересчёта баланса: ");
                        Guid accId = Guid.Parse(Console.ReadLine());
                        dataManagementFacade.RecalculateBalance(accId);
                    }
                        break;
                    case 5:
                    {
                        Console.WriteLine("Экспорт данных в формате CSV:");
                        var csvVisitor = serviceProvider.GetRequiredService<CSVExportVisitor>();
                        foreach (var acc in bankAccountFacade.GetAllBankAccounts())
                            acc.Accept(csvVisitor);
                        foreach (var cat in categoryFacade.GetAllCategories())
                            cat.Accept(csvVisitor);
                        foreach (var op in operationFacade.GetAllOperations())
                            op.Accept(csvVisitor);
                    }
                        break;
                    case 6:
                    {
                        Console.Write("Введите путь к файлу для импорта: ");
                        string filePath = Console.ReadLine();
                        Console.Write("Выберите формат (CSV/YAML/JSON): ");
                        string format = Console.ReadLine().ToUpper();
                        DataImporter importer = format switch
                        {
                            "CSV" => serviceProvider.GetRequiredService<CSVImporter>(),
                            "YAML" => serviceProvider.GetRequiredService<YAMLImporter>(),
                            "JSON" => serviceProvider.GetRequiredService<JSONImporter>(),
                            _ => null
                        };
                        if (importer != null)
                            importer.Import(filePath);
                        else
                            Console.WriteLine("Неверный формат");
                    }
                        break;
                    case 7:
                    {
                        Console.WriteLine("Счета:");
                        foreach (var acc in bankAccountFacade.GetAllBankAccounts())
                            Console.WriteLine($"{acc.Id} – {acc.Name} (баланс {acc.Balance})");
                        Console.WriteLine("Категории:");
                        foreach (var cat in categoryFacade.GetAllCategories())
                            Console.WriteLine($"{cat.Id} – {cat.Name} ({cat.Type})");
                        Console.WriteLine("Операции:");
                        foreach (var op in operationFacade.GetAllOperations())
                            Console.WriteLine($"{op.Id} – {op.Type} на сумму {op.Amount} от {op.Date}");
                    }
                        break;
                    case 8:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверная команда");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Ошибка типа данных: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Неизвестная ошибка: " + ex.Message);
            }
            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить");
            Console.ReadKey();
        }
    }
}
