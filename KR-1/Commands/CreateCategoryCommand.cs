using KR_1.Facades;
using KR_1.Domain;

namespace KR_1.Commands;

public class CreateCategoryCommand : ICommand
{
    private readonly CategoryFacade _categoryFacade;
    private readonly string _name;
    private readonly TransactionType _type;

    public CreateCategoryCommand(CategoryFacade categoryFacade, string name, TransactionType type)
    {
        _categoryFacade = categoryFacade;
        _name = name;
        _type = type;
    }

    public void Execute()
    {
        var category = _categoryFacade.CreateCategory(_name, _type);
        Console.WriteLine($"Категория создана: {category.Name} ({category.Type})");
    }
}