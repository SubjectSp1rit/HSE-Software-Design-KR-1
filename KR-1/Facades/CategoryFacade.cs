using KR_1.Domain;
using KR_1.Repositories;
using KR_1.Factories;

namespace KR_1.Facades;

public class CategoryFacade
{
    private readonly IRepository<Category> _repository;

    public CategoryFacade(IRepository<Category> repository)
    {
        _repository = repository;
    }

    public Category CreateCategory(string name, TransactionType type)
    {
        var category = DomainFactory.CreateCategory(name, type);
        _repository.Add(category);
        return category;
    }

    public void UpdateCategory(Category category)
    {
        _repository.Update(category);
    }

    public void DeleteCategory(Guid id)
    {
        _repository.Delete(id);
    }

    public IEnumerable<Category> GetAllCategories()
    {
        return _repository.GetAll();
    }
}
