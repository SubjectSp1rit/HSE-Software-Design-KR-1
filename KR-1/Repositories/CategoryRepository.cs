using KR_1.Domain;
using KR_1.Data;

namespace KR_1.Repositories;

public class CategoryRepository : IRepository<Category>
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }

    public void Add(Category item)
    {
        _context.Categories.Add(item);
    }

    public void Update(Category item)
    {
        var index = _context.Categories.FindIndex(x => x.Id == item.Id);
        if (index >= 0)
            _context.Categories[index] = item;
    }

    public void Delete(Guid id)
    {
        _context.Categories.RemoveAll(x => x.Id == id);
    }

    public Category Get(Guid id)
    {
        return _context.Categories.FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<Category> GetAll()
    {
        return _context.Categories;
    }
}